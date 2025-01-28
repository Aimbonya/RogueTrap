using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float lifeTime;

    public bool isEnemyBullet = false;

    public bool isBossBullet = false;

    public bool isFrozingBullet= false;  

    private Vector2 lastPosition;

    private Vector2 currentPosition;

    private Vector2 playerPosition;

    void Start()
    {
        StartCoroutine(DeathDelay());

        if(!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPosition= player.position;
    }
    
    void Update()
    {
        if (isEnemyBullet)
        {
            currentPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, 5f * Time.deltaTime);
            if (currentPosition == lastPosition)
            {
                Destroy(gameObject);
            }
            lastPosition = currentPosition;
        } 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy" && !isEnemyBullet)
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (collision.tag == "Boss" && (!isEnemyBullet && !isBossBullet)) 
        {
            collision.gameObject.GetComponent<BossController>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (collision.tag == "Player" && (isEnemyBullet || isBossBullet))
        {
            if (isFrozingBullet) GameController.FreezePlayer();
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
        if(collision.tag == "Wall" || collision.tag == "SwitchableDoors")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}