using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject {
    public GridSystem GridSystem { get; private set; }
    public GridPosition GridPosition { get; private set; }

    public GridObject(GridSystem gridSystem, GridPosition gridPosition) {
        GridSystem = gridSystem;
        GridPosition = gridPosition;
    }

    public override string ToString() {
        return GridPosition + System.Environment.NewLine + GridSystem;
    }
}
