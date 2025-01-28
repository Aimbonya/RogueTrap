using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONTROLplayer : MonoBehaviour
{
    public Ability abilityPrefab;
    public Ability ability2Prefab;

    public Ability ability;

    public float abilityCoolDownTime;
    public float abilityActiveTime;

    public bool isInvincible = false;

    public enum AbilityState
    {
        Ready,
        Active,
        CoolDown,
    }

    public KeyCode key;

    public AbilityState abilityState= AbilityState.Ready;

    public float speed;

    Rigidbody2D rigidbody;

    public GameObject bulletPrefab;

    public float bulletSpeed;

    private float lastfire;

    public float fireDelay;

    void Start()
    {
        ability = abilityPrefab;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;


        switch (abilityState)
        {
            case AbilityState.Ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject);
                    abilityState = AbilityState.Active;
                    abilityActiveTime = ability.activeTime;
                }
                break;
            case AbilityState.Active:
                if (abilityActiveTime > 0)
                {
                    abilityActiveTime-= Time.deltaTime;
                }
                else
                {
                    abilityState= AbilityState.CoolDown;
                    abilityCoolDownTime = ability.coolDownTime;
                }
                break;
            case AbilityState.CoolDown:
                if (abilityCoolDownTime > 0)
                {
                    abilityCoolDownTime -= Time.deltaTime;
                }
                else
                {
                    abilityState = AbilityState.Ready;
                }
                break;
        }

        float horiz = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float shootHoriz = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        if ((shootHoriz != 0 || shootVertical != 0) && Time.time > lastfire+fireDelay)
        {
            Shoot(shootHoriz, shootVertical);
            lastfire= Time.time;    
        }

        rigidbody.velocity = new Vector3 (horiz* speed,  vertical*speed, 0);
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y-GameController.BulletSize/10), transform.rotation) as GameObject;

        bullet.AddComponent<Rigidbody2D>().gravityScale=0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
            );
        bullet.transform.eulerAngles= new Vector3(x, y, 0);
        
    }

    public void Die()
    {
        Destroy(gameObject);
        LeaveCorpse();
    }

    private void LeaveCorpse()
    {

    }
}
