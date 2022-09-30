using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction {
    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    
    private int _maxSwordDistance = 1;
    private State _state;
    private Unit _targetUnit;
    private float _stateTimer;

    private enum State {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private void Update() {
        if (!IsActive) return;
        
        _stateTimer -= Time.deltaTime;
        
        switch (_state) {
            case State.SwingingSwordBeforeHit:
                RotateTowardsTargetUnit();
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (_stateTimer <= 0f) {
            NextState();
        }
    }
    
    private void NextState() {
        switch (_state) {
            case State.SwingingSwordBeforeHit:
                _state = State.SwingingSwordAfterHit;
                float shootStateTime = 0.7f;
                _stateTimer = shootStateTime;
                _targetUnit.Damage(200);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    private void RotateTowardsTargetUnit() {
        Vector3 rotateDir = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;

        float rotateSpeed = 3f;
        transform.forward = Vector3.Lerp(transform.forward, rotateDir, (Time.deltaTime * rotateSpeed));
    }
    
    public override string GetActionName() => "Sword";

    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted) {
        
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = State.SwingingSwordBeforeHit;
        float aimStateTime = .5f;
        _stateTimer = aimStateTime;
        
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionCompleted);
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition gridPosition = Unit.GetGridPosition();
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++) {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit unitTarget = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (unitTarget.IsEnemy() == Unit.IsEnemy()) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) =>
        new() {
            GridPosition = gridPosition,
            ActionValue = 200,
        };

    public int GetSwordDistance() => _maxSwordDistance;
}