using System.Collections.Generic;
using UnityEngine;

public class SelectionUIManager : MonoBehaviour
{
    [Header("Character Selection Info")]
    [SerializeField] private Transform characterSelectionGroup;
    [SerializeField] private GameObject selectionSlotPrefab;

    private List<SelectCharacterSlotWidget> _selectCharacterSlots = new List<SelectCharacterSlotWidget>();
    private List<CharacterScriptable> _charactersInSlots = new List<CharacterScriptable>();

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
    }
    private void Awake()
    {
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
    }
    private void Update() //change to delegates?
    {
        //update player here instead of start (or through delegate events)
        //check if players all have their pick, then reveal fight button.
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
