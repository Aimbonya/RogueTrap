using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int X;

    public int Y;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    private bool updatedDoors = false;

    public int Width;

    public int Height;

    public Door leftDoor;

    public Door rightDoor;

    public Door topDoor;

    public Door bottomDoor;

    public ClosedDoor leftClDoor;

    public ClosedDoor rightClDoor;

    public ClosedDoor topClDoor;

    public ClosedDoor bottomClDoor;

    public List<Door> doors = new List<Door>();

    public List<ClosedDoor> closedDoors = new List<ClosedDoor>();

    public List<ClosedDoor> bossClosedDoors = new List<ClosedDoor>();

    public bool isShop = false;

    void Start()
    {
        if (RoomController.instance == null)
        {
            //Debug.Log("kekw xd");   
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        ClosedDoor[] clds = GetComponentsInChildren<ClosedDoor>(true);

        foreach(ClosedDoor cd in clds)
        {
            if(cd.doorType== ClosedDoor.DoorType.left)
            {
                leftClDoor = cd;
                closedDoors.Add(cd);
            }
            else if (cd.doorType == ClosedDoor.DoorType.right)
            {
                rightClDoor = cd;
                closedDoors.Add(cd);
            }
            else if (cd.doorType == ClosedDoor.DoorType.top)
            {
                topClDoor = cd;
                closedDoors.Add(cd);
            }
            else if (cd.doorType == ClosedDoor.DoorType.bottom)
            {
                bottomClDoor = cd;
                closedDoors.Add(cd);
            }
        }

        foreach (Door d in ds) 
        {
            if (d.doorType == Door.DoorType.right)
            {
                rightDoor = d;
                doors.Add(rightDoor);
            }
            else if(d.doorType == Door.DoorType.top) 
            {
                topDoor = d;
                doors.Add(topDoor);
            }
            else if(d.doorType == Door.DoorType.bottom)
            {
                bottomDoor = d;
                doors.Add(bottomDoor);
            }
            else if(d.doorType == Door.DoorType.left)
            {
                leftDoor = d;
                doors.Add(leftDoor);
            }
        }

        RoomController.instance.RegisterRoom(this);
    }

    public void GetClosedDoors()
    {
        Debug.Log(closedDoors.Count.ToString()+X.ToString()+Y.ToString());
    }


    private void Update()
    {
        if ( name.Contains("End") && updatedDoors == false)
        {
            RemoveUnonnectedDoors();
            updatedDoors = true;
            EnableClosedDoors();
        }
    }

    public void RemoveUnonnectedDoors()
    {
        foreach (Door d in doors) 
        {
            if (d.doorType == Door.DoorType.right)
            {
                if (GetRight()== null)
                {
                    d.gameObject.SetActive(false);
                }
            }
            else if (d.doorType == Door.DoorType.top)
            {
                if(GetTop()== null)     
                {
                    d.gameObject.SetActive(false); 
                }
            }
            else if (d.doorType == Door.DoorType.bottom)
            {
                if (GetBottom() == null)
                {
                    d.gameObject.SetActive(false);
                }
            }
            else if (d.doorType == Door.DoorType.left)
            {
                if(GetLeft() == null)
                {
                    d.gameObject.SetActive(false);
                }
            }
        }
    }

    public void EnableClosedDoors()
    {
        foreach (ClosedDoor d in closedDoors)
        {
            if (d.doorType == ClosedDoor.DoorType.right)
            {
                if (GetRight() == null)
                {
                    d.gameObject.SetActive(true);
                }
            }
            else if (d.doorType == ClosedDoor.DoorType.top)
            {
                if (GetTop() == null)
                {
                    d.gameObject.SetActive(true);
                }
            }
            else if (d.doorType == ClosedDoor.DoorType.bottom)
            {
                if (GetBottom() == null)
                {
                    d.gameObject.SetActive(true);
                }
            }
            else if (d.doorType == ClosedDoor.DoorType.left)
            {
                if (GetLeft() == null)
                {
                    d.gameObject.SetActive(true);
                }
            }
        }
    }

    public Room GetRight()
    {
        if (RoomController.instance.DoesRoomExists(X + 1, Y)) return RoomController.instance.FindRoom(X + 1, Y);
        else return null;
    }

    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExists(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }

        else return null;
    }
    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomExists(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        else return null;
    }

    public Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExists(X, Y - 1))
        {
            return RoomController.instance.FindRoom(X, Y - 1);
        }
        else return null;
    }


    public Vector3 GetRoomCenter()
    {
        return new Vector3(X*Width, Y*Height);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
}
