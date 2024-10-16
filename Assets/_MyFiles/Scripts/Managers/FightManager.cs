using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;

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
}
