using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class PlayerBehavior : LivingThing
{
    float horiz;
    float vertic;
    int counter;
    public CameraManager camera;
    public Vector3 camOffset;
    public EnemyManager enemyManager;
    public Vector2 lastDirectionalInputs;
    bool attacking;
    public int damage;
    public int maxHP;

    public SpriteRenderer sprite;

    public List<Sprite> playerSprites;//up, then clockwise

    public GameObject[] inventorySlots = new GameObject[3];

    public Item[] inventory = new Item[3];
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
        if (!isDead)
        {
            counter++;
            horiz = Input.GetAxisRaw("Horizontal");
            vertic = Input.GetAxisRaw("Vertical");
            attacking = Input.GetKey("space");

            bool isUsingSlot1 = Input.GetKeyDown("1");
            bool isUsingSlot2 = Input.GetKeyDown("2");
            bool isUsingSlot3 = Input.GetKeyDown("3");

            if (isUsingSlot1 && inventory[0] != null)
            {
                UseItem(0);
                enemyManager.Step();
            }
            else if (isUsingSlot2 && inventory[1] != null)
            {
                UseItem(1);
                enemyManager.Step();
            }
            else if (isUsingSlot3 && inventory[2] != null)
            {
                UseItem(2);
                enemyManager.Step();
            }
            else if (horiz != 0 || vertic != 0)
            {
                UpdateSprite(horiz, vertic);

                if (lastDirectionalInputs != new Vector2(horiz, vertic))
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
            camera.position = transform.position + camOffset;
            lastDirectionalInputs = new Vector2(horiz, vertic);
        }
    }

    private void Move(float x, float y)
    {
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(x,y,0)+oldPos;
    }

    public void Obtain(Item item)
    {
        for(int i = 0; i < 3; i++)
        {
            if(inventory[i] == null)
            {
                item.transform.parent = inventorySlots[i].transform;
                item.transform.localPosition = Vector3.zero;

                Image slotImage = inventorySlots[i].GetComponent<Image>();
                SpriteRenderer itemSprite = item.GetComponent<SpriteRenderer>();
                slotImage.sprite = itemSprite.sprite;
                slotImage.color = itemSprite.color;
                slotImage.enabled = true;

                inventory[i] = item;
                break;
            }
        }
    }

    public void Heal(int amount)
    {
        hp += amount;
        if (maxHP < hp)
        {
            hp = maxHP;
        }
    }

    void UseItem(int index)
    {
        inventory[index].Use(this);
        Destroy(inventory[index].gameObject);
        inventory[index] = null;
        inventorySlots[index].GetComponent<Image>().enabled = false;
    }

    void UpdateSprite(float horiz, float vertic)
    {
        switch (3 * horiz + vertic)
        {
            case (1):
                sprite.sprite = playerSprites[0];
                break;
            case (4):
                sprite.sprite = playerSprites[1];
                break;
            case (3):
                sprite.sprite = playerSprites[2];
                break;
            case (2):
                sprite.sprite = playerSprites[3];
                break;
            case (-1):
                sprite.sprite = playerSprites[4];
                break;
            case (-4):
                sprite.sprite = playerSprites[5];
                break;
            case (-3):
                sprite.sprite = playerSprites[6];
                break;
            case (-2):
                sprite.sprite = playerSprites[7];
                break;
        }
    }

    public void Hurt(int amount)
    {
        base.Hurt(amount);
        camera.Shake();
        camera.FlashRed();
    }
}
