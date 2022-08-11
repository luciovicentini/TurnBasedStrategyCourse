using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction {

    public event EventHandler<OnShootEventArgs> OnShooting;

    public class OnShootEventArgs {
        public Unit TargetUnit;
        public Unit ShootingUnit;
    }
    
    private enum State {
        Aiming,
        Shooting,
        CoolOff,
    }
    private int _maxShootDistance = 7;
    private State _state;
    private float _stateTimer;
    private bool _canShootBullet;
    private Unit _targetUnit;

    private void Update() {
        if (!IsActive) return;

        _stateTimer -= Time.deltaTime;
        switch (_state) {
            case State.Aiming:
                RotateTowardsTargetUnit();
                break;
            case State.Shooting:
                if (_canShootBullet) {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.CoolOff:
                break;
        }

        if (_stateTimer <= 0f) {
            NextState();
        }
    }

    private void NextState() {
        switch (_state) {
            case State.Aiming:
                _state = State.Shooting;
                float shootStateTime = 0.1f;
                _stateTimer = shootStateTime;
                break;
            case State.Shooting:
                _state = State.CoolOff;
                float coolOffStateTime = 0.5f;
                _stateTimer = coolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }
    }

    private void Shoot() {
        OnShooting?.Invoke(this, new OnShootEventArgs{TargetUnit = _targetUnit, ShootingUnit = Unit});

        const int shootingDamage = 40;
        _targetUnit.Damage(shootingDamage);
    }

    private void RotateTowardsTargetUnit() {
        Vector3 rotateDir = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
        
        float rotateSpeed = 3f;
        transform.forward = Vector3.Lerp(transform.forward, rotateDir, (Time.deltaTime * rotateSpeed));
    }

    public override string GetActionName() => "Shoot";

    public override void TakeAction(GridPosition gridPosition, Action onActionCompleted) {
        ActionStart(onActionCompleted);
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        _state = State.Aiming;
        float aimStateTime = 1f;
        _stateTimer = aimStateTime;
        _canShootBullet = true;
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = Unit.GetGridPosition();
        
        for (var x = -_maxShootDistance; x < _maxShootDistance; x++) {
            for (var z = -_maxShootDistance; z < _maxShootDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootDistance) continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit unitTarget = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (unitTarget.IsEnemy() == Unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
}
