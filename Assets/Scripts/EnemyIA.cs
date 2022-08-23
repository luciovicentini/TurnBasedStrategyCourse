using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {

    private enum State {
        WaitingForEnemyTurn,
        TalkingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake() {
        state = State.WaitingForEnemyTurn;
    }

    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update() {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state) {
            case State.WaitingForEnemyTurn:
                break;
            case State.TalkingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn)) {
                        state = State.Busy;
                    }
                    else {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
            
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionCompleted) {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()) {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionCompleted)) {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionCompleted) {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        GridPosition unitGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(unitGridPosition)) return false;
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;
        
        spinAction.TakeAction(unitGridPosition, onEnemyAIActionCompleted);
        return true;
    }

    private void SetStateTakingTurn() {
        timer = 0.5f;
        state = State.TalkingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        timer = 2f;
        state = State.TalkingTurn;
    }
}
