using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class LayoutGroupWidget : Widget
{
    [Header("LayoutGroup Info")]
    [SerializeField] private GameObject widgetPrefab;
    List<Widget> _layoutWidgets = new List<Widget>();
    protected Dictionary<Player, Widget> _widgetDictionary = new Dictionary<Player, Widget>();
    private List<CharacterScriptable> _charactersInSlots = new List<CharacterScriptable>();

    public List<Widget> GetLayoutWidgets() { return _layoutWidgets; }
    public void InitializeWidgetForPlayer(Player player) 
    {
        Widget widget = SpawnWidget();
        widget.SetOwner(player.gameObject);

        _widgetDictionary.Add(player, widget);
        player.OnPlayerRemoved += DisconnectPlayerFromWidget;

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

        widget.gameObject.SetActive(false);

        player.OnPlayerRemoved -= DisconnectPlayerFromWidget;
        Debug.Log("Removed Player from widget");

        Destroy(widget.gameObject);
    }
}
