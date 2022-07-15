using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    [SerializeField] private Transform gridDebugObjectPrefab;
    
    private GridSystem _gridSystem;
    
    void Start() {
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
    
    void Update()
    {
        Debug.Log(_gridSystem.GetPosition(MouseWorld.GetPosition()));    
    }
}
