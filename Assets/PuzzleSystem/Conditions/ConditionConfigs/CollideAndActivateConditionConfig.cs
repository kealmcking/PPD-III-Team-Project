using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
                if (conditionObject.ChildDenyMaterial != null)
                {                 
                            conditionObject.ChildDenyMaterial.SetColor("_Color", Color.red);
                            conditionObject.ChildDenyMaterial.SetFloat("_Scale", 1.02f);
                }
                conditionObject.DenyMaterial.SetColor("_Color", Color.red);
                conditionObject.DenyMaterial.SetFloat("_Scale", 1.02f);
                AudioSource source = conditionObject.GetComponent<AudioSource>();
                source.clip = conditionObject.DenyAudioClip;
                source.Play();
            }
            else
            {
                if(controller.objectInHand.GetObject().TryGetComponent(out Item item)){
                    if (item.Data.Name == triggerCheck.Name)
                    {
                        if (conditionObject.ChildDenyMaterial != null)
                        {
                            conditionObject.ChildDenyMaterial.SetColor("_Color", Color.green);
                            conditionObject.ChildDenyMaterial.SetFloat("_Scale", 1.02f);
                        }
                        conditionObject.DenyMaterial.SetColor("_Color", Color.green);
                        conditionObject.DenyMaterial.SetFloat("_Scale", 1.02f);
                        canBeInteractedWith = true;
                    }
                    else
                    {
                        if (conditionObject.ChildDenyMaterial != null)
                        {
                            conditionObject.ChildDenyMaterial.SetColor("_Color", Color.red);
                            conditionObject.ChildDenyMaterial.SetFloat("_Scale", 1.02f);
                        }
                        conditionObject.DenyMaterial.SetColor("_Color", Color.red);
                        conditionObject.DenyMaterial.SetFloat("_Scale", 1.02f);
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
        if (conditionObject.ChildDenyMaterial != null)
        {
            conditionObject.ChildDenyMaterial.SetFloat("_Scale", 0f);
        }
        conditionObject.DenyMaterial.SetFloat("_Scale", 0f);
    }
}
