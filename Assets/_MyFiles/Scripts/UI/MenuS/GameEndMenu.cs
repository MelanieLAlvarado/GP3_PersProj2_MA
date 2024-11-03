using TMPro;
using UnityEngine;

public class GameEndMenu : MenuScript
{
    [SerializeField] private TextMeshProUGUI playerWinText;
    [SerializeField] private CharacterSlot winnerCharacterSlot;
    public CharacterSlot GetWinnerSlot() { return winnerCharacterSlot; }
    public void SetCharacterWinText(string playerName) 
    {
        playerWinText.text = playerName + " wins!";
    }
}
