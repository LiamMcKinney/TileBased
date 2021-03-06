﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : LivingThing
{
    public PlayerBehavior target;
    Vector3 targetPos;
    float targetXDist;
    float targetYDist;
    float targetDistSqrd;
    public float sightRange;

    SpriteRenderer sprite;
    public Color deadColor;
    public float despawnTime;//how many frames until the dead enemy despawns.
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        Physics2D.autoSyncTransforms = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDead)
        {
            sprite.color = deadColor;
            sprite.sortingOrder = -2;
            Destroy(gameObject, despawnTime);
        }
    }

    public void Move()
    {
        if (isDead)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        targetPos = target.transform.position;
        targetXDist = targetPos.x - transform.position.x;
        targetYDist = targetPos.y - transform.position.y;
        targetDistSqrd = (targetXDist * targetXDist) + (targetYDist * targetYDist);
        if (targetDistSqrd <= (sightRange * sightRange))
        {
            Vector3 oldPos = transform.position;
            float horiz;
            float vertic;
            if (oldPos.x > targetPos.x)
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
            if (oldPos.y > targetPos.y)
            {
                vertic = -1;
            }
            else if (oldPos.y == targetPos.y)
            {
                vertic = 0;
            }
            else
            {
                vertic = 1;
            }
            Vector3 moveVector = new Vector3(horiz, vertic, 0);
            Collider2D[] obstacles = Physics2D.OverlapBoxAll(new Vector2(horiz + transform.position.x, vertic + transform.position.y), new Vector2(.9f, .9f), 0);
            if (obstacles.Length == 0)
            {
                transform.position = moveVector + oldPos;
            }
            else if (moveVector + oldPos == targetPos)
            {
                target.Hurt(1);
                target.camera.Shake();
            }
        }
    }
}