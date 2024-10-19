using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [Header("Player Info")]
    private int _playerLifes = 3;
    private float _playerHealth = 100; //may change to damage later

    [Header("Character Info")]
    [SerializeField] private CharacterScriptable _characterScriptable;

    private GameObject currentCharacter;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        GameManager.m_Instance.AddPlayer(this.gameObject);
        GameManager.m_Instance.GetSelectUIManager().SpawnPlayerSelectionUI(this.gameObject);

        GameObject playerHolder = GameManager.m_Instance.GetPlayerHolder();
        transform.SetParent(playerHolder.transform);
    }
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
        Debug.Log("Spawning Character...");
        currentCharacter = Instantiate(_characterScriptable.GetCharacterPrefab(), spawnPosition.position, spawnPosition.rotation);
        CharacterBase charBase = currentCharacter.GetComponent<CharacterBase>();
        charBase.SetOwner(gameObject);

        PlayerController playerCtrl = GetComponent<PlayerController>();
        playerCtrl.SetCharacter(charBase);
    }
}
