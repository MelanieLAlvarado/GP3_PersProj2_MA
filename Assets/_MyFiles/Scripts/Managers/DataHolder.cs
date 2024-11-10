using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static DataHolder m_Instance;
    private List<Player> _players = new List<Player>();
    private bool _keyboardSoloPlayer = true;
    private void Awake() 
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public List<Player> GetPlayers() { return _players; }
    public void AddPlayer(Player player) { _players.Add(player); }
    public void RemovePlayer(Player player) { _players.Remove(player); }
    public void SetKeyboardSoloPlayer(bool stateToSet) { _keyboardSoloPlayer = stateToSet; }
    public bool GetKeyboardSoloPlayer() { return _keyboardSoloPlayer; }
}
