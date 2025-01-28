using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom    
    }

    public DoorType doorType;

    private GameObject player;

    private float widthOffset = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(doorType== DoorType.left) 
            {
                player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
            }
            else if (doorType== DoorType.right) 
            {
                player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
            }
            else if (doorType== DoorType.top) 
            {
                player.transform.position = new Vector2(transform.position.x, transform.position.y - widthOffset);
            }
            else if (doorType == DoorType.bottom)
            {
                player.transform.position = new Vector2(transform.position.x, transform.position.y+widthOffset);
            }
        }
    }
}
