using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction {

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    
    [SerializeField] private int maxMoveDistance = 4;
    
    private Vector3 _targetPosition;
    
    protected override void Awake() {
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update() {
        if (!IsActive) return;
        
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, _targetPosition) < stoppingDistance) {
            // unitAnimator.SetBool("isWalking", false);
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            
            ActionComplete();
        }
        else {
            // unitAnimator.SetBool("isWalking", true);
            MoveAndRotate();
        }
    }

    private void MoveAndRotate() {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        Move(moveDir);
        Rotate(moveDir);
    }

    private void Move(Vector3 moveDir) {
        float moveSpeed = 4f;
        transform.position += moveDir * (Time.deltaTime * moveSpeed);
    }

    private void Rotate(Vector3 direction) {
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, direction, (Time.deltaTime * rotateSpeed));
    }
    
    public override void TakeAction(GridPosition target, Action onActionCompleted) {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(target);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        
        ActionStart(onActionCompleted);
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GetGridPosition();
        
        for (int x = -maxMoveDistance; x < maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z < maxMoveDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                if (unitGridPosition == testGridPosition) continue;
                
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {
        int targetCountAtGridPosition = Unit.GetAction<ShootAction>().GetTargetCountAtGridPosition(gridPosition);
        
        return new EnemyAIAction {
            GridPosition = gridPosition,
            ActionValue = targetCountAtGridPosition * 10,
        };
    }

    public override string GetActionName() => "Move";
}
