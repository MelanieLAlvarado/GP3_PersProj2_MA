using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    GameObject _playerHolder;
    private List<GameObject> _players = new List<GameObject>();
    private void Awake()
    {
        _playerHolder = new GameObject("PlayerHolder");
        
    }

    void Update()
    {
        
    }
}
