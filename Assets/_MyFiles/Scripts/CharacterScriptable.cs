using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptable", menuName = "ScriptableObjects/Character")]
public class CharacterScriptable : ScriptableObject
{
    [Header("Character Info")]
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private GameObject characterPrefab;

    public string GetCharacterName() 
    {
        return characterName;
    }
    public Sprite GetCharacterSprite() 
    {
        return characterSprite;
    }
    public GameObject GetCharacterPrefab() 
    {
        return characterPrefab;
    }
}
