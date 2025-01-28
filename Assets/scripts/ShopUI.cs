using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject ui;

    private void Start()
    {
        ui = GameObject.Find("UI_Shop");
    }
}
