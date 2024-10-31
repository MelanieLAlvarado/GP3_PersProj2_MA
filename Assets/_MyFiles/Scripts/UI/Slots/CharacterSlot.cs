using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterSlot : Widget
{
    [Header("CharacterSlot Info [READ ONLY]")]
    [SerializeField] private CharacterScriptable characterProfile;

    [SerializeField] private Image characterIcon;
    [SerializeField] private TextMeshProUGUI characterText;

    public CharacterScriptable GetCharacterProfile() { return characterProfile; }
    public void SetCharacterInSlot(CharacterScriptable characterToSet)
    {
        characterProfile = characterToSet;
        characterIcon.sprite = characterProfile.GetCharacterSprite();
        characterText.text = characterProfile.GetCharacterName(); 
    }
    public void ClearCharacterInSlot() 
    {
        characterIcon.sprite = null;
        characterText.text = "";
        characterProfile = null;
    }
}
