using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public TextMeshProUGUI exp;

    public TextMeshProUGUI charnum;

    public TextMeshProUGUI charLevel;

    private int player1Level;

    private int player2Level;

    private int EXP;

    private int playerNum = 1;

    private void Start()
    {
        if(PlayerPrefs.GetString("FirstTime")!= "NO")
        {
            PlayerPrefs.SetInt("Player1Level", 1);
            PlayerPrefs.SetInt("Player2Level", 1);
            PlayerPrefs.SetInt("PlayerEXP", 0);
            PlayerPrefs.SetString("FirstTime", "NO");
        }
        PlayerPrefs.SetInt("PlayerEXP", 5500);
        PlayerPrefs.SetFloat("PlayerFireRate", 0.5f);
        PlayerPrefs.SetFloat("PlayerMVSpeed", 6);
        PlayerPrefs.SetFloat("PlayerHP", 5);



        player1Level = PlayerPrefs.GetInt("Player1Level", player1Level);
        player2Level = PlayerPrefs.GetInt("Player2Level", player2Level);

        EXP = PlayerPrefs.GetInt("PlayerEXP", EXP);
    }

    private void Update()
    {
        charLevel.text = "Level: " + (playerNum == 1 ? player1Level : player2Level).ToString();
        exp.text = "EXP: "+ EXP.ToString();
        charnum.text= playerNum.ToString();
    }


    public void PLay()
    {
        PlayerPrefs.SetInt("PlayerEXP", EXP);
        PlayerPrefs.SetInt("Player2Level", player2Level);
        PlayerPrefs.SetInt("Player1Level", player1Level);

        PlayerPrefs.SetInt("ChosenChar", playerNum);

        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void IncreasePlayerNum()
    {
        playerNum++;
        if (playerNum > 2) playerNum = 2;
    }

    public void DecreasePlayerNum()
    {
        playerNum--;
        if(playerNum < 1) playerNum = 1;
    }

    public void UpgradeButtonClicked()
    {
        switch (playerNum)
        {
            case 1:
                UpgradeFirst();
                break;
            case 2:
                UpgradeSecond();
                break;
        }
    }

    private void UpgradeFirst()
    {
        int upgradeCost = 2000 + ((player1Level-1) * 1500);

        if (EXP >= upgradeCost && player1Level<7)
        {
            player1Level++;
            EXP -= upgradeCost;
            //PlayerPrefs.SetInt("Player1Level", player1Level);
        }
    }

    private void UpgradeSecond()
    {
        int upgradeCost = 2000 + ((player2Level - 1) * 1500);


        if (EXP >= upgradeCost && player2Level< 7)
        {
            player2Level++;
            EXP -= upgradeCost;
            //PlayerPrefs.SetInt("Player2Level", player1Level);
        }
    }
}
