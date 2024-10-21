using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSelectionWidget : CharacterSlot, IPointerDownHandler, IDropHandler
{
    [Header("Player Selection Options")]
    private string _playerName;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetCharacterProfile() == null) 
        {
            return;
        }
        ClearCharacterInSlot();
        GetOwner().GetComponent<Player>().ClearCharacter();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        SelectCharacterSlotWidget charSelectSlot = eventData.pointerDrag.GetComponent<SelectCharacterSlotWidget>();
        if (charSelectSlot && charSelectSlot.GetCharacterProfile())
        {
            CharacterScriptable charScriptObj = charSelectSlot.GetCharacterProfile();
            SetCharacterInSlot(charScriptObj);
            GetOwner().GetComponent<Player>().SetCharacter(charScriptObj);
        }
    }
    public void ReadPlayerName(string name) 
    {
        _playerName = name;
        Player player = GetOwner().GetComponent<Player>();
        if (!player) { return; }
        if (_playerName != string.Empty)
        {
            player.SetPlayerName(_playerName);
            return;
        }
        player.SetPlayerName("player");//may update this later
    }

}
