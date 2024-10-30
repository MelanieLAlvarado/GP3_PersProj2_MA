using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUIManager : MonoBehaviour
{
    [Header("UI Info")]
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject selectionLayoutUIPrefab;
    private LayoutGroupWidget _selectionUI;
    [SerializeField] private GameObject playerSelectionLayoutUIPrefab;
    private LayoutGroupWidget _playerSelectionUI;
    [SerializeField] private GameObject fightButtonPrefab;
    private Button _fightButton;

    [SerializeField] private GameObject dragCursorPrefab;
    private CanvasCursor _dragCursor;

    [Header("Character Selections Info")]
    [SerializeField] private CharacterScriptable[] Characters;

    public CanvasCursor GetDragCursor() { return _dragCursor; }
    public void SpawnPlayerSelectionWidget(Player player) 
    {
        if (!playerSelectionLayoutUIPrefab) { return; }

        if (!_playerSelectionUI)
        {
            _playerSelectionUI = Instantiate(playerSelectionLayoutUIPrefab, canvasTransform).GetComponent<LayoutGroupWidget>();
            _playerSelectionUI.SetOwner(gameObject);
        }
        _playerSelectionUI.InitializeWidgetForPlayer(player);//change later
    }
    private void Awake()
    {
        if (!selectionLayoutUIPrefab) { return; }

        _selectionUI = Instantiate(selectionLayoutUIPrefab, canvasTransform).GetComponent<LayoutGroupWidget>();
        _selectionUI.InitializeWidgetsForCharacters(Characters);
        _selectionUI.SetOwner(gameObject);
        //GameManager.m_Instance.OnPlayerCountChanged += UpdateFightButton;
    }
    private void Start()
    {
        SpawnPlayerSelectionLayout();
        //m_OnClick.m_PersistentCalls.m_Calls
        CreateFightButton();

        _dragCursor = Instantiate(dragCursorPrefab, canvasTransform).GetComponent<CanvasCursor>();
    }
    public void SpawnPlayerSelectionLayout() 
    {
        if (!_playerSelectionUI || _playerSelectionUI.GetLayoutWidgets().Count <= 0)
        {
            List<Player> playersAvailable = GameManager.m_Instance.GetPlayers();
            foreach (Player player in playersAvailable)
            {
                SpawnPlayerSelectionWidget(player);
            }
        }
    }
    private void CreateFightButton() 
    {
        _fightButton = Instantiate(fightButtonPrefab, canvasTransform).GetComponent<Button>();

        SceneLoader sceneLoader = GameManager.m_Instance.GetSceneLoader();

        Button fightbtn = _fightButton;
        fightbtn.onClick.AddListener(delegate { sceneLoader.OpenFightScene(); });
        _fightButton.gameObject.SetActive(false);
    }

    public void UpdateFightButton() 
    {
        if (!_fightButton) { return; }

        if (_playerSelectionUI.GetLayoutWidgets().Count < 0) { return; }

        foreach (Widget widget in _playerSelectionUI.GetLayoutWidgets())
        {
            PlayerSelectionWidget playerSelection = widget.GetComponent<PlayerSelectionWidget>();
            if (playerSelection && playerSelection.GetCharacterProfile() == null)
            {
                _fightButton.gameObject.SetActive(false);
                return;
            }
        }
        _fightButton.gameObject.SetActive(true);
    }
}
