using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : LivingThing
{
    public LivingThing target;
    Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = target.transform.position;
    }

    public void Move()
    {
        Vector3 oldPos = transform.position;
        float horiz;
        float vertic;
        if(oldPos.x > targetPos.x)
        {
            horiz = -1;
        }
        else if (oldPos.x == targetPos.x)
        {
            horiz = 0;
        }
        else
        {
            horiz = 1;
        }
        if(oldPos.y > targetPos.y)
        {
            vertic = -1;
        }
        else if(oldPos.y == targetPos.y)
        {
            vertic = 0;
        }
        else
        {
            vertic = 1;
        }
        transform.position = new Vector3(horiz, vertic, 0)+oldPos;
        if(transform.position == targetPos)
        {
            target.Hurt(1);
        }
    }
}
