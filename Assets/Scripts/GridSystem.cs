using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GridSystem {
    private int width;
    private int height;
    private float cellSize;

    public GridSystem(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        LogGridSystem();
    }

    private void LogGridSystem() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                Debug.DrawLine(GetWorldPosition(x,z), GetWorldPosition(x,z) + Vector3.right * .5f, Color.red, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z) => new Vector3(x, 0, z) * cellSize;

    public GridPosition GetPosition(Vector3 worldPosition) =>
        new(Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize));
}
