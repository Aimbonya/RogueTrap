using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health;

    private static int EXP;

    private static int currentGameEXP = 0;

    private static int money = 0;

    private static int killedEnemiesAmount = 0;

    private static bool bossIsKilled = false;

    private static float maxHealth;

    private static float moveSpeed;

    private static float fireRate;

    private static float bulletSize=3f;

    public TextMeshProUGUI healthtext;
    public TextMeshProUGUI EXPtext;
    public TextMeshProUGUI coinText;
    public Button buttonEnd;

    public bool playerIsInShop;

    public bool playerIsInvicible = false;

    public static float Health { get => health; set => health = value; }

    public static int Money { get => money; set => money = value; }

    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public static float FireRate { get => fireRate; set => fireRate = value; }

    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        PlayerPrefs.SetInt("TotalKills", 0);
        PlayerPrefs.SetInt("GainedEXP", 0);
        PlayerPrefs.SetString("BossKilled", "NO");

        currentGameEXP = 0;
        killedEnemiesAmount = 0;
        bossIsKilled = false;
        money = 0;
        EXP = PlayerPrefs.GetInt("PlayerEXP", EXP);
        int charNum = PlayerPrefs.GetInt("ChosenChar");

        fireRate = PlayerPrefs.GetFloat("PlayerFireRate", fireRate);
        moveSpeed = PlayerPrefs.GetFloat("PlayerMVSpeed", moveSpeed);
        maxHealth = PlayerPrefs.GetFloat("PlayerHP", maxHealth);

        SetChar(charNum);

        health = maxHealth;
        instance.buttonEnd.gameObject.SetActive(false);
    }

    void Update()
    {
        if(RoomController.instance.IsRoomShop()) playerIsInShop = true;
        if(!RoomController.instance.IsRoomShop()) playerIsInShop = false;
        healthtext.text = "Health: " + health;
        coinText.text = money.ToString();
        EXPtext.text = "EXP:" + currentGameEXP.ToString();
    }


    private void SetChar(int charNum)
    {
        
        if (charNum == 1)
        {
            int charLevel = PlayerPrefs.GetInt("Player1Level");
            switch (charLevel)
            {
                case 1:
                    maxHealth -= 2;
                    moveSpeed -= 3;
                    fireRate += 0.2f;
                    break;
                case 2:
                    moveSpeed -= 2;
                    maxHealth -= 1;
                    fireRate += 0.2f;

                    break;
                case 3:
                    moveSpeed -= 1;
                    maxHealth -= 1;
                    fireRate += 0.2f;

                    break;
                case 4:
                    moveSpeed -= 0;
                    maxHealth -= 0;
                    fireRate += 0.1f;
                    break;
                case 5:
                    moveSpeed += 1;
                    maxHealth -= 0;
                    fireRate += 0;
                    break;
                case 6:
                    moveSpeed += 2;
                    maxHealth += 1;
                    fireRate -= 0.1f;
                    break;
            }   
        }
        else if(charNum == 2)
        {
            int charLevel = PlayerPrefs.GetInt("Player2Level");
            switch (charLevel)
            {
                case 1:
                    moveSpeed -= 4;
                    maxHealth -= 1;
                    fireRate += 0.3f;
                    break;
                case 2:
                    moveSpeed -= 3;
                    maxHealth -= 0;
                    fireRate += 0.2f;

                    break;
                case 3:
                    moveSpeed -= 2;
                    maxHealth -= 0;
                    fireRate += 0.1f;

                    break;
                case 4:
                    moveSpeed -= 2;
                    maxHealth += 1;
                    fireRate += 0.1f;
                    break;
                case 5:
                    moveSpeed -= 1;
                    maxHealth -= 0;
                    fireRate += 0;
                    break;
                case 6:
                    moveSpeed += 0;
                    maxHealth += 2;
                    fireRate += 0;
                    break;
            }
        }
    }


    public static void DamagePlayer(int damage)
    {
        if (instance.playerIsInvicible == false)
        {
            health -= damage;
            if (health <= 0) KillPlayer();
        }
    }

    public static void HealPlayer(float Heal)
    {
        health += Heal;
        if (health > maxHealth) health = maxHealth;
    }

    public static void SpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void bulletsizeChange(float size)
    {
        bulletSize += size;
    }

    public static void atkSpeedchange(float speed)
    {
        FireRate -= speed;
    }

    public static void ChangeMoney(int num)
    {
        money+= num;
    }

    public static void IncreaseMoney()
    {
        money++;
    }

    public static void IncreaseMoney(int num)
    {
        money+=num;
    }

    public static void MakeEndButtonVisible()
    {
        instance.buttonEnd.gameObject.SetActive(true);
    }

    public static void IncreaseEXP(int num)
    {
        currentGameEXP += num;
    }

    public static void FreezePlayer()
    {
        var preFireRate = fireRate;
        fireRate = 5;
        var preMoveSpeed = moveSpeed;
        moveSpeed= 0;
        instance.StartCoroutine(instance.FreezePlayerCoroutine(preFireRate,preMoveSpeed));
    }

    public IEnumerator FreezePlayerCoroutine(float preFireRate, float premvspd)
    {
        yield return new WaitForSeconds(0.7f);
        UnFreezePlayer(preFireRate, premvspd);
    }

    private void UnFreezePlayer(float preFireRate, float premvspd)
    {
        fireRate = preFireRate;
        moveSpeed = premvspd;
    }

    private static void SaveEXP()
    {
        PlayerPrefs.SetInt("PlayerEXP", EXP);
    }

    public static bool PlayerIsInvicible()
    {
        return instance.playerIsInvicible;
    }
    
    public static void setInvicible()
    {
        instance.playerIsInvicible = true;
    }

    public static void resetInvicible()
    {
        instance.playerIsInvicible = false;
    }

    public static void IncreaseKilledAmount()
    {
        killedEnemiesAmount++;
    }

    public static void SetBossKilled()
    {
        bossIsKilled = true;
    }

    public static void PlayerWon()
    {
        EXP += currentGameEXP;
        SaveEXP();
        instance.StartCoroutine(instance.EndGameCoroutine());
    }

    private static void KillPlayer()
    {
        currentGameEXP /= 5;
        EXP += currentGameEXP;
        SaveEXP();
        GameObject.FindGameObjectWithTag("Player").GetComponent<CONTROLplayer>().Die();
        instance.StartCoroutine(instance.EndGameCoroutine());
    }

    public IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(2f);
        LoadEndScene();
    }

    private void LoadEndScene()
    {
        PlayerPrefs.SetString("BossKilled", bossIsKilled ? "Yes" : "NO");
        PlayerPrefs.SetInt("TotalKills", killedEnemiesAmount/2);
        PlayerPrefs.SetInt("GainedEXP", currentGameEXP);
        SceneManager.LoadSceneAsync("EndScreen");
    }

}
