using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionUIManager : MonoBehaviour
{
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject selectionLayoutUIPrefab;
    private LayoutGroupWidget _selectionUI;
    [SerializeField] private GameObject playerSelectionLayoutUIPrefab;
    private LayoutGroupWidget _playerSelectionUI;

    [Header("Character Selection Info")]

    [SerializeField] private Transform characterSelectionGroup;
    [SerializeField] private GameObject selectionSlotPrefab;

    private List<SelectCharacterSlotWidget> _selectCharacterSlots = new List<SelectCharacterSlotWidget>();

    private List<CharacterScriptable> _charactersInSlots = new List<CharacterScriptable>();//remove once proper ui is implemented
    

    [SerializeField] private CharacterScriptable[] Characters;

    [Header("Player Selection Info")]
    [SerializeField] private Transform playerSelectionGroup;
    [SerializeField] private GameObject playerSelectionUIPrefab;

    private List<PlayerSelectionWidget> _playerSelections = new List<PlayerSelectionWidget>();
    [SerializeField] private GameObject fightButton;
    public void SpawnPlayerSelectionUI(GameObject player) 
    {
        if (playerSelectionGroup == null || playerSelectionUIPrefab == null)
        { return; }

        PlayerSelectionWidget playerSelectUI = Instantiate(playerSelectionUIPrefab).GetComponent<PlayerSelectionWidget>();
        playerSelectUI.SetOwner(player);

        playerSelectUI.transform.SetParent(playerSelectionGroup);
        _playerSelections.Add(playerSelectUI);

        /*if (!playerSelectionUIPrefab) { return; }

        if (!_playerSelectionUI)
        {
            _playerSelectionUI = Instantiate(playerSelectionUIPrefab).GetComponent<LayoutGroupWidget>();
            _playerSelectionUI.transform.SetParent(canvasTransform); //save ref for canvas
        }
        /*Widget widget = _playerSelectionUI.SpawnWidget();
        widget.SetOwner(player);

        PlayerSelectionWidget playerSelection = widget.GetComponent<PlayerSelectionWidget>();
        if (playerSelection)
        { 
            _playerSelections.Add(playerSelection);
        }*/
    }
    private void Awake()
    {
        /*if (!selectionLayoutUIPrefab) { return; }

        _selectionUI = Instantiate(selectionLayoutUIPrefab).GetComponent<LayoutGroupWidget>();
        _selectionUI.transform.SetParent(canvasTransform); //save ref for canvas
        _selectionUI.GetComponent<LayoutGroupWidget>().InitializeWidgetsForCharacters(Characters);*/

        if (characterSelectionGroup == null || selectionSlotPrefab == null)
        { return; }
        foreach (CharacterScriptable character in Characters)
        {
            if (_charactersInSlots.Contains(character))
            {
                continue;
            }
            _charactersInSlots.Add(character);

            SelectCharacterSlotWidget charSlot = Instantiate(selectionSlotPrefab).GetComponent<SelectCharacterSlotWidget>();
            charSlot.transform.SetParent(characterSelectionGroup);
            charSlot.SetCharacterInSlot(character);
            _selectCharacterSlots.Add(charSlot);

        }
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
        if (_playerSelections.Count < 0) { return; }

        foreach (PlayerSelectionWidget playerSelection in _playerSelections)
        {
            if (playerSelection.GetCharacterProfile() == null)
            {
                fightButton.SetActive(false);
                return;
            }
        }
        fightButton.SetActive(true);
    }
}
