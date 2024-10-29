using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(AnimationEventStateBehaviour))]
public class AnimationEventStateBehaviourEditor : Editor
{
    List<AnimationClip> extractedClips = new List<AnimationClip>();
    string[] clipNames;

    Animator animator;

    private static AnimationEventStateBehaviour currentlyPreviewingBehaviour = null;

    private List<GameObject> previewClones = new List<GameObject>();

    // Keep track of the event index for which the preview is active
    private int currentEventIndex = -1;

    // Cleanup clones when scripts recompile or the editor reloads
    [InitializeOnLoadMethod]
    static void CleanupOnRecompile()
    {
        // Find all clones in the scene and destroy them
        var clones = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(go => go.name.Contains("_PreviewClone_"))
            .ToArray();

        foreach (var clone in clones)
        {
            GameObject.DestroyImmediate(clone);
        }

        // Reset the static reference
        currentlyPreviewingBehaviour = null;
    }

    [MenuItem("GameObject/Enforce T-Pose", false, 0)]
    static void EnforceTPose()
    {
        GameObject selected = Selection.activeGameObject;
        if (!selected || !selected.TryGetComponent(out Animator animator) || !animator.avatar) return;

        SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton;

        foreach (HumanBodyBones hbb in Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (hbb == HumanBodyBones.LastBone) continue;

            Transform boneTransform = animator.GetBoneTransform(hbb);
            if (!boneTransform) continue;

            SkeletonBone skeletonBone = skeletonBones.FirstOrDefault(sb => sb.name == boneTransform.name);
            if (skeletonBone.name == null) continue;

            if (hbb == HumanBodyBones.Hips) boneTransform.localPosition = skeletonBone.position;
            boneTransform.localRotation = skeletonBone.rotation;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AnimationEventStateBehaviour stateBehaviour = (AnimationEventStateBehaviour)target;

        SerializedProperty eventsProp = serializedObject.FindProperty("events");
        SerializedProperty isPreviewingProp = serializedObject.FindProperty("isPreviewing");

        if (Validate(stateBehaviour, out string errorMessage))
        {
            GUILayout.Space(10);

            // Use the foldout field from the instance
            stateBehaviour.foldout = EditorGUILayout.Foldout(stateBehaviour.foldout, "Animation Event State Behaviour", true);

            if (stateBehaviour.foldout)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Animation Events", EditorStyles.boldLabel);

                // Draw events list with per-event settings
                for (int i = 0; i < eventsProp.arraySize; i++)
                {
                    SerializedProperty eventProp = eventsProp.GetArrayElementAtIndex(i);
                    SerializedProperty eventNameProp = eventProp.FindPropertyRelative("eventName");
                    SerializedProperty triggerTimeProp = eventProp.FindPropertyRelative("triggerTime");
                    SerializedProperty clipsMaskProp = eventProp.FindPropertyRelative("clipsMask");
                    SerializedProperty cloneFormationProp = eventProp.FindPropertyRelative("cloneFormation");
                    SerializedProperty cloneSpacingProp = eventProp.FindPropertyRelative("cloneSpacing");
                    SerializedProperty useBatchSliderProp = eventProp.FindPropertyRelative("useBatchSlider");
                    SerializedProperty batchTriggerTimeProp = eventProp.FindPropertyRelative("batchTriggerTime");
                    SerializedProperty individualTriggerTimesProp = eventProp.FindPropertyRelative("individualTriggerTimes");

                    EditorGUILayout.BeginVertical("box");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Event {i + 1}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Remove"))
                    {
                        eventsProp.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.PropertyField(eventNameProp, new GUIContent("Event Name"));

                    // Clips Mask Field
                    string[] clipOptions = clipNames;
                    int newEventClipsMask = EditorGUILayout.MaskField("Apply To Clips", clipsMaskProp.intValue, clipOptions);
                    clipsMaskProp.intValue = newEventClipsMask;

                    bool settingsChanged = EditorGUI.EndChangeCheck();
                    bool slidersChanged = false;

                    // Update individualTriggerTimes when clipsMask changes
                    if (settingsChanged)
                    {
                        List<int> selectedClipIndicesForUpdate = GetSelectedClipIndices(clipsMaskProp.intValue);
                        SerializedProperty individualTriggerTimesListForUpdate = individualTriggerTimesProp.FindPropertyRelative("Array");

                        while (individualTriggerTimesListForUpdate.arraySize < selectedClipIndicesForUpdate.Count)
                        {
                            individualTriggerTimesListForUpdate.InsertArrayElementAtIndex(individualTriggerTimesListForUpdate.arraySize);
                            individualTriggerTimesListForUpdate.GetArrayElementAtIndex(individualTriggerTimesListForUpdate.arraySize - 1).floatValue = 0f;
                        }
                        while (individualTriggerTimesListForUpdate.arraySize > selectedClipIndicesForUpdate.Count)
                        {
                            individualTriggerTimesListForUpdate.DeleteArrayElementAtIndex(individualTriggerTimesListForUpdate.arraySize - 1);
                        }
                    }

                    EditorGUILayout.Space();

                    // Formation selection
                    CloneFormation cloneFormation = (CloneFormation)cloneFormationProp.enumValueIndex;
                    cloneFormation = (CloneFormation)EditorGUILayout.EnumPopup("Clone Formation", cloneFormation);
                    cloneFormationProp.enumValueIndex = (int)cloneFormation;

                    // Spacing
                    float cloneSpacing = cloneSpacingProp.floatValue;
                    cloneSpacing = EditorGUILayout.FloatField("Clone Spacing", cloneSpacing);
                    cloneSpacingProp.floatValue = cloneSpacing;

                    EditorGUILayout.Space();

                    // Toggle between batch and individual sliders
                    bool useBatchSlider = useBatchSliderProp.boolValue;
                    useBatchSlider = EditorGUILayout.Toggle("Use Batch Slider", useBatchSlider);
                    useBatchSliderProp.boolValue = useBatchSlider;

                    // Get the indices of selected clips
                    List<int> selectedClipIndices = GetSelectedClipIndices(clipsMaskProp.intValue);

                    if (selectedClipIndices.Count > 0)
                    {
                        if (useBatchSlider)
                        {
                            // Single slider to control time
                            EditorGUI.BeginChangeCheck();
                            float batchTriggerTime = batchTriggerTimeProp.floatValue;
                            batchTriggerTime = EditorGUILayout.Slider("Batch Trigger Time", batchTriggerTime, 0f, 1f);
                            batchTriggerTimeProp.floatValue = batchTriggerTime;
                            if (EditorGUI.EndChangeCheck())
                            {
                                slidersChanged = true;
                                triggerTimeProp.floatValue = batchTriggerTime;
                            }
                        }
                        else
                        {
                            // Individual sliders for each selected clip
                            SerializedProperty individualTriggerTimesList = individualTriggerTimesProp.FindPropertyRelative("Array");

                            // Ensure the list has the correct number of entries
                            while (individualTriggerTimesList.arraySize < selectedClipIndices.Count)
                            {
                                individualTriggerTimesList.InsertArrayElementAtIndex(individualTriggerTimesList.arraySize);
                                individualTriggerTimesList.GetArrayElementAtIndex(individualTriggerTimesList.arraySize - 1).floatValue = 0f;
                            }
                            while (individualTriggerTimesList.arraySize > selectedClipIndices.Count)
                            {
                                individualTriggerTimesList.DeleteArrayElementAtIndex(individualTriggerTimesList.arraySize - 1);
                            }

                            float averageTriggerTime = 0f;

                            for (int j = 0; j < selectedClipIndices.Count; j++)
                            {
                                int clipIndex = selectedClipIndices[j];
                                string clipName = clipNames[clipIndex];

                                SerializedProperty triggerTimeElement = individualTriggerTimesList.GetArrayElementAtIndex(j);
                                EditorGUI.BeginChangeCheck();
                                float triggerTime = triggerTimeElement.floatValue;
                                triggerTime = EditorGUILayout.Slider($"{clipName} Trigger Time", triggerTime, 0f, 1f);
                                triggerTimeElement.floatValue = triggerTime;
                                if (EditorGUI.EndChangeCheck())
                                {
                                    slidersChanged = true;
                                }
                                averageTriggerTime += triggerTime;
                            }

                            if (selectedClipIndices.Count > 0)
                            {
                                // Set the triggerTime to the average of individual trigger times
                                triggerTimeProp.floatValue = averageTriggerTime / selectedClipIndices.Count;
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No clips selected. Please select at least one clip.", MessageType.Warning);
                    }

                    // Check if any sliders changed or settings changed
                    if (slidersChanged || settingsChanged)
                    {
                        // Start preview if not already previewing
                        if (!stateBehaviour.isPreviewing)
                        {
                            currentEventIndex = i;
                            StartPreview(stateBehaviour, isPreviewingProp);
                        }
                        else if (currentEventIndex != i)
                        {
                            // If previewing another event, switch to this one
                            currentEventIndex = i;
                        }

                        // Update the preview
                        PreviewEventClips(stateBehaviour, i);
                    }

                    EditorGUILayout.Space();

                    // Add Preview and Stop Preview buttons per event
                    if (!stateBehaviour.isPreviewing)
                    {
                        if (GUILayout.Button("Preview Event"))
                        {
                            currentEventIndex = i;
                            StartPreview(stateBehaviour, isPreviewingProp);
                            PreviewEventClips(stateBehaviour, i);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Stop Preview Event"))
                        {
                            StopPreview(stateBehaviour, isPreviewingProp);
                            currentEventIndex = -1;
                        }
                    }

                    EditorGUILayout.EndVertical();
                    GUILayout.Space(5);
                }

                if (GUILayout.Button("Add Event"))
                {
                    eventsProp.InsertArrayElementAtIndex(eventsProp.arraySize);
                    SerializedProperty newEventProp = eventsProp.GetArrayElementAtIndex(eventsProp.arraySize - 1);
                    newEventProp.FindPropertyRelative("eventName").stringValue = "";
                    newEventProp.FindPropertyRelative("triggerTime").floatValue = 0f;
                    newEventProp.FindPropertyRelative("clipsMask").intValue = -1; // Select all by default
                    newEventProp.FindPropertyRelative("cloneFormation").enumValueIndex = (int)CloneFormation.Circle;
                    newEventProp.FindPropertyRelative("cloneSpacing").floatValue = 1f;
                    newEventProp.FindPropertyRelative("useBatchSlider").boolValue = true;
                    newEventProp.FindPropertyRelative("batchTriggerTime").floatValue = 0f;
                    newEventProp.FindPropertyRelative("individualTriggerTimes").ClearArray();
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
        }
    }

    private void PreviewEventClips(AnimationEventStateBehaviour stateBehaviour, int eventIndex)
    {
        if (animator == null || extractedClips == null || extractedClips.Count == 0) return;

        // Clean up any existing clones
        CleanupClones();

        // Get the event
        AnimationEventInfo eventInfo = stateBehaviour.events[eventIndex];

        // Get selected clip indices for this event
        List<int> selectedClipIndices = GetSelectedClipIndices(eventInfo.clipsMask);

        if (selectedClipIndices.Count == 0)
        {
            // No clips selected; cannot preview
            return;
        }

        // Generate colors for the number of selected clips
        List<Color> colors = GenerateDistinctColors(selectedClipIndices.Count);

        // Start Animation Mode
        if (!AnimationMode.InAnimationMode())
        {
            AnimationMode.StartAnimationMode();
        }

        // Begin Sampling
        AnimationMode.BeginSampling();

        // Record the original position, rotation, and scale of the animator GameObject
        Vector3 originalPosition = animator.transform.position;
        Quaternion originalRotation = animator.transform.rotation;
        Vector3 originalScale = animator.transform.localScale;

        // First, animate the original GameObject with the first clip
        if (selectedClipIndices.Count > 0)
        {
            int clipIndex = selectedClipIndices[0];
            AnimationClip clip = extractedClips[clipIndex];

            // Temporarily disable root motion to prevent position changes
            bool originalRootMotion = animator.applyRootMotion;
            animator.applyRootMotion = false;

            // Sample the animation clip at the specified time
            float time = eventInfo.useBatchSlider ? eventInfo.batchTriggerTime * clip.length : 0f;
            if (!eventInfo.useBatchSlider && eventInfo.individualTriggerTimes.Count > 0)
            {
                float triggerTime = eventInfo.individualTriggerTimes[0];
                time = triggerTime * clip.length;
            }
            else if (!eventInfo.useBatchSlider)
            {
                time = 0f;
            }

            AnimationMode.SampleAnimationClip(animator.gameObject, clip, time);

            // Restore root motion setting
            animator.applyRootMotion = originalRootMotion;

            // Restore original transform
            animator.transform.position = originalPosition;
            animator.transform.rotation = originalRotation;
            animator.transform.localScale = originalScale;
        }

        // Create clones for the remaining selected clips
        List<GameObject> clonesToPosition = new List<GameObject>();

        for (int i = 1; i < selectedClipIndices.Count; i++)
        {
            int clipIndex = selectedClipIndices[i];
            if (clipIndex < 0 || clipIndex >= extractedClips.Count) continue;

            AnimationClip clip = extractedClips[clipIndex];

            // Instantiate a clone of the animator's GameObject
            GameObject clone = Instantiate(animator.gameObject, animator.transform.parent);
            clone.name = animator.gameObject.name + "_PreviewClone_" + i;
            clone.hideFlags = HideFlags.HideAndDontSave;

            // Disable any animator on the clone to prevent unwanted behavior
            Animator cloneAnimator = clone.GetComponent<Animator>();
            if (cloneAnimator != null)
            {
                cloneAnimator.enabled = false;
            }

            // Apply a transparent material with a distinct color
            Renderer[] renderers = clone.GetComponentsInChildren<Renderer>();
            Color color = colors[i];
            foreach (Renderer renderer in renderers)
            {
                if (renderer.sharedMaterial != null)
                {
                    Material material = new Material(renderer.sharedMaterial);
                    material.color = new Color(color.r, color.g, color.b, 0.5f);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    renderer.sharedMaterial = material;
                }
            }

            // Sample the animation clip at the specified time
            float time;
            if (eventInfo.useBatchSlider)
            {
                time = eventInfo.batchTriggerTime * clip.length;
            }
            else
            {
                int triggerIndex = i;
                if (triggerIndex < eventInfo.individualTriggerTimes.Count)
                {
                    float triggerTime = eventInfo.individualTriggerTimes[triggerIndex];
                    time = triggerTime * clip.length;
                }
                else
                {
                    time = 0f;
                }
            }

            AnimationMode.SampleAnimationClip(clone, clip, time);

            // Add the clone to the list for cleanup and positioning
            previewClones.Add(clone);
            clonesToPosition.Add(clone);
        }

        // End Sampling
        AnimationMode.EndSampling();

        // Position clones according to formation
        PositionClones(clonesToPosition, eventInfo.cloneFormation, eventInfo.cloneSpacing);

        // Repaint the Scene View to reflect the changes
        SceneView.RepaintAll();
    }

    private List<int> GetSelectedClipIndices(int clipsMask)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < clipNames.Length; i++)
        {
            if ((clipsMask & (1 << i)) != 0)
            {
                indices.Add(i);
            }
        }
        return indices;
    }

    private List<Color> GenerateDistinctColors(int count)
    {
        List<Color> colors = new List<Color>();
        float hueStep = 1f / count;
        for (int i = 0; i < count; i++)
        {
            float hue = i * hueStep;
            colors.Add(Color.HSVToRGB(hue, 1f, 1f));
        }
        return colors;
    }

    private void CleanupClones()
    {
        // Destroy clones
        foreach (GameObject clone in previewClones)
        {
            if (clone != null)
            {
                DestroyImmediate(clone);
            }
        }
        previewClones.Clear();

        // Reset the animator's pose to T-pose
        if (animator != null)
        {
            EnforceTPose();

            // Stop Animation Mode on the original GameObject
            if (AnimationMode.InAnimationMode())
            {
                AnimationMode.StopAnimationMode();
            }
        }
    }

    private void StartPreview(AnimationEventStateBehaviour stateBehaviour, SerializedProperty isPreviewingProp)
    {
        // Stop preview on other behaviours
        StopOtherPreviews(stateBehaviour);

        stateBehaviour.isPreviewing = true;
        isPreviewingProp.boolValue = true;
        currentlyPreviewingBehaviour = stateBehaviour;

        // Ensure Animation Mode is active
        if (!AnimationMode.InAnimationMode())
        {
            AnimationMode.StartAnimationMode();
        }

        // Clean up any existing clones
        CleanupClones();
    }

    private void StopPreview(AnimationEventStateBehaviour stateBehaviour, SerializedProperty isPreviewingProp)
    {
        stateBehaviour.isPreviewing = false;
        isPreviewingProp.boolValue = false;

        if (currentlyPreviewingBehaviour == stateBehaviour)
        {
            currentlyPreviewingBehaviour = null;

            // Stop Animation Mode if no other behaviours are previewing
            if (!AnyBehaviourPreviewing())
            {
                if (AnimationMode.InAnimationMode())
                {
                    AnimationMode.StopAnimationMode();
                }
            }
        }

        // Cleanup clones
        CleanupClones();

        // Enforce T-pose after stopping preview
        EnforceTPose();
    }

    private void StopOtherPreviews(AnimationEventStateBehaviour currentBehaviour)
    {
        // Find all AnimationEventStateBehaviour instances
        var allBehaviours = Resources.FindObjectsOfTypeAll<AnimationEventStateBehaviour>();
        foreach (var behaviour in allBehaviours)
        {
            if (behaviour != currentBehaviour && behaviour.isPreviewing)
            {
                // Stop the preview
                behaviour.isPreviewing = false;

                // If this behaviour is the currently previewing one, update the static reference
                if (currentlyPreviewingBehaviour == behaviour)
                {
                    currentlyPreviewingBehaviour = null;
                }
            }
        }

        // Cleanup clones
        CleanupClones();
    }

    private bool AnyBehaviourPreviewing()
    {
        var allBehaviours = Resources.FindObjectsOfTypeAll<AnimationEventStateBehaviour>();
        foreach (var behaviour in allBehaviours)
        {
            if (behaviour.isPreviewing)
            {
                return true;
            }
        }
        return false;
    }

    bool Validate(AnimationEventStateBehaviour stateBehaviour, out string errorMessage)
    {
        AnimatorController animatorController = GetValidAnimatorController(out errorMessage);
        if (animatorController == null) return false;

        animator = Selection.activeGameObject.GetComponent<Animator>();

        ChildAnimatorState matchingState = animatorController.layers
            .SelectMany(l => l.stateMachine.states)
            .FirstOrDefault(s => s.state.behaviours.Contains(stateBehaviour));

        Motion motion = matchingState.state?.motion;

        if (motion == null)
        {
            errorMessage = "No motion assigned to the current state.";
            return false;
        }

        extractedClips.Clear();

        if (motion is AnimationClip clip)
        {
            extractedClips.Add(clip);
            clipNames = new string[] { clip.name };
        }
        else if (motion is BlendTree blendTree)
        {
            // Extract clips from the BlendTree
            GetAnimationClipsFromBlendTree(blendTree, extractedClips);
            if (extractedClips.Count > 0)
            {
                clipNames = extractedClips.Select(c => c.name).ToArray();
            }
            else
            {
                errorMessage = "No AnimationClips found in the BlendTree.";
                return false;
            }
        }
        else
        {
            errorMessage = "Unsupported motion type in the current state.";
            return false;
        }
        return true;
    }

    void GetAnimationClipsFromBlendTree(BlendTree blendTree, List<AnimationClip> clips)
    {
        foreach (var child in blendTree.children)
        {
            if (child.motion is AnimationClip clip)
            {
                if (!clips.Contains(clip))
                    clips.Add(clip);
            }
            else if (child.motion is BlendTree childBlendTree)
            {
                GetAnimationClipsFromBlendTree(childBlendTree, clips);
            }
        }
    }

    AnimatorController GetValidAnimatorController(out string errorMessage)
    {
        errorMessage = string.Empty;
        GameObject targetGameObject = Selection.activeGameObject;
        if (targetGameObject == null)
        {
            errorMessage = "Please select a GameObject with an Animator to Preview";
            return null;
        }
        Animator animator = targetGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            errorMessage = "The selected GameObject does not have an Animator component.";
            return null;
        }

        AnimatorController animController = animator.runtimeAnimatorController as AnimatorController;
        if (animController == null)
        {
            errorMessage = "The selected Animator does not have a valid AnimatorController.";
            return null;
        }

        return animController;
    }

    // Clone Formation Methods

    void PositionClones(List<GameObject> clones, CloneFormation formation, float spacing)
    {
        switch (formation)
        {
            case CloneFormation.Circle:
                PositionClonesInCircle(clones, spacing);
                break;
            case CloneFormation.Square:
                PositionClonesInSquare(clones, spacing);
                break;
            case CloneFormation.Line:
                PositionClonesInLine(clones, spacing);
                break;
            case CloneFormation.Triangle:
                PositionClonesInTriangle(clones, spacing);
                break;
            default:
                break;
        }
    }

    void PositionClonesInCircle(List<GameObject> clones, float spacing)
    {
        int numClones = clones.Count;
        float circumference = spacing * numClones;
        float radius = circumference / (2 * Mathf.PI);

        for (int i = 0; i < numClones; i++)
        {
            float angle = i * Mathf.PI * 2 / numClones;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            clones[i].transform.position = animator.transform.position + new Vector3(x, 0, z);
        }
    }

    void PositionClonesInLine(List<GameObject> clones, float spacing)
    {
        int numClones = clones.Count;
        float totalWidth = spacing * (numClones - 1);

        for (int i = 0; i < numClones; i++)
        {
            float x = (-totalWidth / 2f) + i * spacing;
            clones[i].transform.position = animator.transform.position + new Vector3(x, 0, 0);
        }
    }

    void PositionClonesInSquare(List<GameObject> clones, float spacing)
    {
        int numClones = clones.Count;
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(numClones));

        for (int i = 0; i < numClones; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;
            float x = (col - (gridSize - 1) / 2f) * spacing;
            float z = (row - (gridSize - 1) / 2f) * spacing;
            clones[i].transform.position = animator.transform.position + new Vector3(x, 0, z);
        }
    }

    void PositionClonesInTriangle(List<GameObject> clones, float spacing)
    {
        int numClones = clones.Count;
        int row = 0;
        int clonesPlaced = 0;

        while (clonesPlaced < numClones)
        {
            int clonesInRow = row + 1;

            for (int i = 0; i < clonesInRow && clonesPlaced < numClones; i++)
            {
                float x = (i - row / 2f) * spacing;
                float z = -row * spacing;
                clones[clonesPlaced].transform.position = animator.transform.position + new Vector3(x, 0, z);
                clonesPlaced++;
            }
            row++;
        }
    }
}
