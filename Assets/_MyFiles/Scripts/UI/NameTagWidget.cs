using TMPro;
using UnityEngine;

public class NameTagWidget : Widget
{
    [SerializeField] TextMeshProUGUI playerNameText;

    public override void SetOwner(GameObject owner) 
    {
        base.SetOwner(owner);
        if (owner == null) { return; }

        HealthComponent ownerHealthComponent = owner.GetComponent<HealthComponent>();
        if (ownerHealthComponent != null) 
        {
            ownerHealthComponent.OnDead += OwnerDead;
        }

        CharacterBase charBase = owner.GetComponent<CharacterBase>();
        if (charBase)
        { 
            string playerName = charBase.GetOwnerPlayer().GetPlayerName();
            SetPlayerNameText(playerName);
        }
    }
    private void SetPlayerNameText(string playerName) 
    {
        if (playerName != null)
        { 
            playerNameText.text = playerName;
        }
    }
    private void OwnerDead() { Destroy(gameObject); }
}
