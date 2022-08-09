using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour {
    protected Unit Unit;
    protected bool IsActive;
    protected Action OnActionComplete;

    protected virtual void Awake() {
        Unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionCompleted);
    
    public bool IsValidActionGridPosition(GridPosition gridPosition) =>
        GetValidActionGridPositionList().Contains(gridPosition);

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost() => 1;
}
