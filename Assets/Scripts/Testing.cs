using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    
    
    void Start() {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> pathfindingList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);
            Debug.Log(pathfindingList);
            
            for (int i = 0; i < pathfindingList.Count-1; i++) {
                Debug.Log(pathfindingList[i]);
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(pathfindingList[i]),
                    LevelGrid.Instance.GetWorldPosition(pathfindingList[i+1]),
                    Color.white,
                    5f);
            }
            Debug.Log(pathfindingList[^1]);
        }
    }
}
