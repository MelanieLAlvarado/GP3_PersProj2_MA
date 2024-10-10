using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [Header("CharacterSlot Info [READ ONLY]")]
    [SerializeField] private CharacterScriptable characterProfile;

    [SerializeField] private Image icon;
    [SerializeField] private Button selectButton;//May be replaced later

    public void SetCharacterInSlot(CharacterScriptable characterToSet)
    {
        characterProfile = characterToSet;
        icon.sprite = characterProfile.GetCharacterSprite();
        Debug.Log($"Character in slot is {characterProfile.GetCharacterName()}");
        if (selectButton)
        { 
            selectButton.interactable = true;
        }    
    }

}
