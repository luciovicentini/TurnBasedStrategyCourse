using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GrenadeProjectile : MonoBehaviour {
    public static event EventHandler OnAnyGrenadeExploded; 

    [SerializeField] private int damageAmount = 30;
    [SerializeField] private ParticleSystem grenadeExplodeVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve acrYAnimationCurve;
    
    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviourComplete;
    private float _totalDistance;
    private Vector3 _positionXZ;

    public void Setup(Vector3 targetWorldPosition, Action onGrenadeBehaviourComplete) {
        _targetPosition = targetWorldPosition;
        _onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;

        _positionXZ = transform.position;
        _positionXZ.y = 0f;
        _totalDistance = Vector3.Distance(_targetPosition, _positionXZ);
    }

    private void Update() {
        Vector3 moveDir = (_targetPosition - _positionXZ).normalized;
        float speed = 15f;
        _positionXZ += moveDir * (speed * Time.deltaTime);

        float distance = Vector3.Distance(_targetPosition, _positionXZ);
        float distanceNormalize = 1 - distance / _totalDistance;
        float maxHeight = _totalDistance / 4f;
        float positionY = acrYAnimationCurve.Evaluate(distanceNormalize) * maxHeight;

        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(_targetPosition, transform.position) < reachedTargetDistance) {
            float grenadeDamageRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(_targetPosition, grenadeDamageRadius);
            foreach (Collider collider in colliders) {
                if (collider.TryGetComponent(out Unit unit)) {
                    unit.Damage(damageAmount);
                }
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            
            _onGrenadeBehaviourComplete?.Invoke();
            Instantiate(grenadeExplodeVFXPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
