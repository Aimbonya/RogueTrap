using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BossController : MonoBehaviour
{

    public Animator meleeAttackAnim;
    public Animator takeDamageAnim;

    public enum RageState
    {
        Raged,
        NonRaged
    }

    public enum BossStates
    {
        CoolDown,
        Sleep,
        Attack,
        Dead
    }

    public BossStates state= BossStates.Sleep;
    public RageState rState;

    GameObject player;

    public GameObject normalBulletPrefab;
    public GameObject frozingBulletPrefab;
    public GameObject wavePrefab;

    public int bossHP;

    public float bossCoolDown;

    private bool ATKCoolDown = false;

    public bool notInBossRoom=true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = BossStates.Sleep;
        rState = RageState.NonRaged;
        notInBossRoom = true;
    }

    private void Update()
    {
        if (notInBossRoom && state != BossStates.Dead)
        {
            state = BossStates.Sleep;
        }
        else if (!notInBossRoom)
        {
            if (state != BossStates.Dead)
            {
                state = BossStates.Attack;
            }
        }
        switch (state)
        {
            case BossStates.Sleep:
                Sleep();
                break;
            case BossStates.Dead:
                Die();
                break;
            case BossStates.CoolDown:
                CoolDown();
                break;
            case BossStates.Attack:
                Attack();
                break;
        }
    }

    private void Sleep()
    {

    }

    private void Attack()
    {
        if (!ATKCoolDown)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 3f)
            {
                MeleeAttack();
            }
            else
            {
                int rand = Random.Range(0, 4);
                switch (rand)
                {
                    default:
                    case 0:
                        //Boss Attack NxNxR(F)
                        InstantiateNormalBullet(0);
                        InstantiateNormalBullet(-30);
                        InstantiateFrozingBullet((float)Random.Range(-50, 50));
                        StartCoroutine(BossCoolDown());
                        break;
                    case 1:
                        //BossAttack R(N)xFxF
                        InstantiateFrozingBullet(0);
                        InstantiateNormalBullet((float)Random.Range(-50, 50));
                        InstantiateFrozingBullet(30);
                        StartCoroutine(BossCoolDown());
                        break;
                    case 2:
                        //Boss Attack FxNxF=>N
                        InstantiateFrozingBullet(30);
                        InstantiateNormalBullet(0);
                        InstantiateFrozingBullet(-30);
                        StartCoroutine(BossAttackMarginCase2());
                        StartCoroutine(BossCoolDown());
                        break;
                    case 3:
                        //Boss Attack NxNxR(F)=>FxNxF
                        InstantiateNormalBullet(0);
                        InstantiateNormalBullet(-30);
                        InstantiateFrozingBullet((float)Random.Range(-50, 50));
                        StartCoroutine(Case3Coroutine());
                        StartCoroutine(BossCoolDown());
                        break;
                }
            }
        }
    }

    private IEnumerator Case3Coroutine() 
    {
        yield return new WaitForSeconds(0.7f);
        InstantiateNormalBullet(0);
        InstantiateFrozingBullet(15);
        InstantiateFrozingBullet(-15);
    }


    private IEnumerator BossAttackMarginCase2()
    {
        yield return new WaitForSeconds(0.5f);
        InstantiateNormalBullet(0);
    }

    private void MeleeAttack()
    {
        StartCoroutine(MeleeAttackCoroutine());
        StartCoroutine(BossCoolDown());
    }


    private IEnumerator MeleeAttackCoroutine()
    {
        meleeAttackAnim.enabled = true;
        meleeAttackAnim.Play("BossWaveAttackAnim");
        yield return new WaitForSeconds(2.01f);
        meleeAttackAnim.enabled = false;
        if (Vector3.Distance(transform.position, player.transform.position) < 3f) GameController.DamagePlayer(2);
    }

    private void Die()
    {
        GameController.IncreaseEXP(200);
        GameController.IncreaseMoney(8);
        GameController.MakeEndButtonVisible();
        GameController.SetBossKilled();
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());

        Destroy(gameObject);
    }

    private void CoolDown()
    {

    }

    public IEnumerator TakeDamageAnimation()
    {
        takeDamageAnim.enabled = true;
        takeDamageAnim.Play("BossTakeDamageAnim");
        yield return new WaitForSeconds(0.26666666666666f);
        takeDamageAnim.enabled = false; 
    }

    public void TakeDamage(int damage)
    {
        bossHP -= damage;
        StartCoroutine(TakeDamageAnimation());
        if (bossHP <= 0) Die();
    }

    private IEnumerator BossCoolDown()
    {
        ATKCoolDown = true;
        state=BossStates.CoolDown;
        yield return new WaitForSeconds(3);
        ATKCoolDown=false;
        ChangeState(BossStates.Attack);
    }

    public void ChangeState(BossStates state1)
    {
        state = state1;
    }

    private void InstantiateBullet(GameObject bulletPrefab, bool isBossBullet, float angleOffset)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<BulletController>().GetPlayer(player.transform);
        Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        bullet.GetComponent<BulletController>().isBossBullet = isBossBullet;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 rotatedDirection = RotateVector(direction, angleOffset);

        float bulletSpeed = 6f;
        rb.velocity = rotatedDirection * bulletSpeed;
    }

    private Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    private void InstantiateNormalBullet(float angleOffset)
    {
        InstantiateBullet(normalBulletPrefab, true, angleOffset);
    }

    private void InstantiateFrozingBullet(float angleOffset)
    {
        InstantiateBullet(frozingBulletPrefab, true, angleOffset);
    }

}