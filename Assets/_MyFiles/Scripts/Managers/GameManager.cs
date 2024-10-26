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
    /*public delegate void OnPlayerCountChangedDelegate();
    public event OnPlayerCountChangedDelegate OnPlayerCountChanged;*/

    public static GameManager m_Instance;
    private Camera _mainCamera;

    [Header("Managers")]//shouldn't show in inspector
    private SceneLoader _sceneLoader;

    /*OPTIONAL MANAGERS*/
    private SelectionUIManager _selectionUIManager;
    private FightManager _fightManager;
    private GameplayUIManager _gameplayUIManager;


    [Header("Player Info")]
    DataHolder _dataHolder;
    private string _dataHolderName = "DataHolder";
    private bool _keyboardSoloPlayer; //need to debug this because gamemanager is currently destroying itself!!

    [SerializeField] private string playerHolderName = "PlayerHolder";
    GameObject _playerHolder;
    [SerializeField] private GameObject playerPrefab;
    private List<GameObject> _players = new List<GameObject>();
    public List<GameObject> GetPlayers() { return _players; }
    public void AddPlayer(GameObject player)
    {
        _players.Add(player);
        Debug.Log($"Player, {player.name}");
        //OnPlayerCountChanged?.Invoke();
    }
    public GameObject GetPlayerHolder() { return _playerHolder; }
    public Camera GetMainCamera() { return _mainCamera; }
    public SceneLoader GetSceneLoader() { return _sceneLoader; }
    public SelectionUIManager GetSelectUIManager() { return _selectionUIManager; }
    public FightManager GetFightManager() { return _fightManager; }
    public GameplayUIManager GetGameplayUIManager() { return _gameplayUIManager; }
    private void Awake()
    {
        SceneManager.sceneLoaded += ProcessLoadedScene;
        _dataHolder = GameObject.FindGameObjectWithTag(_dataHolderName).GetComponent<DataHolder>();
        _keyboardSoloPlayer = _dataHolder.GetKeyboardSoloPlayer();
        
        Debug.Log("AWAKE");

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

    private void ProcessLoadedScene(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Process Loaded Scene:" + scene.buildIndex);
        /*if (scene.buildIndex == _sceneLoader.GetMainMenuSceneIndex())
        { 
            ///Main Menu Stuff Here...
        }*/
        if (scene.buildIndex == _sceneLoader.GetSelectionSceneIndex())
        {
            //_selectionUIManager.InitializeSelectionNecesities();
        }
        else if (scene.buildIndex == _sceneLoader.GetFightSceneIndex())
        {
            /*_fightManager.PrepareFightNecessities();
            _fightManager.SetUpFight();*/
        }
    }

    private void Start()
    {
        Debug.Log("START");
        _mainCamera = Camera.main;
    }
    private void GatherGameNecesities() 
    {
        
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
        else
        {
            List<GameObject> playersInHolder = new List<GameObject>();
            foreach (Transform child in _playerHolder.transform)
            {
                playersInHolder.Add(child.gameObject);
            }
            _players = playersInHolder;
        }
        if (_players.Count <= 0 && playerPrefab != null) ///if there are no players in the scene (players have DontDestroyOnLoad)
        {
            GameObject player1 = Instantiate(playerPrefab);
            player1.transform.SetParent(_playerHolder.transform);
        }
        Debug.Log(_players.Count);
    }
    public void ProcessKeyboardPlayers(PlayerInput triggeredPlayerInput, string leftScheme, string rightScheme)
    {
        if (!_sceneLoader.IsSelectionScreenScene()) { return; } 

        if (_keyboardSoloPlayer)
        {
            GameObject player = Instantiate(playerPrefab);
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
    /*private void AddRequiredManagers() //WIP - Dependent on build index
    {
        int currentSceneInt = _sceneLoader.GetCurrentSceneIndex();

        switch (currentSceneInt) 
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }*/
}
