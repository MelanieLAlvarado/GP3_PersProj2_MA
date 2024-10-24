using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    [Header("Spawn Info")]
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private float respawnWait = 1.5f;

    [Header("GameplayUI Info")]
    [SerializeField] private Transform canvasTransform;
    private GameplayUIManager _gameplayUI;
    [SerializeField] private GameObject gameplayUILayoutPrefab;

    [SerializeField] private GameObject pauseMenuUI;//may spawn later
    private bool _isPaused = false;

    [Header("Camera Info")]
    [SerializeField] private Transform battleCamSpawnPos;
    [SerializeField] private GameObject battleCamPrefab;
    private GameObject _battleCamObj;
    private BattleCamera _battleCam;

    public BattleCamera GetBattleCam() { return _battleCam; }

    private void Awake()
    {
        if (pauseMenuUI)
        { 
            ResumeGame();
        }
        if (spawnPositions.Length > 0)
        {
            RemoveDuplicateSpawnPositions();
        }
        _battleCamObj = Instantiate(battleCamPrefab, battleCamSpawnPos.position, battleCamSpawnPos.rotation);
        _battleCam = _battleCamObj.GetComponent<BattleCamera>();
    }
    private void Start()
    {
        if (spawnPositions.Length < 0)
        {
            return;
        }
        
        List<GameObject> playerList = GameManager.m_Instance.GetPlayers();
        List<Transform> spawnPosList = spawnPositions.ToList();

        _gameplayUI = Instantiate(gameplayUILayoutPrefab, canvasTransform).GetComponent<GameplayUIManager>();
        _gameplayUI.SetOwner(gameObject);
        foreach (GameObject player in playerList) 
        {
            int randomSpawnIndex = Random.Range(0, spawnPosList.Count);
            Transform randomSpawnPos = spawnPosList[randomSpawnIndex];
            player.GetComponent<Player>().SpawnCharacter(randomSpawnPos, _gameplayUI);
            spawnPosList.RemoveAt(randomSpawnIndex);

            player.GetComponent<PlayerController>().OnPauseTriggered += OnPauseAction;
        }
    }
    private void RemoveDuplicateSpawnPositions()
    {
        List<Transform> tempSpawnPositions = new List<Transform>();

        foreach (Transform spawnPos in spawnPositions)
        {
            if (tempSpawnPositions.Contains(spawnPos))
            {
                continue;
            }
            tempSpawnPositions.Add(spawnPos);
        }
        Array.Clear(spawnPositions, 0, spawnPositions.Length);
        spawnPositions = tempSpawnPositions.ToArray();

    }
    public void StartRespawnDelay(Player player) 
    {
        StartCoroutine(RespawnDelay(player));
    }
    private IEnumerator RespawnDelay(Player player) 
    {
        yield return new WaitForSeconds(respawnWait);
        if (respawnPosition && player.GetPlayerLifes() > 0)
        { 
            player.SpawnCharacter(respawnPosition, _gameplayUI);
        }
        StopCoroutine(RespawnDelay(player));
    }

    public void OnPauseAction()
    {
        if (!pauseMenuUI)
        {
            return;
        }


        if (_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
        _isPaused = !_isPaused;
    }
    private void PauseGame()
    { 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void ResumeGame() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
