using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Transform target;

    private bool following = false;

    public void Enable() {
        following = true;
    }

    public void Disable() {
        following = false;
    }

    public void SetOffset(Vector3 newOffset) {
        offset = newOffset;
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
        Debug.Log("Now following " + target.gameObject.name);
    }

    void Update() {
        if (following && target != null) {
            transform.position = target.position + offset;
        }
    }
}
