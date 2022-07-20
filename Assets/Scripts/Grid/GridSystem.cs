using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GridSystem {
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] _gridObjectArray;
    
    public GridSystem(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        
        CreateGridObject();
    }

    private void CreateGridObject() {
        _gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                _gridObjectArray[x, z] = new GridObject(this, new GridPosition(x, z));
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) => new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;

    public GridPosition GetPosition(Vector3 worldPosition) =>
        new(Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize));

    public void CreateDebugObjects(Transform debugPrefab) {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                debugObject.GetComponent<GridDebugObject>().SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition) => _gridObjectArray[gridPosition.x, gridPosition.z];

    public bool IsValidGridPosition(GridPosition gridPosition) => gridPosition.x >= 0 &&
                                                                  gridPosition.x < width &&
                                                                  gridPosition.z >= 0 &&
                                                                  gridPosition.z < height;

    public int GetWidth() => width;
    
    public int GetHeight() => height;
}
