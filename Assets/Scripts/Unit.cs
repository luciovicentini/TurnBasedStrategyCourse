using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    private MoveAction _moveAction;
    private GridPosition _gridPosition;

    private void Awake() {
        _moveAction = GetComponent<MoveAction>();
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

    public MoveAction GetMoveAction() => _moveAction;

    public GridPosition GetGridPosition() => _gridPosition;
}