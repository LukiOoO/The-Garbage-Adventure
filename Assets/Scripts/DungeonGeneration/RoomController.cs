using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;

    public int X;

    public int Y;

}


public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    Room currRoom;

    bool spawnedBossRoom = false;
    bool updatedRooms = false;
    public GameObject bossPrefab;

    void Awake()
    {
        instance = this;
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y) == true)
        {
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;
        loadRoomQueue.Enqueue(newRoomData);

    }


    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;
        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }

    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]
        {
            "Empty",
            "Basic1"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }



    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame


    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }
        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms(); 
                updatedRooms = true;
            }
            return;
        }
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }
    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0 && loadedRooms.Count > 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            int bossRoomX = bossRoom.X;
            int bossRoomY = bossRoom.Y;

            Destroy(bossRoom.gameObject);
            loadedRooms.Remove(bossRoom);
            LoadRoom("End", bossRoomX, bossRoomY);

            yield return new WaitUntil(() => DoesRoomExist(bossRoomX, bossRoomY));
            Room loadedBossRoom = FindRoom(bossRoomX, bossRoomY);

            Vector3 fixedBossPosition = loadedBossRoom.GetRoomCentre();
            GameObject boss = Instantiate(bossPrefab, fixedBossPosition, Quaternion.identity);
            boss.transform.SetParent(loadedBossRoom.transform);

            EnemyController bossEnemyController = boss.GetComponent<EnemyController>();
            if (bossEnemyController != null)
            {
                bossEnemyController.notInRoom = true;
            }
                
        }
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

   public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            if (currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies != null)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                        ///Debug.Log("Not in room");
                    }
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                        Debug.Log("Not in room");
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false); // Make sure doors are closed when enemies are present
                    }
                }
            }
        }
    }
}
    

