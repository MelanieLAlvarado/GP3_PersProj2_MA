using UnityEngine;

public class PaladinCharacter : CharacterBase
{
    public void StartComboMove() 
    {
        bAdditionalMovement = true;
    }
    public void EndComboMove()
    {
        bAdditionalMovement = false;
    }
}
