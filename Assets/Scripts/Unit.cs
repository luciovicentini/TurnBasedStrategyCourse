using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    private const int ACTION_POINTS_MAX = 2;

    [SerializeField] private bool isEnemy;
    
    public static event EventHandler OnAnyActionPointChanged; 

    private GridPosition _gridPosition;
    private HealthSystem _healthSystem;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = ACTION_POINTS_MAX;

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() {
        _healthSystem.OnDead += HealthSystem_OnDead;
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }
    
    private void Update() {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition) {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        return _actionPoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int amount) {
        _actionPoints -= amount;
        
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        
        Destroy(gameObject);
    }

    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn())) {
            _actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
        }
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
    public Vector3 GetWorldPosition() => transform.position;
    public BaseAction[] GetBaseActions() => _baseActionArray;
    public int GetActionPoints() => _actionPoints;

    public bool IsEnemy() => isEnemy;

    public void Damage(int damageAmount) {
        _healthSystem.TakeDamage(damageAmount);
    }
}