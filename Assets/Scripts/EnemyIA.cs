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
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActions()) {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction)) continue;

            if (bestEnemyAIAction == null) {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.ActionValue > bestEnemyAIAction.ActionValue) {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction == null) return false;
        if (bestBaseAction == null) return false;
        if (!enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction)) return false;
        
        bestBaseAction.TakeAction(bestEnemyAIAction.GridPosition, onEnemyAIActionCompleted);
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
