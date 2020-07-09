using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        Vector3 oldPos = transform.position;
        float horiz;
        float vertic;
        if(oldPos.x > target.position.x)
        {
            horiz = -1;
        }
        else if (oldPos.x == target.position.x)
        {
            horiz = 0;
        }
        else
        {
            horiz = 1;
        }
        if(oldPos.y > target.position.y)
        {
            vertic = -1;
        }
        else if(oldPos.y == target.position.y)
        {
            vertic = 0;
        }
        else
        {
            vertic = 1;
        }
        transform.position = new Vector3(horiz, vertic, 0)+oldPos;
    }
}
