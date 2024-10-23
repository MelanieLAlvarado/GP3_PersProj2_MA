using UnityEngine;
using UnityEngine.InputSystem;
using static FightManager;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public delegate void OnPlayerChangedDelegate(Player player);
    public event OnPlayerChangedDelegate OnPlayerDead;

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
        SelectionUIManager selectUIManager = GameManager.m_Instance.GetSelectUIManager();
        if (selectUIManager)
        { 
            selectUIManager.SpawnPlayerSelectionUI(this.gameObject);
        }
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
    public void SpawnCharacter(Transform spawnPosition, GameplayUIManager gameplayUI)
    {
        Debug.Log($"Spawning Character... for {_playerName}");
        Debug.Log($"Widget spawning on {gameplayUI.name}");
        if (!_characterScriptable)
        {
            Debug.LogError($"There is no CharacterScriptable on the player {this.gameObject.name}");
            return;
        }

        _currentCharacter = Instantiate(_characterScriptable.GetCharacterPrefab(), spawnPosition.position, spawnPosition.rotation);
        CharacterBase charBase = _currentCharacter.GetComponent<CharacterBase>();
        charBase.SetOwner(gameObject);

        PlayerController playerCtrl = GetComponent<PlayerController>();
        playerCtrl.SetCharacter(charBase);

        if (gameplayUI.CanAddGameplayWidget(this))
        {
            gameplayUI.InitializeWidgetForSingleGameObj(this.gameObject);
        }
        gameplayUI.AttachPlayerToWidget(this);
    }
}
