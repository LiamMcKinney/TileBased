using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour {
    public int dungeonSize;
    public Vector2 roomSize;//dimensions of a room
    Dictionary<Vector2, Room> layout;

    public GameObject roomPrefab;
    public GameObject hallwayPrefab;
    public float roomSpacingScale;
    public float hallwayOffset;

    public Tilemap grid;
    public Tile wallTile;

	// Use this for initialization
	void Start () {
        layout = new Dictionary<Vector2, Room>();
        GenerateDungeonPlan();

        GenerateDungeon();
    }

    void GenerateDungeonPlan()
    {
        layout.Add(Vector2.zero, new Room(Vector2.zero));

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
            }

            nextRoom.openExits.Remove(exitDir);
            layout[nextRoom.position + exitDir].openExits.Remove(-exitDir);
        }
    }

    void GenerateDungeon()
    {
        foreach(Vector2 key in layout.Keys)
        {
            //Instantiate(roomPrefab, key * roomSpacingScale, Quaternion.identity);

            //List<Vector2> usedExits = new List<Vector2>(new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right });
            /*foreach(Vector2 exit in layout[key].openExits)
            {
                usedExits.Remove(exit);
            }*/

            /*foreach(Vector2 exit in usedExits)
            {
                Instantiate(hallwayPrefab, (key * roomSpacingScale) + (exit * hallwayOffset), Quaternion.identity);
            }*/
            CreateRoom(key, layout[key].openExits);
        }
    }

    void CreateRoom(Vector2 position, List<Vector2> walledExits)
    {
        Vector2 baseCorner = new Vector2(position.x * roomSize.x, position.y * roomSize.y);//coordinates of the bottom left corner of the room.
        Vector2 exitCoords = new Vector2(baseCorner.x + (int)(roomSize.x / 2), baseCorner.y + (int)(roomSize.y / 2));

        for(int i = (int)baseCorner.x; i < baseCorner.x + roomSize.x; i++)
        {
            grid.SetTile(new Vector3Int(i, (int)baseCorner.y, 0), (i==exitCoords.x && !walledExits.Contains(Vector2.down)) ? null : wallTile);
            grid.SetTile(new Vector3Int(i, (int)(baseCorner.y + roomSize.y) - 1, 0), (i == exitCoords.x && !walledExits.Contains(Vector2.up)) ? null : wallTile);
        }

        for (int i = (int)baseCorner.y; i < baseCorner.y + roomSize.y; i++)
        {
            grid.SetTile(new Vector3Int((int)baseCorner.x, i, 0), (i == exitCoords.y && !walledExits.Contains(Vector2.left)) ? null : wallTile);
            grid.SetTile(new Vector3Int((int)(baseCorner.x + roomSize.x) - 1, i, 0), (i == exitCoords.y && !walledExits.Contains(Vector2.right)) ? null : wallTile);
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
}
