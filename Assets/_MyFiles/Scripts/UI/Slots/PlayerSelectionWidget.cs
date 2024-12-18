using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerSelectionWidget : CharacterSlot, IPointerDownHandler, IDropHandler
{
    public delegate void OnPlayerSelectionChangedDelegate();
    public event OnPlayerSelectionChangedDelegate OnPlayerSelectionChanged;

    [Header("Player Selection Options")]
    [SerializeField] TextMeshProUGUI placeHolderNameText;
    private string _playerName;

    private void Awake()
    {
        OnPlayerSelectionChanged += GameManager.m_Instance.GetSelectUIManager().UpdateFightButton;
    }
    private void Start()
    {
        OnPlayerSelectionChanged?.Invoke();

        Player player = GetOwner().GetComponent<Player>();
        if (player)
        {
            string nameOnPlayer = player.GetPlayerName();
            placeHolderNameText.text = nameOnPlayer;
            ReadPlayerName(nameOnPlayer);
        }
        else
        {
            ReadPlayerName(string.Empty);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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
        else 
        {
            return;
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
        if (!GetOwner()) { return; }

        Player player = GetOwner().GetComponent<Player>();
        if (!player) { return; }
        
        if (_playerName != string.Empty)
        {
            player.SetPlayerName(_playerName);
            return;
        }
        int playerIndex = DataHolder.m_Instance.GetPlayers().IndexOf(player);
        playerIndex += 1;
        player.SetPlayerName("Player " + playerIndex);
        placeHolderNameText.text = "Player " + playerIndex;
    }
    private void OnDestroy()
    {
        OnPlayerSelectionChanged?.Invoke();
    }
}
