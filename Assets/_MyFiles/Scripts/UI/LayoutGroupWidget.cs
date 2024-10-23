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
    /*public void InitializeWidgetsForGameobjects(List<GameObject> connectedObjects)
    {
        if (!widgetPrefab)
        { return; }
        foreach (GameObject connectedObj in connectedObjects)
        {
            InitializeWidgetForSingleGameObj(connectedObj);
        }
    }*/
    public void InitializeWidgetForSingleGameObj(GameObject connectedObj) 
    {
        Widget widget = SpawnWidget();
        widget.SetOwner(connectedObj);
        Debug.Log($"Obj = {connectedObj.name}; widget = {widget.name}");
        InitializeWidget(connectedObj, widget);
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
        //overridden in child class if all widgets need something additional
    }

}
