using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalls : MonoBehaviour
{

    public GameObject doorCollider;

    private GameObject player;

    private float widthOffset = 1.4f;

    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (doorType == DoorType.left)
            {
                player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
            }
            else if (doorType == DoorType.right)
            {
                player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
            }
            else if (doorType == DoorType.top)
            {
                player.transform.position = new Vector2(transform.position.x, transform.position.y - widthOffset);
            }
            else if (doorType == DoorType.bottom)
            {
                player.transform.position = new Vector2(transform.position.x, transform.position.y + widthOffset);
            }
        }
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }
}
