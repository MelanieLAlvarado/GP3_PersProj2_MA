using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Indexs")] //values edited in inspector
    [SerializeField] private int mainMenuSceneInt;
    [SerializeField] private int selectionSceneInt;
    [SerializeField] private int fightSceneInt;

    public int GetCurrentSceneIndex() { return SceneManager.GetActiveScene().buildIndex; }
    public int GetMainMenuSceneIndex() { return mainMenuSceneInt; }
    public int GetSelectionSceneIndex() {  return selectionSceneInt; }
    public int GetFightSceneIndex() { return fightSceneInt; }

    public void OpenMainMenu()
    {
        OpenScene(mainMenuSceneInt);

    }
    public void OpenSelectionScene() 
    {
        OpenScene(selectionSceneInt);
    }
    public void OpenFightScene()
    {
        OpenScene(fightSceneInt);
    }
    private void OpenScene(int sceneIndex) 
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
