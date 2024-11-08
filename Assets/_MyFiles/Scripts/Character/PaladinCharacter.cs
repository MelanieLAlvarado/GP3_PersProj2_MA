using UnityEngine;

public class PaladinCharacter : CharacterBase
{
    private bool bComboMove = false;
    private void Update()
    {
        if (!bComboMove)
        {
            return;
        }
        GetComponent<CharacterController>().Move(transform.forward * Time.deltaTime);
    }

    public void StartComboMove() 
    {
        bComboMove = true;
    }
    public void EndComboMove()
    {
        bComboMove = false;
    }
}
