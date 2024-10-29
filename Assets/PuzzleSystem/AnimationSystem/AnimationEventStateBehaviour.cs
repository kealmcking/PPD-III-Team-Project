using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

[Serializable]
public class AnimationEventInfo
{
    public string eventName;
    [HideInInspector]
    public float triggerTime;
    public int clipsMask = -1; // Default to all clips

    // New fields for per-event settings
    public CloneFormation cloneFormation = CloneFormation.Circle;
    public float cloneSpacing = 1f;
    public bool useBatchSlider = true;
    public float batchTriggerTime = 0f;
    public List<float> individualTriggerTimes = new List<float>();

    [NonSerialized]
    public bool hasTriggered;

    [NonSerialized]
    public List<AnimationClip> selectedClips = new List<AnimationClip>();

    [NonSerialized]
    public float lastTriggeredTime = -1f; // Track last time event was triggered
}

public enum CloneFormation
{
    Circle,
    Square,
    Line,
    Triangle
}

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    public List<AnimationEventInfo> events = new List<AnimationEventInfo>();

    [HideInInspector]
    public bool isPreviewing = false;

    [HideInInspector]
    public bool foldout = true;

    [NonSerialized]
    private float previousTime = 0f;

    [NonSerialized]
    private List<AnimationClip> stateClips = new List<AnimationClip>();

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset hasTriggered and lastTriggeredTime for all events
        foreach (var evt in events)
        {
            evt.hasTriggered = false;
            evt.lastTriggeredTime = -1f;
        }

        // Initialize previousTime
        previousTime = 0f;

        // Clear the stateClips list
        stateClips.Clear();

        // Get the clips from the current state
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        AnimatorState currentState = GetCurrentState(animatorController, layerIndex, stateInfo.shortNameHash);

        if (currentState != null)
        {
            ExtractClipsFromMotion(currentState.motion, stateClips);

            // For each event, resolve the selected clips based on the clipsMask
            foreach (var evt in events)
            {
                evt.selectedClips.Clear();
                for (int i = 0; i < stateClips.Count; i++)
                {
                    if ((evt.clipsMask & (1 << i)) != 0)
                    {
                        evt.selectedClips.Add(stateClips[i]);
                    }
                }
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = stateInfo.normalizedTime % 1f;

        // Detect if the animation has looped
        if (currentTime < previousTime)
        {
            // Animation has looped, reset hasTriggered and lastTriggeredTime for all events
            foreach (var evt in events)
            {
                evt.hasTriggered = false;
                evt.lastTriggeredTime = -1f;
            }
        }

        // Get the current playing clips
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);

        // For each event, check if the current clip matches and trigger if conditions are met
        foreach (var evt in events)
        {
            // Check if event has already been triggered
            if (evt.hasTriggered)
                continue;

            // Check if any of the current clips match the selected clips
            foreach (var clipInfo in clipInfos)
            {
                if (evt.selectedClips.Contains(clipInfo.clip))
                {
                    // Only consider clips with significant weight
                    if (clipInfo.weight >= 0.5f)
                    {
                        // Ensure the event is only triggered once per loop
                        if (currentTime >= evt.triggerTime && evt.lastTriggeredTime < evt.triggerTime)
                        {
                            // Trigger the event
                            NotifyReceiver(animator, evt.eventName);
                            evt.hasTriggered = true;
                            evt.lastTriggeredTime = currentTime;
                            break;
                        }
                    }
                }
            }
        }

        // Update previousTime
        previousTime = currentTime;
    }

    private void NotifyReceiver(Animator animator, string eventName)
    {
        if (animator.TryGetComponent(out AnimationEventReceiver receiver))
        {
            receiver.OnAnimationEventTriggered(eventName);
        }
    }

    private AnimatorState GetCurrentState(AnimatorController animatorController, int layerIndex, int stateNameHash)
    {
        AnimatorStateMachine stateMachine = animatorController.layers[layerIndex].stateMachine;

        foreach (ChildAnimatorState childState in stateMachine.states)
        {
            if (childState.state.nameHash == stateNameHash)
            {
                return childState.state;
            }
        }

        // If not found in the base state machine, search sub-state machines
        foreach (ChildAnimatorStateMachine childStateMachine in stateMachine.stateMachines)
        {
            AnimatorState state = GetStateFromStateMachine(childStateMachine.stateMachine, stateNameHash);
            if (state != null)
                return state;
        }

        return null;
    }

    private AnimatorState GetStateFromStateMachine(AnimatorStateMachine stateMachine, int stateNameHash)
    {
        foreach (ChildAnimatorState childState in stateMachine.states)
        {
            if (childState.state.nameHash == stateNameHash)
            {
                return childState.state;
            }
        }

        foreach (ChildAnimatorStateMachine childStateMachine in stateMachine.stateMachines)
        {
            AnimatorState state = GetStateFromStateMachine(childStateMachine.stateMachine, stateNameHash);
            if (state != null)
                return state;
        }

        return null;
    }

    private void ExtractClipsFromMotion(Motion motion, List<AnimationClip> clips)
    {
        if (motion is AnimationClip clip)
        {
            if (!clips.Contains(clip))
                clips.Add(clip);
        }
        else if (motion is BlendTree blendTree)
        {
            foreach (var child in blendTree.children)
            {
                ExtractClipsFromMotion(child.motion, clips);
            }
        }
    }
}
