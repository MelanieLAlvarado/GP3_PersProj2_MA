using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterSlot : MonoBehaviour
{
    [Header("CharacterSlot Info [READ ONLY]")]
    [SerializeField] private CharacterScriptable characterProfile;

    [SerializeField] private Image characterIcon;
    [SerializeField] private TextMeshProUGUI characterText;
    //[SerializeField] private Button selectButton;//May be replaced later

    public void SetCharacterInSlot(CharacterScriptable characterToSet)
    {
        characterProfile = characterToSet;
        characterIcon.sprite = characterProfile.GetCharacterSprite();
        characterText.text = characterProfile.GetCharacterName();
        /*if (selectButton)
        { 
            selectButton.interactable = true;
        } */   
    }

}
