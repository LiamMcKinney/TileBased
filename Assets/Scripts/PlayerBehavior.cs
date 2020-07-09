using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    float horiz;
    float vertic;
    int HP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vertic = Input.GetAxisRaw("Vertical");
        Move(horiz, vertic);
    }

    private void Move(float x, float y)
    {
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(oldPos.x+x, oldPos.y+y, oldPos.z);
    }
}
