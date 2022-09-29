using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour {
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;
    
    private GridPosition _gridPosition;

    private void Start() {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage() {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, Quaternion.identity);
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(RemoveDestroyedCrate(crateDestroyedTransform));
    }

    private IEnumerator RemoveDestroyedCrate(Transform crateDestroyedTransform) {
        float elapsedTime = 0f;
        float removeCrateTimer = 2f;
        
        while (elapsedTime <= removeCrateTimer) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // yield return new WaitForSeconds(removeCrateTimer);
        
        Destroy(crateDestroyedTransform.gameObject);
        Destroy(gameObject);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition,
        float explosionRadius) {
        foreach (Transform child in root) {
            if (child.TryGetComponent(out Rigidbody childRigidBody)) {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                
                ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRadius);
            }
        }
    }

    public GridPosition GetGridPosition() => _gridPosition;
}
