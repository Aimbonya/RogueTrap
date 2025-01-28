using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]

public class Item
{
    public string Name;
    public string Description;

    public Sprite itemImage;
}

public class COLLECT : MonoBehaviour
{

    public Item item;

    public float HealthChange;

    public float moveSpeedChange;

    public float attackSpeedChange;

    public float bulletSizeChange;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //GetComponent<SpriteRenderer>().sprite = item.itemImage;
        //Destroy(GetComponent<PolygonCollider2D>());
        //gameObject.AddComponent<PolygonCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameController.HealPlayer(HealthChange);
            GameController.SpeedChange(moveSpeedChange);
            GameController.bulletsizeChange(bulletSizeChange);
            GameController.atkSpeedchange(attackSpeedChange);

            Destroy(gameObject);

        }
    }
}
