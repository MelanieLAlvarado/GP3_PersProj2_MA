using System.Collections.Generic;
using UnityEngine;

public class SelectionUIManager : MonoBehaviour
{
    [Header("Selection Info")]
    [SerializeField] private Transform selectionGroup;
    [SerializeField] private GameObject characterSlotPrefab;
    private List<CharacterSlot> _characterSlots = new List<CharacterSlot>();
    private List<CharacterScriptable> _charactersInSlots = new List<CharacterScriptable>();

    [SerializeField] private CharacterScriptable[] Characters;

    private void Awake()
    {
        if (selectionGroup == null)
        { return; }

        foreach (CharacterScriptable character in Characters)
        {
            if (_charactersInSlots.Contains(character))
            {
                continue;
            }
            _charactersInSlots.Add(character);

            CharacterSlot charSlot = Instantiate(characterSlotPrefab).GetComponent<CharacterSlot>();
            charSlot.transform.SetParent(selectionGroup);
            _characterSlots.Add(charSlot);
            charSlot.SetCharacterInSlot(character);

        }
    }
}
