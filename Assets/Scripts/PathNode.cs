using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    
    public PathNode(GridPosition gridPosition) {
        this.gridPosition = gridPosition;
    }

    public int GetGCost() => gCost;
    public int GetHCost() => hCost;
    public int GetFCost() => fCost;

    public override string ToString() => gridPosition.ToString();
}
