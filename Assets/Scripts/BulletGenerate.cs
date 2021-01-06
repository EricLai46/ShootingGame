using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerate : MonoBehaviour
{
    public int shootrate = 10;// how many bullet per second
    public float timer = 0; //the timer of bullet
    public bool isFireing = false;//if is firing
    public GameObject bulletpre;//the prefab of bullet
    public Player player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.player_life > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isFireing = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isFireing = false;
            }
            if (isFireing&&GameManager.Instance.game_ammo>0)
            {
                timer += Time.deltaTime;
                if (timer > 1f / shootrate)
                {
                    shoot();
                    timer = 0;
                }

            }
        }
    }

    void shoot()
    {
        GameObject.Instantiate(bulletpre, transform.position, transform.rotation);

    }
}
