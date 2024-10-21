using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
    [SerializeField] GameObject toolTip;
    public TextMeshProUGUI textComp;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);   //make sure there is only 1 tooltip manager
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        toolTip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        toolTip.transform.position = UnityEngine.Input.mousePosition * new Vector2(1.3f, 1.3f);
    }

    public void setAndShow(string msg, string info)
    {
        toolTip.SetActive(true);
        textComp.text = msg + '\n' + info;
    }

    public void hide()
    {
        toolTip.SetActive(false);
    }
}
