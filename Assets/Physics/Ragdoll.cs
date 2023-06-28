using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;

    private void Start() {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        SetRagdoll(false);
    }

    public void SetRagdoll(bool active) {
        foreach (Collider collider in allColliders) {
            if (collider.gameObject.CompareTag("Ragdoll")) {
                collider.enabled = active;
            }
        }

        foreach (Rigidbody rigidbody in allRigidbodies) {
            if (rigidbody.gameObject.CompareTag("Ragdoll")) {
                rigidbody.isKinematic = !active;
                rigidbody.useGravity = active;
            }
        }

        controller.enabled = !active;
        animator.enabled = !active;
    }
}
