using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoat : MonoBehaviour {

    int xCoordinate = 0;
    int yCoordinate = 0;

    bool move = false;
    Vector3 movement;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xCoordinate++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xCoordinate--;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            yCoordinate++;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            yCoordinate--;
        }
    }

    private void FixedUpdate()
    {
        movement = Vector3.zero;

        if (transform.position.x < xCoordinate)
        {
            movement += new Vector3(0.125f, 0f, 0f);
        }

        if (transform.position.x > xCoordinate)
        {
            movement += new Vector3(-0.125f, 0f, 0f);
        }

        if (transform.position.y < yCoordinate)
        {
            movement += new Vector3(0f, 0.125f, 0f);
        }

        if (transform.position.y > yCoordinate)
        {
            movement += new Vector3(0f, -0.125f, 0f);
        }

        transform.position += movement;
    }    
}
