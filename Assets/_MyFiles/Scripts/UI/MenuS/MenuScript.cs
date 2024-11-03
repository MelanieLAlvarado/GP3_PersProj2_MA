using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    SceneLoader _sceneLoader;
    FightManager _fightManager;

    [Header("Button Info")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button selectionButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
        if (GameManager.m_Instance)
        { 
            _sceneLoader = GameManager.m_Instance.GetSceneLoader();
            _fightManager = GameManager.m_Instance.GetFightManager();
        }
        ConnectAllMenuButtons();
    }
    public void ConnectAllMenuButtons() 
    {
        ConnectResumeButton();
        ConnectSelectionButton();
        ConnectMainMenuButton();
        ConnectQuitButton();
    }

    private void ConnectResumeButton()
    {
        if (resumeButton && _fightManager)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(delegate { _fightManager.OnPauseAction(); });
        }
    }
    private void ConnectSelectionButton()
    {
        if (selectionButton && _sceneLoader)
        {
            selectionButton.onClick.RemoveAllListeners();
            selectionButton.onClick.AddListener(delegate { _sceneLoader.OpenSelectionScene(); });
        }
    }

    private void ConnectMainMenuButton()
    {
        if (mainMenuButton && _sceneLoader)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(delegate { _sceneLoader.OpenMainMenu(); });
        }
    }

    private void ConnectQuitButton()
    {
        if (quitButton && _sceneLoader)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(delegate { _sceneLoader.Quit(); });
        }
    }
}
