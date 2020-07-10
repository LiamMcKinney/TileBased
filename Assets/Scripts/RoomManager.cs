using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {
    public float dungeonSize;
    public float sizeIncreasePerFloor;
    public Vector2 roomSize;//dimensions of a room
    Dictionary<Vector2, Room> layout;

    public Tilemap grid;
    public Tile wallTile;
    public Tile groundTile;

    public EnemyManager enemyManager;

    //values to control how many enemies spawn in each room.
    public float initialDifficulty;
    public float difficultyScaling;
    public float difficultyIncreasePerFloor;

    Vector2 lastRoom;//coordinates of the last room generated

    public GameObject enemyPrefab;
    //public GameObject playerPrefab;
    public GameObject exitPrefab;
    public GameObject goldPrefab;
    public CameraManager cam;

    public PlayerBehavior player;

    public Canvas ui;
    public Slider hpBar;
    public Text floorText;
    public Text goldText;
    public Text totalGoldText;
    int floorNumber = 0;

    private static RoomManager instance;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance.grid = grid;
            

            instance.GenerateMap();

            Destroy(enemyManager.gameObject);
            Destroy(player.gameObject);
            Destroy(cam.gameObject);
            Destroy(ui.gameObject);
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ui.gameObject);
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(enemyManager.gameObject);
        DontDestroyOnLoad(cam);

        GenerateMap();
    }

    void GenerateMap()
    {
        floorNumber++;
        floorText.text = "Floor " + floorNumber;

        dungeonSize += sizeIncreasePerFloor;

        initialDifficulty += difficultyIncreasePerFloor;

        layout = new Dictionary<Vector2, Room>();
        enemyManager.enemies.Clear();

        GenerateDungeonPlan();

        GenerateDungeon();
    }

    void GenerateDungeonPlan()
    {
        //generate first room
        layout.Add(Vector2.zero, new Room(Vector2.zero));

        //generate and initialize player in the middle of the room
        //player = Instantiate(playerPrefab, new Vector2((int)(roomSize.x / 2) + .5f, (int)(roomSize.y / 2) + .5f), Quaternion.identity).GetComponent<PlayerBehavior>();
        player.transform.position = new Vector2((int)(roomSize.x / 2) + .5f, (int)(roomSize.y / 2) + .5f);
        player.enemyManager = enemyManager;
        player.camera = cam;


        while (layout.Count < dungeonSize)
        {
            //select a random room to add an exit to.
            Room[] rooms = new Room[layout.Count];
            layout.Values.CopyTo(rooms, 0);
            Room nextRoom = rooms[Random.Range(0, rooms.Length)];

            //if there are no open exits in that room, reset the loop
            if (nextRoom.openExits.Count == 0)
            {
                continue;
            }

            //choose a random available exit direction, get/make the room there, and remove that exit as an option.
            Vector2 exitDir = nextRoom.openExits[Random.Range(0, nextRoom.openExits.Count)];
            
            if(!layout.ContainsKey(nextRoom.position + exitDir))
            {
                layout.Add(nextRoom.position + exitDir, new Room(nextRoom.position + exitDir));
                lastRoom = nextRoom.position + exitDir;
            }

            nextRoom.openExits.Remove(exitDir);
            layout[nextRoom.position + exitDir].openExits.Remove(-exitDir);
        }
    }

    void GenerateDungeon()
    {
        foreach(Vector2 key in layout.Keys)
        {
            CreateRoom(key, layout[key].openExits, key.Equals(lastRoom));
        }
    }

    void CreateRoom(Vector2 position, List<Vector2> walledExits, bool hasExit)
    {
        Vector2 baseCorner = new Vector2(position.x * roomSize.x, position.y * roomSize.y);//coordinates of the bottom left corner of the room.
        Vector2 exitCoords = new Vector2(baseCorner.x + (int)(roomSize.x / 2), baseCorner.y + (int)(roomSize.y / 2));

        for(int i = (int)baseCorner.x; i < baseCorner.x + roomSize.x; i++)
        {

            for (int j = (int)baseCorner.y; j < baseCorner.y + roomSize.y; j++)
            {
                grid.SetTile(new Vector3Int(i, j, 0), groundTile);
                if (i == baseCorner.x + roomSize.x - 1)
                {
                    grid.SetTile(new Vector3Int((int)baseCorner.x, j, 0), (j == exitCoords.y && !walledExits.Contains(Vector2.left)) ? groundTile : wallTile);
                    grid.SetTile(new Vector3Int((int)(baseCorner.x + roomSize.x) - 1, j, 0), (j == exitCoords.y && !walledExits.Contains(Vector2.right)) ? groundTile : wallTile);
                }
            }

            grid.SetTile(new Vector3Int(i, (int)baseCorner.y, 0), (i == exitCoords.x && !walledExits.Contains(Vector2.down)) ? groundTile : wallTile);
            grid.SetTile(new Vector3Int(i, (int)(baseCorner.y + roomSize.y) - 1, 0), (i == exitCoords.x && !walledExits.Contains(Vector2.up)) ? groundTile : wallTile);
        }

        //don't spawn enemies in the starting room.
        if (position != Vector2.zero)
        {
            //calculate the number of enemies to spawn in the room based on distance from the start.
            int numEnemies = (int)(initialDifficulty + position.magnitude * difficultyScaling);

            for (int i = 0; i < numEnemies; i++)
            {
                //generate the enemy at a random position in the room.
                Vector2 enemyPosition = baseCorner + new Vector2(Random.Range(1, (int)roomSize.x - 2), Random.Range(1, (int)roomSize.y - 2)) + new Vector2(.5f, .5f);

                EnemyBehavior enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity).GetComponent<EnemyBehavior>();
                enemy.target = player;
                //enemy.isDead = true;
                enemyManager.enemies.Add(enemy);
            }
        }

        //if it's a dead end, add a coin as a reward.
        if(walledExits.Count == 3)
        {
            Vector2 goldPosition = baseCorner + new Vector2(Random.Range(1, (int)roomSize.x - 2), Random.Range(1, (int)roomSize.y - 2)) + new Vector2(.5f, .5f);

            Instantiate(goldPrefab, goldPosition, Quaternion.identity);
        }

        if (hasExit)
        {
            Instantiate(exitPrefab, baseCorner + new Vector2(Random.Range(1, (int)roomSize.x - 2), Random.Range(1, (int)roomSize.y - 2)) + new Vector2(.5f, .5f), Quaternion.identity);
        }
    }

    class Room
    {
        public List<Vector2> openExits = new List<Vector2>(new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right });
        public Vector2 position;

        public Room(Vector2 location)
        {
            position = location;
        }
    }

    private void Update()
    {
        hpBar.value = (float)player.hp / player.maxHP;
        goldText.text = "Gold: " + player.money;
        totalGoldText.text = "Total Gold Collected: " + player.moneyTotal;
    }
}
