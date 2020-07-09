using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string name;
    public int charges;
    bool isInInventory;
    // Start is called before the first frame update
    void Start()
    {
        isInInventory = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] obstacles = Physics2D.OverlapBoxAll(transform.position, new Vector2(.9f, .9f), 0);

        foreach (Collider2D coll in obstacles)
        {
            PlayerBehavior player = coll.GetComponent<PlayerBehavior>();

            if (player != null)
            {
                player.Obtain(this);
            }
        }
    }

    public abstract void Use(PlayerBehavior player);
    
}
