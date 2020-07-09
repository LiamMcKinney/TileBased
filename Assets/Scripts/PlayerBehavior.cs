using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : LivingThing
{
    float horiz;
    float vertic;
    int counter;
    public Camera camera;
    public Vector3 camOffset;
    public EnemyManager enemyManager;
    Vector2 lastDirectionalInputs;
    bool attacking;
    public int damage;
    public int maxHP;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        counter = 0;
        lastDirectionalInputs = new Vector2(0, 0);
        if(damage == null)
        {
            damage = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        horiz = Input.GetAxisRaw("Horizontal");
        vertic = Input.GetAxisRaw("Vertical");
        attacking = Input.GetKey("space");
        if (horiz != 0 || vertic != 0)
        {
            if(lastDirectionalInputs != new Vector2(horiz, vertic))
            {
                counter = 101;
            }
            if (counter > 100)
            {
                if (!attacking)
                {
                    Collider2D[] obstacles = Physics2D.OverlapBoxAll(new Vector2(horiz + transform.position.x, vertic + transform.position.y), new Vector2(.9f, .9f), 0);
                    if (obstacles.Length == 0)
                    {
                        Move(horiz, vertic);
                        enemyManager.Step();
                        counter = 0;
                    }
                }
                else
                {
                    Collider2D victim = Physics2D.OverlapPoint(new Vector3(horiz, vertic, 0) + transform.position);
                    if (victim != null)
                    {
                        if (victim.GetComponent<LivingThing>() != null)
                        {
                            victim.GetComponent<LivingThing>().Hurt(damage);
                            enemyManager.Step();
                        }
                    }
                }
            }
        }
        camera.transform.position = transform.position + camOffset;
        lastDirectionalInputs = new Vector2(horiz, vertic);
    }

    private void Move(float x, float y)
    {
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(x,y,0)+oldPos;
    }

    public void Obtain(Item item)
    {

    }

    public void Heal(int amount)
    {
        hp += amount;
        if (maxHP < hp)
        {
            hp = maxHP;
        }
    }
}
