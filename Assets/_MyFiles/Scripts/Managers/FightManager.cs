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
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private float respawnWait = 1.5f;

    [Header("GameplayUI Info (test)")]
    [SerializeField] private Transform canvasTransform;
    private LayoutGroupWidget _gameplayUI;
    [SerializeField] private GameObject gameplayUILayoutPrefab;

    private void Awake()
    {
        if (spawnPositions.Length > 0)
        {
            RemoveDuplicateSpawnPositions();
        }
    }
    private void Start()
    {
        Debug.Log($"Spawn Positions Length: {spawnPositions.Length}");
        if (spawnPositions.Length < 0)
        {
            return;
        }
        
        List<GameObject> playerList = GameManager.m_Instance.GetPlayers();
        List<Transform> spawnPosList = spawnPositions.ToList();
        Debug.Log($"Player List Count: {playerList.Count}");

        foreach (GameObject player in playerList) 
        {
            int randomSpawnIndex = Random.Range(0, spawnPosList.Count);
            Transform randomSpawnPos = spawnPosList[randomSpawnIndex];
            player.GetComponent<Player>().SpawnCharacter(randomSpawnPos);

            spawnPosList.RemoveAt(randomSpawnIndex);
            //GameManager.m_Instance.GetGameplayUIManager().SpawnPlayerGameplaySlot(player);
        }
        //GameManager.m_Instance.GetGameplayUIManager().InitializeGamePlayWidgets(playerList);

        //GameManager.m_Instance.GetUIManager().SpawnGameplayUI();
        //GameManager.m_Instance.GetUIManager().InitializeWidgetsForGameobjects(playerList);
        _gameplayUI = Instantiate(gameplayUILayoutPrefab, canvasTransform).GetComponent<LayoutGroupWidget>();
        _gameplayUI.InitializeWidgetsForGameobjects(playerList);
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
            player.SpawnCharacter(respawnPosition);
        }
        StopCoroutine(RespawnDelay(player));
    }
}
