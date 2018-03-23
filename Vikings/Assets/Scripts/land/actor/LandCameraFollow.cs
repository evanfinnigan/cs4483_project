using UnityEngine;
using System.Collections;

public class LandCameraFollow : MonoBehaviour {

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private GameObject follow;

    // Use this for initialization
    void Start() {
        //offset = transform.position - follow.transform.position;
        if(follow == null) {
            Debug.LogError("LandCameraFollow behaviour missing fields on " + name);
        }
    }

    // Update is called once per frame
    void Update() {
        if(follow != null) {
            transform.position = follow.transform.position + offset;
        }
    }
}
