using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollideAndActivateConfig", menuName = "PuzzleSystem/ConditionConfig/CollideAndActivateConfig")]
public class CollideAndActivateConditionConfig : ConditionConfig
{
    [SerializeField] CraftableItemData triggerCheck;
    bool canBeInteractedWith = false;

    public override void EnterSetup(Condition conditionObject)
    {

    }
    public override void ConditionStatus(Condition conditionObject)
    {
        if (conditionObject != null&& canBeInteractedWith == true && conditionObject.isInteractedWith)
            ConfigConditionMet?.Invoke();
    }

    

    public override void TriggerEntered(Condition conditionObject, Collider other)
    {
        if(other.TryGetComponent(out playerController controller)){
            if (!TutorialUIManager.Instance.DisplayBlocked)
            {
                TutorialUIManager.Instance.DisplayBlockedArea();
            }
            if(controller.ObjectInHand == null)
            {
                if (conditionObject.ChildToUpdate != null)
                {
                    Material mat = conditionObject.ChildToUpdate.GetComponent<Renderer>().materials[1];
                    mat.SetColor("_Color", Color.red);
                    mat.SetFloat("_Scale", 1.01f);
                }
                conditionObject.DenyMaterial.SetColor("_Color", Color.red);
                conditionObject.DenyMaterial.SetFloat("_Scale", 1.01f);
                AudioSource source = conditionObject.GetComponent<AudioSource>();
                source.clip = conditionObject.DenyAudioClip;
                source.Play();
            }
            else
            {
                if(controller.objectInHand.GetObject().TryGetComponent(out Item item)){
                    if (item.Data.Name == triggerCheck.Name)
                    {
                        if (conditionObject.ChildToUpdate != null)
                        {
                            Material mat = conditionObject.ChildToUpdate.GetComponent<Renderer>().materials[1];
                            mat.SetColor("_Color", Color.green);
                            mat.SetFloat("_Scale", 1.01f);
                        }
                        conditionObject.DenyMaterial.SetColor("_Color", Color.green);
                        conditionObject.DenyMaterial.SetFloat("_Scale", 1.01f);
                        canBeInteractedWith = true;

                    }
                    else
                    {
                        if (conditionObject.ChildToUpdate != null)
                        {
                            Material mat = conditionObject.ChildToUpdate.GetComponent<Renderer>().materials[1];
                            mat.SetColor("_Color", Color.red);
                            mat.SetFloat("_Scale", 1.01f);
                        }
                        conditionObject.DenyMaterial.SetColor("_Color", Color.red);
                        conditionObject.DenyMaterial.SetFloat("_Scale", 1.01f);
                        AudioSource source = conditionObject.GetComponent<AudioSource>();
                        source.clip = conditionObject.DenyAudioClip;
                        source.Play();
                    }
                }
            }
        }
       
    }
    public override void TriggerExited(Condition conditionObject, Collider other)
    {
        other.TryGetComponent(out Item item);
        if (item != null)
        {
            if (item.Data.Name == triggerCheck.Name)
                canBeInteractedWith = false;
        }
        if (conditionObject.ChildToUpdate != null)
        {
            Material mat = conditionObject.ChildToUpdate.GetComponent<Renderer>().materials[1];
            mat.SetFloat("_Scale", 0f);
        }
        conditionObject.DenyMaterial.SetFloat("_Scale", 0f);
    }
}
