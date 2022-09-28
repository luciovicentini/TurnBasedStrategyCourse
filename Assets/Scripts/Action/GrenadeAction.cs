using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction {
    [SerializeField] private GrenadeProjectile grenadeProjectile;

    private int _maxThrowDistance = 7;

    private void Update() {
        if (!IsActive) return;
    }

    public override string GetActionName() {
        return "Grenade";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted) {
        GrenadeProjectile grenade = Instantiate(grenadeProjectile, Unit.GetWorldPosition(), Quaternion.identity);
        grenade.Setup(LevelGrid.Instance.GetWorldPosition(gridPosition), OnGrenadeBehaviourComplete);
        ActionStart(onActionCompleted);
    }

    private void OnGrenadeBehaviourComplete() {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition gridPosition = Unit.GetGridPosition();
        for (int x = -_maxThrowDistance; x < _maxThrowDistance; x++) {
            for (int z = -_maxThrowDistance; z < _maxThrowDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxThrowDistance) continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        return new EnemyAIAction {
            GridPosition = Unit.GetGridPosition(),
            ActionValue = 0,
        };
    }
}