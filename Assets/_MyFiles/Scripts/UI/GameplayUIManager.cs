using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : LayoutGroupWidget
{
    private Dictionary<Player, Widget> _gameplaySlots = new Dictionary<Player, Widget>();

    public bool CanAddGameplayWidget(Player player) { return !_gameplaySlots.ContainsKey(player); }
    public void DisconnectPlayerFromWidget(Player player) 
    {
        player.OnPlayerDead -= DisconnectPlayerFromWidget;

        Widget widget = _gameplaySlots[player];
        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();
        GameObject currentChar = player.GetCurrentFightingCharacter();
        
        currentChar.GetComponent<HealthComponent>().OnHealthChanged -= gameSlotUI.UpdateHealthText;
    }
    public void AttachPlayerToWidget(Player player)
    {
        Widget widget = _gameplaySlots[player];
        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();

        GameObject currentChar = player.GetCurrentFightingCharacter();

        HealthComponent healthComponent = currentChar.GetComponent<HealthComponent>();
        if (healthComponent)
        {
            gameSlotUI.UpdateHealthText(healthComponent.GetHealth());
            healthComponent.OnHealthChanged += gameSlotUI.UpdateHealthText;
            player.OnPlayerDead += DisconnectPlayerFromWidget;
        }
    }
    public override void InitializeWidget(GameObject connectedObj, Widget widget) 
    {
        Player player = connectedObj.GetComponent<Player>();
        if (!player)
        {
            Debug.LogError("There is no Player Scripts on this Object!");
            return;
        }    

        CharacterScriptable character = player.GetCharacter();
        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();
        if (!gameSlotUI || !character)
        {
            Debug.LogError("There is no CharacterScriptable on this Object (or) a widget!");

            return;
        }

        gameSlotUI.SetCharacterInSlot(character);
        gameSlotUI.SetPlayerNameText(player.GetPlayerName());

        GameObject currentChar = player.GetCurrentFightingCharacter();
        if (!currentChar)
        {
            Debug.LogError("There is no character GameObjcet on this Object!");
            return;
        }
        if (!_gameplaySlots.ContainsKey(player)) 
        {
            _gameplaySlots.Add(player, widget);
        }

        HealthComponent healthComponent = currentChar.GetComponent<HealthComponent>();
        if (healthComponent)
        {
            healthComponent.OnHealthChanged += gameSlotUI.UpdateHealthText;
            //healthComponent.OnDead += DisconnectPlayerFromWidget;
        }
    }
}
