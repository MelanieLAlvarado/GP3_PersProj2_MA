using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneInt;
    [SerializeField] private int selectionSceneInt;
    [SerializeField] private int fightSceneInt;

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
