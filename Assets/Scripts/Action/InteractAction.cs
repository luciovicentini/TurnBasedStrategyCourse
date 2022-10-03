using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction {
    private const int MaxInteractDistance = 1;
    
    public override string GetActionName() => "Interact";

    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted) {
        ActionStart(onActionCompleted);
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        if (door == null) return;
        
        door.Interact(OnInteractComplete);
    }

    private void OnInteractComplete() {
        ActionComplete();
    }
    
    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition gridPosition = Unit.GetGridPosition();
        for (int x = -MaxInteractDistance; x <= MaxInteractDistance; x++) {
            for (int z = -MaxInteractDistance; z <= MaxInteractDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if (door == null) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) => new() {
        GridPosition = gridPosition,
        ActionValue = 0,
    };
}