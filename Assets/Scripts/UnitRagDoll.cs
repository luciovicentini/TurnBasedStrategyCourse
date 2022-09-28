using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagDoll : MonoBehaviour {
    [SerializeField] private Transform ragDollRootBone;
    
    public void Setup(Transform originalRootBone) {
        MatchAllChildTransforms(originalRootBone, ragDollRootBone);

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ApplyExplosionToRagDoll(ragDollRootBone, 300f, transform.position + randomDir, 10f);
    }

    private void MatchAllChildTransforms(Transform original, Transform clone) {
        foreach (Transform child in original) {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null) {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                
                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagDoll(Transform root, float explosionForce, Vector3 explosionPosition,
        float explosionRadius) {
        foreach (Transform child in root) {
            if (child.TryGetComponent(out Rigidbody childRigidBody)) {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                
                ApplyExplosionToRagDoll(child, explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}
