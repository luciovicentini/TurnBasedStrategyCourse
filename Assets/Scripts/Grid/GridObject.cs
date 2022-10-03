using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject {
    private GridSystem<GridObject> GridSystem { get; set; }
    private GridPosition GridPosition { get; set; }
    private readonly List<Unit> _unitList;
    private IInteractable _interactable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition) {
        GridSystem = gridSystem;
        GridPosition = gridPosition;
        _unitList = new List<Unit>();
    }

    public override string ToString() {
        return GridPosition + System.Environment.NewLine +
               HasUnitString() + System.Environment.NewLine +
               GridSystem;
    }

    private string HasUnitString() {
        string unitString = "";
        foreach (Unit unit in _unitList) {
            unitString += unit.ToString() + (_unitList.IndexOf(unit) == _unitList.Count - 1 ? "" : System.Environment.NewLine);
        }
        return unitString;
    }

    public void AddUnit(Unit unit) => _unitList.Add(unit);

    public void RemoveUnit(Unit unit) => _unitList.Remove(unit);

    public List<Unit> GetUnitList() => _unitList;

    public bool HasAnyUnit() => _unitList.Count > 0;

    public Unit GetUnit() => HasAnyUnit() ? _unitList[0] : null;

    public IInteractable GetInteractable() => _interactable;

    public void SetInteractable(IInteractable interactable) => _interactable = interactable;
}
