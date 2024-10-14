using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invItem : MonoBehaviour
{
    public itemScriptableObject itemScriptableObject;

    [SerializeField] Image iconImage;
    // Update is called once per frame
    void Update()
    {
        iconImage.sprite = itemScriptableObject.icon; 
    }
}
