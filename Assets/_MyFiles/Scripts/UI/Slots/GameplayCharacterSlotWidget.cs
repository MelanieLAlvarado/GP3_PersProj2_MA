using TMPro;
using UnityEngine;

public class GameplayCharacterSlotWidget : CharacterSlot
{
    [Header("Gameplay Attributes")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI lifeText;

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
    public void UpdateLifeText(int livesToSet)
    { 
        lifeText.text = livesToSet.ToString();
        if (livesToSet <= 0)
        {
            healthText.text = "";
            lifeText.text = "DEAD";
        }
    }
}
