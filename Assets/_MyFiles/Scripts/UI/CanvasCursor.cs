using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasCursor : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private Image _dragIcon;

    private void Start()
    {
        InputSystem.onAfterUpdate += MoveCursor;
        SetIconVisibility(false);
    }

    public void SetDragIcon(Sprite iconToSet)
    { 
        _dragIcon.sprite = iconToSet;
    }
    public void SetIconVisibility(bool stateToSet) 
    {
        _dragIcon.gameObject.SetActive(stateToSet);
    }
    private void MoveCursor() 
    {
        if (_dragIcon)
        {
            _dragIcon.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDragIcon(null);
        SetIconVisibility(false);
    }
}
