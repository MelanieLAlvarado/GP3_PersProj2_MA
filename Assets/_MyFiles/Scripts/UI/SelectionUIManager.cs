using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    private void Awake()
    {
        if (playerSelectionGroup == null || playerSelectionUIPrefab == null)
        { return; }

        PlayerSelectionWidget playerSelectUI = Instantiate(playerSelectionUIPrefab).GetComponent<PlayerSelectionWidget>();
        playerSelectUI.transform.SetParent(playerSelectionGroup);
        _playerSelections.Add(playerSelectUI);

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
}
