using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CombatRpgManager : NetworkBehaviour 
{
    public bool canBattle;
    public Weapon heldWeapon;
    public gameState currentGameState;
    [SerializeField]
    private KeyCode gameStateSwitchButton;
    [SerializeField]
    private MovementAnim animController;
    public enum gameState
    {
        neutral,combat
    }
    void Start()
    {
        if (IsLocalPlayer)
        {
            changeGameState(gameState.neutral);
        }
    }
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (!canBattle) { return; }
            gameStateCheck();
        }
    }
    public void changeGameState(gameState value)
    {
        currentGameState = value;
    }
    private void gameStateCheck()
    {
        if (!Input.GetKeyDown(gameStateSwitchButton)) { return ; }
        if(currentGameState == gameState.neutral)
        {
            changeGameState(gameState.combat);
            ChangeanimLayer(currentGameState);
        }
        else
        {
            changeGameState(gameState.neutral);
            ChangeanimLayer(currentGameState);
        }
    }
    private void ChangeanimLayer(gameState current_gameState)
    {
        Debug.Log($"Current : {currentGameState}");
        switch (current_gameState)
        {
            case gameState.neutral:
                animController.changeCombatLayerWeightServerRpc(0f);
                break;
            case gameState.combat:
                animController.changeCombatLayerWeightServerRpc(1f);
                break;
        }
    }
}
