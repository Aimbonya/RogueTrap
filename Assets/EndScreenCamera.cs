using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class EndScreenCamera : MonoBehaviour
{

    public TextMeshProUGUI killedAmount;
    public TextMeshProUGUI BossIsKilled;
    public TextMeshProUGUI EXPgained;

    private void Start()
    {
        killedAmount.text= "Total kills: " + PlayerPrefs.GetInt("TotalKills").ToString();
        EXPgained.text = "EXP gained: " + PlayerPrefs.GetInt("GainedEXP").ToString();
        BossIsKilled.text = "Killed Boss? " + PlayerPrefs.GetString("BossKilled");
    }

    public void OKButton()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
    
}
