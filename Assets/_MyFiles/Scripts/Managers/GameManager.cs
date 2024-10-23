using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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

    public SelectionUIManager GetSelectUIManager() { return _selectionUIManager; }
    public FightManager GetFightManager() { return _fightManager; }
    public GameplayUIManager GetGameplayUIManager() { return _gameplayUIManager; }
    private void Awake()
    {
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
        else
        {
            List<GameObject> playersInHolder = new List<GameObject>();
            foreach (Transform child in _playerHolder.transform)
            {
                playersInHolder.Add(child.gameObject);
            }
            _players = playersInHolder;
        }
        if (_players.Count <= 0 && playerPrefab != null) //if there are no players in the scene (players have DontDestroyOnLoad)
        {
            GameObject player1 = Instantiate(playerPrefab);
            player1.transform.SetParent(_playerHolder.transform);
        }
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
