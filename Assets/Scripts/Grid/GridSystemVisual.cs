using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour {
    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial {
        public GridVisualType GridVisualType;
        public Material Material;
    }
    
    public enum GridVisualType {
        White,
        Blue,
        Red,
        SoftRed,
        Yellow,
    }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    
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

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        
        HideAll();
        UpdateGridVisual();
    }

    private void HideAll() {
        foreach (GridSystemVisualSingle gridSystemVisualSingle in _gridSystemVisualSingleArray) {
            gridSystemVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType) {
        foreach (GridPosition gridPosition in gridPositionList) {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void ShowGridPositionRange(GridPosition centerGridPosition, int range, GridVisualType gridVisualType) {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = -range; x <= range; x++) {
            for (int z = -range; z <= range; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = centerGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) continue;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                gridPositions.Add(testGridPosition);
            }
        }
        
        ShowGridPositionList(gridPositions, gridVisualType);
    }
    
    private void ShowGridPositionRangeRect(GridPosition centerGridPosition, int range, GridVisualType gridVisualType) {
        List<GridPosition> gridPositions = new List<GridPosition>();
        for (int x = -range; x <= range; x++) {
            for (int z = -range; z <= range; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = centerGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                gridPositions.Add(testGridPosition);
            }
        }
        
        ShowGridPositionList(gridPositions, gridVisualType);
    }

    private void UpdateGridVisual() {
        HideAll();

        BaseAction baseAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;
        switch (baseAction) {
            case MoveAction:
            default:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case GrenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition(), 
                    shootAction.GetMaxShootingRange(),
                    GridVisualType.SoftRed);
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRangeRect(UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition(), 
                    swordAction.GetSwordDistance(),
                    GridVisualType.SoftRed);
                break;
            case InteractAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        
        ShowGridPositionList(baseAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) {
        UpdateGridVisual();
    }
    
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType) {
        foreach (GridVisualTypeMaterial item in gridVisualTypeMaterialList) {
            if (item.GridVisualType == gridVisualType) {
                return item.Material;
            }
        }
        Debug.LogError($"The grid visual type hasn't been defined = {gridVisualType}");
        return null;
    }
        
}
