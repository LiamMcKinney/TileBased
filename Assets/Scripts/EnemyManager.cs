﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyBehavior> enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(EnemyBehavior e in enemies)
        {
            if (e.isDead)
            {
                enemies.Remove(e);
                break;
            }
        }
    }
    
    public void Step()
    {
        foreach(EnemyBehavior e in enemies)
        {
            e.Move();
        }
    }
}
