using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveBoat : MonoBehaviour {

    public GameObject targetUISprite;

    public Tilemap collisionMap;

    int xCoordinate = 0;
    int yCoordinate = 0;

    bool move = false;
    Vector3 movement;

    Vector3Int uiPosition;

    private void Start()
    {
        uiPosition = new Vector3Int(0, 0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && !collisionMap.HasTile(uiPosition + (new Vector3Int(2,0,0))))
        {
            xCoordinate++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !collisionMap.HasTile(uiPosition + (new Vector3Int(-2, 0, 0))))
        {
            xCoordinate--;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !collisionMap.HasTile(uiPosition + (new Vector3Int(0, 2, 0))))
        {
            yCoordinate++;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !collisionMap.HasTile(uiPosition + (new Vector3Int(0, -2, 0))))
        {
            yCoordinate--;
        }

        uiPosition = new Vector3Int(xCoordinate*2, yCoordinate*2, 0);

        if (!collisionMap.HasTile(uiPosition))
            targetUISprite.transform.position = new Vector2(xCoordinate,yCoordinate);
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
