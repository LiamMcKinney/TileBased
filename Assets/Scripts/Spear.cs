using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Item
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        name = "Spear";
        charges = 5;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Use(PlayerBehavior player)
    {
        Vector2 direction = player.lastDirectionalInputs;
        Collider2D thing1 = Physics2D.OverlapPoint(new Vector3(direction.x, direction.y, 0) + player.transform.position);
        Collider2D thing2 = Physics2D.OverlapPoint(new Vector3(direction.x*2, direction.y*2, 0) + player.transform.position);
        if (thing1 != null)
        {
            if (thing1.GetComponent<LivingThing>() != null)
            {
                thing1.GetComponent<LivingThing>().Hurt(player.damage);
            }
        }
        if (thing2 != null)
        {
            if (thing2.GetComponent<LivingThing>() != null)
            {
                thing2.GetComponent<LivingThing>().Hurt(player.damage);
            }
        }
    }
}
