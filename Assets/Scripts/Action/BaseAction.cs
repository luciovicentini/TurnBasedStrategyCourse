using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    
    protected Unit Unit;
    protected bool IsActive;
    protected Action OnActionCompleted;

    protected virtual void Awake() {
        Unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionCompleted);
    
    public bool IsValidActionGridPosition(GridPosition gridPosition) =>
        GetValidActionGridPositionList().Contains(gridPosition);

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost() => 1;

    protected void ActionStart(Action onActionCompleted) {
        IsActive = true;
        OnActionCompleted = onActionCompleted;
        
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete() {
        IsActive = false;
        OnActionCompleted();
        
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public EnemyAIAction GetBestEnemyAIAction() {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();
        foreach (GridPosition gridPosition in validActionGridPositionList) {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0) {
            enemyAIActionList.Sort((a, b) => b.ActionValue - a.ActionValue);
            return enemyAIActionList[0];
        }

        return null;
    }

    protected abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    public Unit GetUnit() => Unit;
}
