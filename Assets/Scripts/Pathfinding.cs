using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one UnitActionSystem Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (_, gridPosition) => new PathNode(gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        SetupPathNodeIsWalkable();
    }

    private void SetupPathNodeIsWalkable() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance,
                        Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask)) {
                    GetPathNode(x,z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLenght) {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startPathNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endPathNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startPathNode);

        ResetAllPathNode();

        startPathNode.SetGCost(0);
        startPathNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startPathNode.CalculateFCost();

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if (currentNode == endPathNode) {
                pathLenght = endPathNode.GetFCost();
                return CalculatePath(endPathNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.IsWalkable()) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() +
                                     CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost()) {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        pathLenght = 0;
        return null;
    }

    private void ResetAllPathNode() {
        for (int x = 0; x < gridSystem.GetWidth(); x++) {
            for (int z = 0; z < gridSystem.GetHeight(); z++) {
                PathNode pathNode = gridSystem.GetGridObject(new GridPosition(x, z));
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList) {
        PathNode lowestPathNode = pathNodeList[0];
        foreach (PathNode pathNode in pathNodeList) {
            if (pathNode.GetFCost() < lowestPathNode.GetFCost()) {
                lowestPathNode = pathNode;
            }
        }

        return lowestPathNode;
    }

    private int CalculateDistance(GridPosition startGridPosition, GridPosition endGridPosition) {
        GridPosition gridPositionDistance = endGridPosition - startGridPosition;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetPathNode(int x, int z) => gridSystem.GetGridObject(new GridPosition(x, z));

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition currentGridPosition = currentNode.GetGridPosition();
        for (int x = -1; x <= 1; x++) {
            for (int z = -1; z <= 1; z++) {
                int xTesting = currentGridPosition.x + x;
                int zTesting = currentGridPosition.z + z;

                if (xTesting < 0 || xTesting > gridSystem.GetWidth()) continue;
                if (zTesting < 0 || zTesting > gridSystem.GetHeight()) continue;

                neighbourList.Add(GetPathNode(xTesting, zTesting));
            }
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode) {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode current = endNode;
        while (current.GetCameFromPathNode() != null) {
            pathNodeList.Add(current.GetCameFromPathNode());
            current = current.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode item in pathNodeList) {
            gridPositionList.Add(item.GetGridPosition());
        }

        return gridPositionList;
    }

    public bool IsWalkable(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).IsWalkable();
    public void SetIsWalkable(GridPosition gridPosition, bool isWalkable) => gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);

    public bool HasPath(GridPosition start, GridPosition end) => FindPath(start, end, out _) != null;
    
    public int GetFindPathLength(GridPosition start, GridPosition end) {
        FindPath(start, end, out int pathLenght);
        return pathLenght;
    }
}