using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {
    SpriteRenderer sprite;
    float hue = 0;
    public float rainbowSpeed;
	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //RAINBOWWWW
        hue += rainbowSpeed;
        hue %= 1;
        sprite.color = Color.HSVToRGB(hue, 1, 1);

        //check for collision with a player. If so, win.
        Collider2D[] obstacles = Physics2D.OverlapBoxAll(transform.position, new Vector2(.9f, .9f), 0);

        foreach(Collider2D coll in obstacles)
        {
            PlayerBehavior player = coll.GetComponent<PlayerBehavior>();

            if(player != null)
            {
                Win();
            }
        }
    }

    void Win()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
