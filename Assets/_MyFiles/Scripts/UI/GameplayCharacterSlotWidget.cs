using TMPro;
using UnityEngine;

public class GameplayCharacterSlotWidget : CharacterSlot
{
    [Header("Gameplay Attributes")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI healthText;//might change to damage later on

    public void SetPlayerNameText(string name)
    { 
        playerNameText.text = name;
    }
    public void UpdateHealthText(float healthToSet)
    {
        healthToSet *= 10;
        healthToSet = Mathf.Round(healthToSet);
        healthToSet /= 10;
        healthText.text = healthToSet.ToString();
    }
}
