using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2f;

    private GridSystem<GridObject> _gridSystem;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one LevelGrid Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start() {
        _gridSystem = new GridSystem<GridObject>(width, height, cellSize,
            (system, position) => new GridObject(system, position));
        // _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition) {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) =>
        _gridSystem.GetGridObject(gridPosition).AddUnit(unit);

    public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition) {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) =>
        _gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) =>
        _gridSystem.GetGridObject(gridPosition).HasAnyUnit();

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition).GetUnit();
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition) => _gridSystem.GetGridObject(gridPosition).GetInteractable();

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable) =>
        _gridSystem.GetGridObject(gridPosition).SetInteractable(interactable);

    public int GetWidth() => _gridSystem.GetWidth();

    public int GetHeight() => _gridSystem.GetHeight();
}