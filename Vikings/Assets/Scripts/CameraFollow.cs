using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public bool retro;
    public static int width = 5;
    public static int height = 5;

    public int xOffset;
    public int yOffset;

    float bias = 0.5f;

    void FixedUpdate () {

        if (retro)
        {
            if (transform.position.x - target.position.x < -1 * width)
            {
                StartCoroutine(MoveCameraRightCo());
            }

            if (transform.position.x - target.position.x > width)
            {
                StartCoroutine(MoveCameraLeftCo());
            }

            if (transform.position.y - target.position.y < -1 * height)
            {
                StartCoroutine(MoveCameraDownCo());
            }

            if (transform.position.y - target.position.y > height)
            {
                StartCoroutine(MoveCameraUpCo());
            }
        }
        else
        {
            transform.position = (bias)*transform.position + (1f-bias)*(new Vector3(target.position.x + xOffset, target.position.y + yOffset, transform.position.z));
        }

    }

    private IEnumerator MoveCameraRightCo()
    {
        while (transform.position.x - target.position.x < width)
        {
            transform.position += new Vector3(0.125f, 0f);
            yield return null;
        }
    }

    private IEnumerator MoveCameraLeftCo()
    {
        while (transform.position.x - target.position.x > -1*width)
        {
            transform.position -= new Vector3(0.125f, 0f);
            yield return null;
        }
    }

    private IEnumerator MoveCameraDownCo()
    {
        while (transform.position.y - target.position.y < height)
        {
            transform.position += new Vector3(0f, 0.125f);
            yield return null;
        }
    }

    private IEnumerator MoveCameraUpCo()
    {
        while (transform.position.y - target.position.y > -1*height)
        {
            transform.position -= new Vector3(0f, 0.125f);
            yield return null;
        }
    }
}
