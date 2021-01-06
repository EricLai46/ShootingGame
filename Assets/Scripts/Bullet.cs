using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100; // the speed of bullet
    public GameObject[] bulletholes;
    Player player; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.player_life > 0&&GameManager.Instance.game_ammo>0) { 
        Vector3 oripos = transform.position;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Vector3 direction = transform.position - oripos;
        float length = (transform.position - oripos).magnitude;

        RaycastHit info;
        bool isColider=Physics.Raycast(oripos, direction, out info, length);
            if (isColider)
            {
                int index = Random.Range(0, 2);
                GameObject bulletpre = bulletholes[index];
                Vector3 pos = info.point; //get the point which ray hits
                GameObject go = GameObject.Instantiate(bulletpre, pos, Quaternion.identity) as GameObject;
                go.transform.LookAt(pos - info.normal);
                go.transform.Translate(Vector3.back * 0.01f);
            }
        }
    }
}
