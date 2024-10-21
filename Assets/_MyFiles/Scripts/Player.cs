using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [Header("Player Info")]
    private string _playerName;
    private int _playerLifes = 3;
    private float _playerHealth = 100; //may change to damage later and/or move into separate component

    [Header("Character Info")]
    [SerializeField] private CharacterScriptable _characterScriptable;
    private GameObject _currentCharacter;

    public void SetPlayerName(string nameToSet) { _playerName = nameToSet; }
    public string GetPlayerName() { return _playerName; }//<-- dont think is needed.

    public int GetPlayerLifes() { return _playerLifes; }
    public void RemoveLife() { _playerLifes--; }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GameManager.m_Instance.AddPlayer(this.gameObject);
        GameManager.m_Instance.GetSelectUIManager().SpawnPlayerSelectionUI(this.gameObject);

        GameObject playerHolder = GameManager.m_Instance.GetPlayerHolder();
        transform.SetParent(playerHolder.transform);
    }
    public GameObject GetCurrentFightingCharacter() { return _currentCharacter; }
    public CharacterScriptable GetCharacter() { return _characterScriptable; }
    public void SetCharacter(CharacterScriptable characterToSelect) 
    {
        _characterScriptable = characterToSelect;
    }
    public void ClearCharacter()
    {
        _characterScriptable = null;
    }
    public void SpawnCharacter(Transform spawnPosition) 
    {
        Debug.Log($"Spawning Character... for {_playerName}");
        _currentCharacter = Instantiate(_characterScriptable.GetCharacterPrefab(), spawnPosition.position, spawnPosition.rotation);
        CharacterBase charBase = _currentCharacter.GetComponent<CharacterBase>();
        charBase.SetOwner(gameObject);

        PlayerController playerCtrl = GetComponent<PlayerController>();
        playerCtrl.SetCharacter(charBase);
    }
}
