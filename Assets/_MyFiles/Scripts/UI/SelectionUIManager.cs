using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionUIManager : MonoBehaviour
{
    [Header("UI Info")]
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject selectionLayoutUIPrefab;
    private LayoutGroupWidget _selectionUI;
    [SerializeField] private GameObject playerSelectionLayoutUIPrefab;
    private LayoutGroupWidget _playerSelectionUI;
    [SerializeField] private GameObject fightButton;

    [Header("Character Selections Info")]
    [SerializeField] private CharacterScriptable[] Characters;

    [Header("Player Selection Info")]//should not be shown in inspector
    private List<PlayerSelectionWidget> _playerSelections = new List<PlayerSelectionWidget>();
    public void SpawnPlayerSelectionUI(GameObject player) 
    {
        if (!playerSelectionLayoutUIPrefab) { return; }

        if (!_playerSelectionUI)
        {
            _playerSelectionUI = Instantiate(playerSelectionLayoutUIPrefab, canvasTransform).GetComponent<LayoutGroupWidget>();
        }
        Widget widget = _playerSelectionUI.SpawnWidget();
        widget.SetOwner(player);

        PlayerSelectionWidget playerSelection = widget.GetComponent<PlayerSelectionWidget>();
        if (playerSelection)
        { 
            _playerSelections.Add(playerSelection);
        }
    }
    private void Awake()
    {
        if (!selectionLayoutUIPrefab) { return; }

        _selectionUI = Instantiate(selectionLayoutUIPrefab, canvasTransform).GetComponent<LayoutGroupWidget>();
        _selectionUI.GetComponent<LayoutGroupWidget>().InitializeWidgetsForCharacters(Characters);
        //GameManager.m_Instance.OnPlayerCountChanged += UpdateFightButton;
    }
    private void Update() //change to delegates?
    {
        //update player here instead of start (or through delegate events)
        //check if players all have their pick, then reveal fight button.
        UpdateFightButton();
    }
    private void UpdateFightButton() 
    {
        if (_playerSelectionUI.GetLayoutWidgets().Count < 0) { return; }

        foreach (Widget widget in _playerSelectionUI.GetLayoutWidgets())
        {
            PlayerSelectionWidget playerSelection = widget.GetComponent<PlayerSelectionWidget>();
            if (playerSelection && playerSelection.GetCharacterProfile() == null)
            {
                fightButton.SetActive(false);
                return;
            }
        }
        fightButton.SetActive(true);
    }
}
