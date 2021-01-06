using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //enemy prefab
    public Transform enemyprefab;
    //the number of enemies
    public int enemy_Count = 0;
    //the max number of enemies
    public int enemy_maxCount = 3;
    //Time interval for spawning enemies
    public float enemy_timer = 0;
    protected Transform enemytransform;

    void Start()
    {
        enemytransform = this.transform;     
    }

    // Update is called once per frame
    void Update()
    {
        //if the number is equal to the max number of enemies, stop working
        if(enemy_Count>=enemy_maxCount)
        {
            return;

        }

        //wait for time
        enemy_timer -= Time.deltaTime;
        if(enemy_timer<=0)
        {
            enemy_timer = Random.value * 15.0f;
            if(enemy_timer<5)
            
                enemy_timer = 5;
            //spawn the enemy
            Transform obj = (Transform)Instantiate(enemyprefab, enemytransform.position, Quaternion.identity);

            //get the scripts of enemy
            Enemy enemy = obj.GetComponent<Enemy>();
            //initialization
            enemy.init(this);
        }
    }
}
