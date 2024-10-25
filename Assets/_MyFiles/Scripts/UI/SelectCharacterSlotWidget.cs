using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCharacterSlotWidget : CharacterSlot, IBeginDragHandler, IDragHandler //,IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        //CharacterScriptable charProfile = GetCharacterProfile();
        if (eventData.pointerDrag == null)
        {
            return;
        }

        Sprite iconSprite = GetCharacterProfile().GetCharacterSprite();
        if (!iconSprite)
        {
            return;
        }

        Debug.Log(iconSprite.name);
        SelectionUIManager selectManager = GameManager.m_Instance.GetSelectUIManager();
        if (selectManager)
        {
            selectManager.GetDragCursor().SetDragIcon(iconSprite);
            selectManager.GetDragCursor().SetIconVisibility(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
    }
}