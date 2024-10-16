using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCharacterSlotWidget : CharacterSlot, IBeginDragHandler, IDragHandler //,IEndDragHandler
{

    public void OnBeginDrag(PointerEventData eventData)
    {
        CharacterScriptable charProfile = GetCharacterProfile();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
    }
    /*
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
    }*/
}