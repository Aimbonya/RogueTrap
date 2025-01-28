using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCosts : MonoBehaviour
{
    public enum ItemType
    {
        ATKspeed,
        HealPoition,
        BulletSize,
        MoveSpeed
    }

    public static int GetCost(ItemType type)
    {
        switch(type) 
        {
            default:
            case ItemType.ATKspeed: return 4;
            case ItemType.HealPoition: return 2;
            case ItemType.BulletSize: return 3;
            case ItemType.MoveSpeed: return 3;
            
        }
    }
}
