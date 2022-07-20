using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour {
    public static GridSystemVisual Instance { get; private set; }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one GridSystemVisual Instance! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Start() {
        int levelHeight = LevelGrid.Instance.GetHeight();
        int levelWidth = LevelGrid.Instance.GetWidth();
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[levelWidth, levelHeight];

        for (int x = 0; x < levelWidth; x++) {
            for (int z = 0; z < levelHeight; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab,
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                _gridSystemVisualSingleArray[x, z] =
                    gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        HideAll();
    }

    private void Update() {
        UpdateGridVisual();
    }

    public void HideAll() {
        foreach (GridSystemVisualSingle gridSystemVisualSingle in _gridSystemVisualSingleArray) {
            gridSystemVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList) {
        foreach (GridPosition gridPosition in gridPositionList) {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual() {
        HideAll();

        Unit unitSelected = UnitActionSystem.Instance.GetSelectedUnit();
        ShowGridPositionList(unitSelected.GetMoveAction().GetValidActionGridPositionList());
    }
}
