using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetAsSelectedEventManagerObjectOnEnable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
