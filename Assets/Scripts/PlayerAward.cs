using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAward : MonoBehaviour
{
    public Player player;
    public Text text_life;
     void Start()
    {
        
    }

    public void GetAward(ItemType type)
    {
        if (player.player_life > 0 && player.player_life <= 4)
        {
            player.player_life += 1;
            text_life.text = "Health: " + player.player_life;
        }
    }
}
