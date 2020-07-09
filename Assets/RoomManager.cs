using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    public int dungeonSize;
    public Vector2 roomSize;
    Room[] layout;
	// Use this for initialization
	void Start () {
        layout = new Room[dungeonSize];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
