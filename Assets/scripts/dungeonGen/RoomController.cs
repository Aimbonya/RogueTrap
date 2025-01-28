using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Diagnostics;

public class RoomInfo
{
    public string name;

    public int X;

    public int Y;

}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName= "Upworld";

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> loadRoomQue= new Queue<RoomInfo>();

    Room currRoom;

    public List<Room> loadedRooms= new List<Room>();

    bool isLoadingRoom= false;

    bool spawnedBossRoom = false;

    bool updatedRooms= false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       /* LoadRoom("Start", 0, 0);
        LoadRoom("Empty", 1, 0);
        LoadRoom("Empty", -1, 0);
        LoadRoom("Empty", 0, 1);
        LoadRoom("Empty", 0, -1);
        LoadRoom("End",   2, 0);
        */
    }

    private void Update()
    {
        UpdateRoomQue();
    }

    void UpdateRoomQue()
    {
        if (isLoadingRoom) return;

        if (loadRoomQue.Count == 0) 
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnonnectedDoors();
                }
                foreach(Room room in loadedRooms)
                {
                    room.EnableClosedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return; 
        }
       
        currentLoadRoomData= loadRoomQue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom= true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room temp = new Room(bossRoom.X, bossRoom.Y); 

            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == temp.X && r.Y == temp.Y);

            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", temp.X, temp.Y);
        }
    }

    public void LoadRoom(string roomname, int x, int y)
    {
        if (DoesRoomExists(x,y)) return;
        RoomInfo newRoomData= new RoomInfo();
        newRoomData.name = roomname;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomname= currentWorldName+info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomname, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom (Room room)
    {
        if (!DoesRoomExists(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(currentLoadRoomData.X * room.Width, currentLoadRoomData.Y * room.Height, 0);

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentLoadRoomData.name + "    " + currentWorldName + " " + room.X + " " + room.Y;
            if(currentLoadRoomData.name=="Shop") room.isShop = true;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
            room.RemoveUnonnectedDoors();
        }
        else    
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExists(int x, int y)
    {
        return loadedRooms.Find(item => item.X== x && item.Y == y)!=null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;
        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            bool isCurrentRoom = room == currRoom;
            BossController boss = room.GetComponentInChildren<BossController>();
            EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

            if (enemies.Length > 0)
            {
                foreach (var enemy in enemies)
                {
                    enemy.notInRoom = !isCurrentRoom;
                }
                SetDoorColliders(room, isCurrentRoom);
            }
            else
            {
                SetDoorColliders(room, false);
            }

            if (boss != null)
            {
                boss.notInBossRoom = !isCurrentRoom;
                SetDoorColliders(room, isCurrentRoom);
            }
            else if (enemies.Length == 0)
            {
                SetDoorColliders(room, false);
            }
        }

        void SetDoorColliders(Room room, bool isActive)
        {
            foreach (var door in room.GetComponentsInChildren<SideWalls>())
            {
                door.doorCollider.SetActive(isActive);
            }
        }
    }


        /*foreach (Room room in loadedRooms)
        {
            if (room != currRoom)
            {
                BossController boss = room.GetComponentInChildren<BossController>();
                if( boss != null ) 
                {
                    UnityEngine.Debug.Log("Boss is not null, currRomm!= bossroom");
                    boss.notInBossRoom = true;
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                else
                {
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies!= null)
                {
                    foreach(var enemy in enemies) 
                    {
                        enemy.notInRoom = true;                    
                    }

                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(false);
                    }

                }
                else
                {
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }

            }
            else
            {
                BossController boss = room.GetComponentInChildren<BossController>();
                if( boss != null ) 
                {
                    boss.notInBossRoom = false;
                    UnityEngine.Debug.Log("Boss is not null, currRoom== bossRoom");
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        UnityEngine.Debug.Log("Activating doors");
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }


                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length>0)
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach (var door in room.GetComponentsInChildren<SideWalls>())
                    {
                        door.doorCollider.SetActive(false);
                    }

                }
            }
        }
    }

    */
    public bool IsRoomShop()
    {
        if(currRoom) return currRoom.isShop;
        return false;
    }


}
