using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public delegate void OnPlayerChangedDelegate(Player player);
    public event OnPlayerChangedDelegate OnPlayerRemoved;

    public delegate void OnLivesChangedDelgate(int lives);
    public event OnLivesChangedDelgate OnLivesChanged;

    public delegate void OnNoPlayerLivesDelegate();
    public event OnNoPlayerLivesDelegate OnNoPlayerLives;

    PlayerController _playerController;
    [Header("Player Info")]
    private string _playerName = "";
    [SerializeField] private int playerLifes = 3;
    private int _currentLifes = 3;

    [Header("Character Info")]
    private CharacterScriptable _characterScriptable;
    private GameObject _currentCharacter;

    public void SetPlayerName(string nameToSet) { _playerName = nameToSet; }
    public string GetPlayerName() { return _playerName; }

    public int GetPlayerLifes() { return _currentLifes; }
    public void ResetPlayerLifes()
    {
        _currentLifes = playerLifes;
    }
    public void RemoveLife() 
    {
        _currentLifes--;
        OnLivesChanged?.Invoke(_currentLifes);
        if (_currentLifes <= 0)
        {
            OnNoPlayerLives?.Invoke();
            OnNoPlayerLives -= GameManager.m_Instance.GetFightManager().CheckAllPlayersInBattle;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        ResetPlayerLifes();

        DataHolder.m_Instance.AddPlayer(this);
        _playerController = GetComponent<PlayerController>();
        SelectionUIManager selectUIManager = GameManager.m_Instance.GetSelectUIManager();
        if (selectUIManager)
        { 
            selectUIManager.SpawnPlayerSelectionWidget(this);
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
        if (!_characterScriptable)
        {
            Debug.LogError($"There is no CharacterScriptable on the player {this.gameObject.name}");
            return;
        }

        _currentCharacter = Instantiate(_characterScriptable.GetCharacterPrefab(), spawnPosition.position, spawnPosition.rotation);
        CharacterBase charBase = _currentCharacter.GetComponent<CharacterBase>();
        charBase.SetOwnerPlayer(this);

        _playerController.SetControlledCharacter(charBase);

        if (gameplayUI.CanAddGameplayWidget(this))
        {
            gameplayUI.InitializeWidgetForPlayer(this);
        }
        gameplayUI.AttachPlayerToWidget(this);
    }

    public void RemoveFromGame() 
    {
        OnPlayerRemoved?.Invoke(this);
        DataHolder.m_Instance.RemovePlayer(this);
        if (_currentCharacter)
        {
            Destroy(_currentCharacter);
        }
        if (_playerController) 
        {
            _playerController.DisablePlayerInputActions();
        }
        Destroy(gameObject);
    }
}
