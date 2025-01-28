using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState{
    Wander, 

    Follow, 

    Die, 

    Attack,

    Idle
}

public enum EnemyType
{
    Melee,

    Ranged
}

public class EnemyController : MonoBehaviour
{
    GameObject player;

    public GameObject Coin;

    public GameObject bulletPrefab;

    public EnemyState state = EnemyState.Idle;

    public EnemyType type;

    public int enemyHP;

    public float enemyRange;

    public float enemyspeed;

    private bool ChooseDirection=false;

    public float coolDown;

    public float attackingRange = 0.5f;

    private bool attackCooldown=false;

    public bool notInRoom = false;


    private Vector3 randomDirection;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

   
    void Update()
    {
        
         if (state==EnemyState.Wander)
        {
            Wander();
            
        }
        else if( state==EnemyState.Die)
        {
            Die();
            
        }
        else if (state == EnemyState.Follow)
        {
            Follow();
           
        }
        else if (state == EnemyState.Attack)
        {
            Attack();
        }
        else if(state==EnemyState.Idle) 
        {
            Idle();
        }
        if (player == null)
        {
            state = EnemyState.Idle;
        }
        else {
            if (!notInRoom)
            {
                if (IsPlayerDetected(enemyRange) && state != EnemyState.Die)
                {
                    if (type == EnemyType.Melee)
                    {
                        state = EnemyState.Follow;
                    }
                    else if (type == EnemyType.Ranged)
                    {
                        state = EnemyState.Attack;
                    }
                }

                else if (!IsPlayerDetected(enemyRange) && state != EnemyState.Die)
                {
                    state = EnemyState.Wander;
                }
                if (Vector3.Distance(transform.position, player.transform.position) < attackingRange)
                {
                    state = EnemyState.Attack;
                }
            }
            else state = EnemyState.Idle; }
    }

    private bool IsPlayerDetected(float range)
    {
        if(player != null)return Vector3.Distance(transform.position, player.transform.position) < range;
        return false;
    }

    private IEnumerator chooseDirection()
    {
        ChooseDirection=true; 
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRot = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRot, Random.Range(0.7f,2f));
        ChooseDirection = false;
    }

    void Wander()
    {
        if (!ChooseDirection)
        {
            StartCoroutine(chooseDirection());
        }
        transform.position += (-transform.right * enemyspeed * Time.deltaTime);

        if(IsPlayerDetected(enemyRange))
        {
            state= EnemyState.Follow;
        }
    }

    void Attack()
    {
        if (!attackCooldown)
        {
            if (type == EnemyType.Melee)
            {
                GameController.DamagePlayer(1);
                StartCoroutine(EnemyCoolDown());
            }
            else if(type == EnemyType.Ranged) 
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<BulletController>().isEnemyBullet=true;
                StartCoroutine(EnemyCoolDown());
            }
        }
    }

    void Idle()
    {

    }

    private IEnumerator EnemyCoolDown() {
        attackCooldown = true;
        yield return new WaitForSeconds(1);
        attackCooldown=false;
    }

    void Follow()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyspeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0) Die();
    }

    public void Die()
    {
        var position = transform.position;
        int random = Random.Range(0, 9);
        if(random == 1) 
        {
            SpawnCoin(position);
        }
        GameController.IncreaseKilledAmount();
        GameController.IncreaseEXP(20);
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }

    void SpawnCoin(Vector3 deathPosition)
    {
        Instantiate(Coin, deathPosition, Quaternion.identity);
    }
}
