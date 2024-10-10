using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    private int _playerLifes = 3;
    private float _playerHealth = 100; //may change to damage later

    [Header("Character Info")]
    private CharacterScriptable _characterScriptable;

    private Transform _characterSpawnLocation;
    private GameObject currentCharacter;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void SetCharacter(CharacterScriptable characterToSelect) 
    {
        _characterScriptable = characterToSelect;
    }
    public void ClearCharacter()
    {
        _characterScriptable = null;
    }
    public void CharacterSpawn() 
    {
        currentCharacter = Instantiate(_characterScriptable.GetCharacterPrefab(), _characterSpawnLocation.position, _characterSpawnLocation.rotation);
    }
}
