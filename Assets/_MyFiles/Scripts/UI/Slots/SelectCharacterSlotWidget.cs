using UnityEngine;
using UnityEngine.EventSystems;

public class SelectCharacterSlotWidget : CharacterSlot, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasCursor _dragCursor;
    private void Start()
    {
        SelectionUIManager selectManager = GameManager.m_Instance.GetSelectUIManager();
        if (selectManager)
        {
            _dragCursor = selectManager.GetDragCursor();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) { return; }

        Sprite iconSprite = GetCharacterProfile().GetCharacterSprite();
        if (!iconSprite)
        {
            return;
        }

        if (_dragCursor)
        {
            _dragCursor.SetDragIcon(iconSprite);
            _dragCursor.SetIconVisibility(true);
        }
    }
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || !_dragCursor) { return; }
        _dragCursor.OnCursorDragEnded?.Invoke();
    }
}
