using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    private Camera _mainCamera;

    [Header("Managers")]//shouldn't show in inspector
    private SceneLoader _sceneLoader;

    /*Additional MANAGERS*/
    private SelectionUIManager _selectionUIManager;
    private FightManager _fightManager;
    private GameplayUIManager _gameplayUIManager;


    [Header("Player Info")]
    [SerializeField] DataHolder dataHolderPrefab;
    DataHolder _dataHolder;
    private string _dataHolderName = "DataHolder";
    private bool _keyboardSoloPlayer; 

    [SerializeField] private string playerHolderName = "PlayerHolder";
    GameObject _playerHolder;
    [SerializeField] private Player playerPrefab;

    public GameObject GetPlayerHolder() { return _playerHolder; }
    public Camera GetMainCamera() { return _mainCamera; }
    public SceneLoader GetSceneLoader() { return _sceneLoader; }
    public SelectionUIManager GetSelectUIManager() { return _selectionUIManager; }
    public FightManager GetFightManager() { return _fightManager; }
    public GameplayUIManager GetGameplayUIManager() { return _gameplayUIManager; }
    private void Awake()
    {
        GameObject dataHolderObject = GameObject.FindGameObjectWithTag(_dataHolderName);

        if (dataHolderObject)
        {
            _dataHolder = dataHolderObject.GetComponent<DataHolder>();
        } 
        else if (dataHolderPrefab)
        { 
            _dataHolder = Instantiate(dataHolderPrefab);
        }

        _keyboardSoloPlayer = _dataHolder.GetKeyboardSoloPlayer();
        
        if (m_Instance != null && m_Instance != this)
        {
            Debug.LogError("Multiple GameManagers found. Deleting Copy...");
            Destroy(this);
        }
        else
        {
            m_Instance = this;
        }
        GatherManagers();
        CheckPresentPlayers();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void GatherManagers() 
    {
        _sceneLoader = GetComponent<SceneLoader>();
        _selectionUIManager = GetComponent<SelectionUIManager>();
        _fightManager = GetComponent<FightManager>();
        _gameplayUIManager = GetComponent<GameplayUIManager>();
    }
    private void CheckPresentPlayers() 
    {
        _playerHolder = GameObject.Find(playerHolderName);
        if (!_playerHolder)
        {
            _playerHolder = new GameObject(playerHolderName);
            DontDestroyOnLoad(_playerHolder);
        }
        
        if (_dataHolder.GetPlayers().Count <= 0 && playerPrefab != null) ///if there are no players in the scene (players have DontDestroyOnLoad)
        {
            Player player1 = Instantiate(playerPrefab);
            player1.transform.SetParent(_playerHolder.transform);
        }
    }
    public void ProcessKeyboardPlayers(PlayerInput triggeredPlayerInput, string leftScheme, string rightScheme)
    {
        if (!_sceneLoader.IsSelectionScreenScene()) { return; } 

        if (_keyboardSoloPlayer)
        {
            Player player = Instantiate(playerPrefab);
            player.transform.SetParent(_playerHolder.transform);
            PlayerInput spawnPlayerInput = player.GetComponent<PlayerInput>();


            InputUser.PerformPairingWithDevice(Keyboard.current, triggeredPlayerInput.user);
            triggeredPlayerInput.user.ActivateControlScheme(leftScheme);

            InputUser.PerformPairingWithDevice(Keyboard.current, spawnPlayerInput.user);
            spawnPlayerInput.user.ActivateControlScheme(rightScheme);
            _keyboardSoloPlayer = !_keyboardSoloPlayer;
        }
        else if (triggeredPlayerInput.currentControlScheme == rightScheme)
        {
            Player player = triggeredPlayerInput.GetComponent<Player>();
            player.RemoveFromGame();
            _keyboardSoloPlayer = !_keyboardSoloPlayer;
        }
        _dataHolder.SetKeyboardSoloPlayer(_keyboardSoloPlayer);
    }
}
