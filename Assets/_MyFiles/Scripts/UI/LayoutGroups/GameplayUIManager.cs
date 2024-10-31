using System.Collections.Generic;
using UnityEngine;

public class GameplayUIManager : LayoutGroupWidget
{
    public bool CanAddGameplayWidget(Player player) { return !_widgetDictionary.ContainsKey(player); }
    
    public void AttachPlayerToWidget(Player player)
    {
        Widget widget = _widgetDictionary[player];
        GameplayCharacterSlotWidget gameSlotUI = widget.GetComponent<GameplayCharacterSlotWidget>();

        GameObject currentChar = player.GetCurrentFightingCharacter();

        HealthComponent healthComponent = currentChar.GetComponent<HealthComponent>();
        if (healthComponent)
        {
            gameSlotUI.UpdateHealthText(healthComponent.GetHealth());
            healthComponent.OnHealthChanged += gameSlotUI.UpdateHealthText;
        }

        player.OnLivesChanged += gameSlotUI.UpdateLifeText;
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
    }
}
