using UnityEngine;

public class DataHolder : MonoBehaviour
{
    private bool _keyboardSoloPlayer = true;
    public void SetKeyboardSoloPlayer(bool stateToSet) { _keyboardSoloPlayer = stateToSet; }
    public bool GetKeyboardSoloPlayer() { return _keyboardSoloPlayer; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
