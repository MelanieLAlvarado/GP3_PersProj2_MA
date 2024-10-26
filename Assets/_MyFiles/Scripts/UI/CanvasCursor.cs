using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasCursor : MonoBehaviour
{
    public delegate void OnCursorDragDragDelagate();
    public OnCursorDragDragDelagate OnCursorDragEnded;

    [SerializeField] private Image _dragIcon;

    private void Start()
    {
        InputSystem.onAfterUpdate += MoveCursor;
        OnCursorDragEnded += CursorDragEnded;
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
    private void CursorDragEnded()
    {
        SetDragIcon(null);
        SetIconVisibility(false);
    }
}
