using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LayoutGroupWidget : MonoBehaviour
{
    [Header("LayoutGroup Info")]
    [SerializeField] private GameObject widgetPrefab;
    List<Widget> _layoutWidgets = new List<Widget>();
    private List<CharacterScriptable> _charactersInSlots = new List<CharacterScriptable>();

    public List<Widget> GetLayoutWidgets() { return _layoutWidgets; }
    public void InitializeWidgetsForGameobjects(List<GameObject> connectedObjects)
    {
        if (!widgetPrefab)
        { return; }
        foreach (GameObject connectedObj in connectedObjects)
        {
            Widget widget = SpawnWidget();
            widget.SetOwner(connectedObj);
            InitializeWidget(connectedObj, widget);
        }
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
        /*//GAMEPLAY UI MANAGER
        Player player = connectedObj.GetComponent<Player>();
        CharacterScriptable character = connectedObj.GetComponent<>().GetCharacter();

        
        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();
        gameSlotUI.SetCharacterInSlot(character);

        gameSlotUI.SetPlayerNameText(player.GetPlayerName());

         GameObject currentChar = player.GetCurrentFightingCharacter();
        if (!currentChar.GetComponent<HealthComponent>())
        {
            return;
        }
        currentChar.GetComponent<HealthComponent>().OnHealthChanged += gameSlotUI.UpdateHealthText;
         */
    }

}
