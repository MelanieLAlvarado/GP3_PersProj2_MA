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

    [SerializeField] private GameObject pauseMenuPrefab;
    private GameObject _pauseMenuUI;
    private bool _isPaused = false;

    [Header("Camera Info")]
    [SerializeField] private Transform battleCamSpawnPos;
    [SerializeField] private GameObject battleCamPrefab;
    private GameObject _battleCamObj;
    private BattleCamera _battleCam;

    [Header("Fight End Info")]
    [SerializeField] private GameEndMenu gameEndMenuPrefab;
    private GameEndMenu _gameEndMenu;
    private bool _bIsFightActive;


    public BattleCamera GetBattleCam() { return _battleCam; }

    public bool GetIsFightActive() { return _bIsFightActive; }
    private void Awake()
    {
        PrepareFightNecessities();
    }
    private void Start()
    {
        SetUpFight();
    }

    private void PrepareFightNecessities()
    {
        if (spawnPositions.Length > 0)
        {
            RemoveDuplicateSpawnPositions();
        }
        _battleCamObj = Instantiate(battleCamPrefab, battleCamSpawnPos.position, battleCamSpawnPos.rotation);
        _battleCam = _battleCamObj.GetComponent<BattleCamera>();
    }

    private void SetUpFight() 
    {
        if (spawnPositions.Length < 0)
        {
            return;
        }

        List<Player> playerList = DataHolder.m_Instance.GetPlayers();
        List<Transform> spawnPosList = spawnPositions.ToList();

        _gameplayUI = Instantiate(gameplayUILayoutPrefab, canvasTransform).GetComponent<GameplayUIManager>();
        _gameplayUI.SetOwner(gameObject);

        PreparePauseMenu();
        foreach (Player player in playerList)
        {
            player.ResetPlayerLifes();
            player.OnNoPlayerLives += CheckAllPlayersInBattle;

            int randomSpawnIndex = Random.Range(0, spawnPosList.Count);
            Transform randomSpawnPos = spawnPosList[randomSpawnIndex];
            player.SpawnCharacter(randomSpawnPos, _gameplayUI);
            spawnPosList.RemoveAt(randomSpawnIndex);

            player.GetComponent<PlayerController>().OnPauseTriggered += OnPauseAction;
        }
        _bIsFightActive = true;
    }
    private void PreparePauseMenu() 
    {
        _pauseMenuUI = Instantiate(pauseMenuPrefab, canvasTransform);
        if (_pauseMenuUI)
        {
            ResumeGame();
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
        if (!_pauseMenuUI)
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
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void ResumeGame() 
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void CheckAllPlayersInBattle() 
    {
        List<Player> players = DataHolder.m_Instance.GetPlayers();
        List<Player> remainingPlayers = new List<Player>();
        foreach (Player player in players)
        {
            if (player.GetPlayerLifes() > 0)
            { 
                remainingPlayers.Add(player);
            }
        }
        if (remainingPlayers.Count > 1)
        {
            return;
        }
        EndGame(remainingPlayers[0]);
    }
    public void EndGame(Player winnerPlayer)
    {
        _gameEndMenu = Instantiate(gameEndMenuPrefab, canvasTransform);
        
        string playerName = winnerPlayer.GetPlayerName();
        _gameEndMenu.SetCharacterWinText(playerName);
        CharacterScriptable charScriptable = winnerPlayer.GetCharacter();
        _gameEndMenu.GetWinnerSlot().SetCharacterInSlot(charScriptable);

        Time.timeScale = 0;
        Cursor.lockState= CursorLockMode.None;
        Cursor.visible = true;
    }
}
