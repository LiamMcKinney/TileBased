using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingThing : MonoBehaviour
{
    public int hp;
    public bool isDead;
    public int money;
    // Start is called before the first frame update
    protected void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    public void Hurt(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            isDead = true;
        }
    }

    public void collect(int amount)
    {
        money += amount;
    }
}
