using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item
{
    void Use(PlayerBehavior player)
    {
        player.Heal(1);
    }
}
