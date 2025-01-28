using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoor : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom,
    }

    public DoorType doorType;
}
