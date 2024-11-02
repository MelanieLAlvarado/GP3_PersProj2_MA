using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LayoutGroupWidget : Widget
{
    [Header("LayoutGroup Info")]
    [SerializeField] private GameObject widgetPrefab;
    List<Widget> _layoutWidgets = new List<Widget>();
    protected Dictionary<Player, Widget> _widgetDictionary = new Dictionary<Player, Widget>();
    private List<CharacterScriptable> _charactersInSlots = new List<CharacterScriptable>();

    public List<Widget> GetLayoutWidgets() { return _layoutWidgets; }
    /*public void InitializeWidgetsForGameobjects(List<GameObject> connectedObjects)
    {
        if (!widgetPrefab)
        { return; }
        foreach (GameObject connectedObj in connectedObjects)
        {
            InitializeWidgetForSingleGameObj(connectedObj);
        }
    }*/
    public void InitializeWidgetForPlayer(Player player) 
    {
        Widget widget = SpawnWidget();
        widget.SetOwner(player.gameObject);

        _widgetDictionary.Add(player, widget);
        player.OnPlayerRemoved += DisconnectPlayerFromWidget;

        //Debug.Log($"Obj = {player.name}; widget = {widget.name}");
        InitializeWidget(player.gameObject, widget);
    }

    public void InitializeWidgetsForCharacters(CharacterScriptable[] characters)
    {
        if (!widgetPrefab)
        { return; }
        foreach (CharacterScriptable character in characters)
        {
            if (_charactersInSlots.Contains(character))
            {
                continue;
            }
            _charactersInSlots.Add(character);

            SelectCharacterSlotWidget charSlot = SpawnWidget().GetComponent<SelectCharacterSlotWidget>();
            SetOwner(gameObject);
            charSlot.SetCharacterInSlot(character);
        }
    }
    public Widget SpawnWidget() 
    {
        Widget widget = Instantiate(widgetPrefab, this.transform).GetComponent<Widget>();
        _layoutWidgets.Add(widget);
        return widget;
    }
    public virtual void InitializeWidget(GameObject connectedObj, Widget widget) 
    {
        //overridden in child class if all widgets need something additional
    }
    public void DisconnectPlayerFromWidget(Player player)
    {
        Debug.Log("Removing Player from widget");
        Widget widget = _widgetDictionary[player];
        if (!widget)
        { return; }
        _layoutWidgets.Remove(widget);
        _widgetDictionary.Remove(player);

        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();
        GameObject currentChar = player.GetCurrentFightingCharacter();
        if (gameSlotUI != null && currentChar != null)
        { 
            currentChar.GetComponent<HealthComponent>().OnHealthChanged -= gameSlotUI.UpdateHealthText;
        }

        /*PlayerSelectionWidget playerSelectUI = widget.GetComponent<PlayerSelectionWidget>();
        if (playerSelectUI)
        { 
            playerSelectUI.ClearCharacterInSlot();
            playerSelectUI.OnPlayerSelectionChanged -= GameManager.m_Instance.GetSelectUIManager().UpdateFightButton;
        }

        SelectionUIManager selectionUIManager = GetOwner().GetComponent<SelectionUIManager>();
        if (selectionUIManager) ///ensures that fight button is updated player is removed.
        {
            selectionUIManager.UpdateFightButton();
        }*/

        player.OnPlayerRemoved -= DisconnectPlayerFromWidget;
        Destroy(widget.gameObject);
    }
}
