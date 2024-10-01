using UnityEngine;
//make editor script for Lore when make Lore is pressed a window will pop-up that asks which case/motive you want to add it to then press ok.
//This will create a prefab and add the prefab to the correct scriptable object for this puzzle
public class Lore : MonoBehaviour, IInspectable
{
    [SerializeField, Tooltip("Place a description explaining the lore here")] Description description;
    [SerializeField, Tooltip("Only place either a texture or a sprite for the icon, not both!")] protected Texture2D textureIcon;
    [SerializeField, Tooltip("Only place either a texture or a sprite for the icon, not both!")] protected Sprite spriteIcon;
    public void Inspect()
    {

    }
}