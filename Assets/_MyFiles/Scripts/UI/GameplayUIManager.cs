using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : LayoutGroupWidget
{
    [Header("Player Selection Info")]
    [SerializeField] private Transform playerGameplayWidgetGroup;
    [SerializeField] private GameObject playerGameplayUIPrefab;
    private List<GameplayCharacterSlotWidget> _playerGamePlayWidgets = new List<GameplayCharacterSlotWidget>();
    
    public void InitializeGamePlayWidgets(List<GameObject> players) 
    {
        foreach (GameObject player in players) 
        {
            SpawnPlayerGameplaySlot(player);
        }
    }
    public void SpawnPlayerGameplaySlot(GameObject playerObj) 
    {
        Player player = playerObj.GetComponent<Player>();
        CharacterScriptable character = player.GetCharacter();

        GameObject gameSlot = Instantiate(playerGameplayUIPrefab);
        gameSlot.transform.SetParent(playerGameplayWidgetGroup);

        GameplayCharacterSlotWidget gameSlotUI = gameSlot.GetComponent<GameplayCharacterSlotWidget>();
        gameSlotUI.SetCharacterInSlot(character);
        gameSlotUI.SetPlayerNameText(player.GetPlayerName());
        gameSlotUI.SetOwner(playerObj);


        GameObject currentChar = player.GetCurrentFightingCharacter();
        if (!currentChar.GetComponent<HealthComponent>())
        {
            return;
        }
        currentChar.GetComponent<HealthComponent>().OnHealthChanged += gameSlotUI.UpdateHealthText;
    }
    public override void InitializeWidget(GameObject connectedObj, Widget widget) 
    {
        Player player = connectedObj.GetComponent<Player>();
        CharacterScriptable character = player.GetCharacter();


        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();
        gameSlotUI.SetCharacterInSlot(character);

        gameSlotUI.SetPlayerNameText(player.GetPlayerName());

        GameObject currentChar = player.GetCurrentFightingCharacter();
        if (!currentChar.GetComponent<HealthComponent>())
        {
            return;
        }
        currentChar.GetComponent<HealthComponent>().OnHealthChanged += gameSlotUI.UpdateHealthText;
    }
}
