
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CheatSheetUI : MonoBehaviour
{
    [SerializeField] Image killerImg;
    [SerializeField] Image motiveImg;
    [SerializeField] Image roomImg;
    [SerializeField] Image weaponImg;
    private void OnEnable()
    {
        EventSheet.SendGameSelection += UpdateCanvasImages;
        EventSheet.SendKillerClue += UpdateKillerImage;
    }
    private void OnDisable()
    {
        EventSheet.SendGameSelection -= UpdateCanvasImages;
        EventSheet.SendKillerClue -= UpdateKillerImage;
    }
    public void UpdateCanvasImages(GameSelection selection)
    {
        TextMeshProUGUI text = motiveImg.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = selection.GetMotive().Name;
        motiveImg.sprite = selection.GetMotive().Icon;

        TextMeshProUGUI roomTxt = roomImg.GetComponentInChildren<TextMeshProUGUI>();
        if (roomTxt != null)
            roomTxt.text = selection.GetRoom().Name;
        roomImg.sprite = selection.GetRoom().Icon;

        TextMeshProUGUI weaponTxt = weaponImg.GetComponentInChildren<TextMeshProUGUI>();
        if (weaponTxt != null)
            weaponTxt.text = selection.GetWeapon().Name;
        weaponImg.sprite = selection.GetWeapon().Icon;
      
        
    }
    public void UpdateKillerImage(KillerClueData data)
    {
        TextMeshProUGUI text = killerImg.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = data.Name;
        killerImg.sprite = data.Icon;
    }
}
