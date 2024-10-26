using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Indexs")] //values edited in inspector
    [SerializeField] private int mainMenuSceneInt;
    [SerializeField] private int selectionSceneInt;
    [SerializeField] private int fightSceneInt;

    /*private void Awake()
    {
        
    }
    private void AddRequiredManagers() //WIP - Dependent on build index
    {
        int currentSceneInt = SceneManager.GetActiveScene().buildIndex;
        


        switch (currentSceneInt)
        {
            case 0:
                GameManager.m_Instance.AddMainMenuSceneManagers();
                break;
            case 1:
                GameManager.m_Instance.AddSelectionSceneManagers();
                break;
            case 2:
                GameManager.m_Instance.AddFightSceneManagers();
                break;
        }
    }*/

    public int GetCurrentSceneIndex() { return SceneManager.GetActiveScene().buildIndex; }
    public int GetMainMenuSceneIndex() { return mainMenuSceneInt; }
    public int GetSelectionSceneIndex() {  return selectionSceneInt; }
    public int GetFightSceneIndex() { return fightSceneInt; }

    public bool IsSelectionScreenScene() 
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == selectionSceneInt)
        { 
            return true;
        }
        return false;
    }

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
    public void Quit() 
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
