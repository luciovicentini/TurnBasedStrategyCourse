using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {
    [SerializeField] private int _damageAmount = 30;
    
    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviourComplete;

    public void Setup(Vector3 targetWorldPosition, Action onGrenadeBehaviourComplete) {
        _targetPosition = targetWorldPosition;
        _onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
    }

    private void Update() {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        float speed = 15f;
        transform.position += moveDir * (speed * Time.deltaTime);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(_targetPosition, transform.position) < reachedTargetDistance) {
            float grenadeDamageRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(_targetPosition, grenadeDamageRadius);
            foreach (Collider collider in colliders) {
                if (collider.TryGetComponent(out Unit unit)) {
                    unit.Damage(_damageAmount);
                }
            }
            _onGrenadeBehaviourComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
