using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagDollSpawner : MonoBehaviour {
    [SerializeField] private Transform ragDollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem _healthSystem;

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        Transform ragDollTransform = Instantiate(ragDollPrefab, transform.position, transform.rotation);
        ragDollTransform.GetComponent<UnitRagDoll>().Setup(originalRootBone);
    }
}
