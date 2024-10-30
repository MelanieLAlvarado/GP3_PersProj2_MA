using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSelectionWidget : CharacterSlot, IPointerDownHandler, IDropHandler
{
    public delegate void OnPlayerSelectionChangedDelegate();
    public event OnPlayerSelectionChangedDelegate OnPlayerSelectionChanged;

    [Header("Player Selection Options")]
    private string _playerName;

    private void Awake()
    {
        OnPlayerSelectionChanged += GameManager.m_Instance.GetSelectUIManager().UpdateFightButton;
    }
    private void Start()
    {
        OnPlayerSelectionChanged?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == null) 
        {
            return;
        }

        if (GetCharacterProfile() == null) 
        {
            return;
        }
        ClearCharacterInSlot();
        GetOwner().GetComponent<Player>().ClearCharacter();
        OnPlayerSelectionChanged?.Invoke();
    }
    public void OnDrop(PointerEventData eventData)
    {
        SelectCharacterSlotWidget charSelectSlot = eventData.pointerDrag.GetComponent<SelectCharacterSlotWidget>();
        if (charSelectSlot && charSelectSlot.GetCharacterProfile())
        {
            CharacterScriptable charScriptObj = charSelectSlot.GetCharacterProfile();
            SetCharacterInSlot(charScriptObj);
            GetOwner().GetComponent<Player>().SetCharacter(charScriptObj);
            OnPlayerSelectionChanged?.Invoke();
        }

        SelectionUIManager selectManager = GetOwner().GetComponent<SelectionUIManager>();
        if (selectManager)
        {
            selectManager.GetDragCursor().SetDragIcon(null);
            selectManager.GetDragCursor().SetIconVisibility(false);
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
