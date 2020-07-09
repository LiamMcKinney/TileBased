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
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        horiz = Input.GetAxisRaw("Horizontal");
        vertic = Input.GetAxisRaw("Vertical");
        if (horiz != 0 || vertic != 0)
        {
            if (counter > 100)
            {
                Move(horiz, vertic);
                enemyManager.Step();
                counter = 0;
            }
        }
        camera.transform.position = transform.position + camOffset;
    }

    private void Move(float x, float y)
    {
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(oldPos.x+x, oldPos.y+y, oldPos.z);
    }
}
