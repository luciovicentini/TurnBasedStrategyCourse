using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject {
    public GridSystem GridSystem { get; private set; }
    public GridPosition GridPosition { get; private set; }

    private readonly List<Unit> _unitList; 

    public GridObject(GridSystem gridSystem, GridPosition gridPosition) {
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
}
