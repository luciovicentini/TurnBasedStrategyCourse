using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction {
    
    private float _totalSpinAmount;

    private void Update() {
        if (!IsActive) return;
        
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount > 360f) {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition _, Action onActionCompleted) {
        _totalSpinAmount = 0f;
        ActionStart(onActionCompleted);
    }

    public override List<GridPosition> GetValidActionGridPositionList() => new() {Unit.GetGridPosition()};

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) =>
        new EnemyAIAction {
            GridPosition = gridPosition,
            ActionValue = 0,
        };

    public override string GetActionName() => "Spin";

    public override int GetActionPointsCost() => 2;
}
