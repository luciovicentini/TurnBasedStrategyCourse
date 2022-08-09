using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    private GridPosition _gridPosition;
    
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = 2;
    
    private void Awake() {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void Update() {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        return _actionPoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int amount) {
        _actionPoints -= amount;
    }
    
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) {
        if (CanSpendActionPointsToTakeAction(baseAction)) {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }

        return false;
    }
    
    public MoveAction GetMoveAction() => _moveAction;
    public SpinAction GetSpinAction() => _spinAction;
    public GridPosition GetGridPosition() => _gridPosition;
    public BaseAction[] GetBaseActions() => _baseActionArray;
    public int GetActionPoints() => _actionPoints;
}