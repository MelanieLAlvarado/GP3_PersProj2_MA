using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[RequireComponent(typeof(SceneLoader))]
public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    private Camera _mainCamera;

    [Header("Managers")]//shouldn't show in inspector
    private SceneLoader _sceneLoader;

    [Header("Player Info")]
    [SerializeField] private string playerHolderName = "PlayerHolder";
    GameObject _playerHolder;
    [SerializeField] private GameObject playerPrefab;
    private List<GameObject> _players = new List<GameObject>();
    public List<GameObject> GetPlayers() { return _players; }
    public void AddPlayer(GameObject player) 
    { 
        _players.Add(player); 
    }
    public GameObject GetPlayerHolder() { return _playerHolder; }
    public Camera GetMainCamera() { return _mainCamera;}

    public SelectionUIManager GetSelectUIManager() { return GetComponent<SelectionUIManager>(); }
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

        CheckPresentPlayers();
    }
    private void Start()
    {
        _mainCamera = Camera.main;
        _sceneLoader = GetComponent<SceneLoader>();
        AddRequiredManagers();
    }
    

    void Update()
    {
        
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
        if (_players.Count <= 0 && playerPrefab) //if there are no players in the scene (players have DontDestroyOnLoad)
        {
            GameObject player1 = Instantiate(playerPrefab);
            _players.Add(player1);
            player1.transform.SetParent(_playerHolder.transform);
        }
    }

    private void AddRequiredManagers() //WIP
    {
        int currentSceneInt = _sceneLoader.GetCurrentSceneIndex();
        int mainSceneInt = _sceneLoader.GetMainMenuSceneIndex();


        /*switch (currentSceneInt) 
        {
            case mainSceneInt:
                break;
        }*/
    }
}
