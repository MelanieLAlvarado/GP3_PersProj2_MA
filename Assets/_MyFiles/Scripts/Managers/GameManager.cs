using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;

    GameObject _playerHolder;
    [SerializeField] private GameObject playerPrefab;
    private List<GameObject> _players = new List<GameObject>();
    public List<GameObject> GetPlayers() { return _players; }
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

        _playerHolder = new GameObject("PlayerHolder");
        if (_playerHolder)
        { 
            GameObject player1 = Instantiate(playerPrefab);
            _players.Add(player1);
        }
    }

    void Update()
    {
        
    }
}
