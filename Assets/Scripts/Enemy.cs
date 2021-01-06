using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public Transform enemy_transform;
    // the player 
    Player player;
    //Enemy Navigation
    NavMeshAgent enemy_agent;
    
   public float enemy_speed = 2.0f;//the speed of enemy
    
    public float enemy_rotspeed = 5.0f;//rotation speed
   
    public float enemy_timer = 0;//timer

    public float enemy_attacktimer = 2;
    
    public int enemy_life = 5; //enemy healthy life

    protected EnemySpawn  enemy_spawn;
    
    Animator enemy_animator;//animation compoent
    public Transform lifeball;
    bool isattack = false;

    void Start()
    {
        enemy_transform = this.transform;
        //get the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //get the compent of Navigation
        enemy_agent = GetComponent<NavMeshAgent>();
        enemy_agent.speed = enemy_speed; //set the speed of enemy
        enemy_agent.SetDestination(player.transform.position);
        enemy_animator = this.GetComponent<Animator>();
        isattack = false;

    }
    public void init(EnemySpawn spawn)
    {
        enemy_spawn = spawn;
        enemy_spawn.enemy_Count++;
    }
    void Rotateto()
    {
        //get the direction of player
        Vector3 targetdir = player.transform.position - enemy_transform.position;
        //calculate the new direction
        Vector3 newdir = Vector3.RotateTowards(transform.forward, targetdir, enemy_rotspeed * Time.deltaTime, 0.0f);
        //rotate to the new direction
        enemy_transform.rotation = Quaternion.LookRotation(newdir);
        enemy_timer = enemy_attacktimer;


    }
    void Update()
    {
        //if player's health equals to 0, do nothing
        if (player.player_life <= 0)
            return;
         float distance = Vector3.Distance(player.transform.position, transform.position);
        //update the timerenemy_transform.position, player.transform.position
        //enemy_timer -= Time.deltaTime;

        //get the current state of animation
        //AnimatorStateInfo stateinfo=enemy_animator.GetCurrentAnimatorStateInfo(0);
        //transform.LookAt(player.transform.position);
        //float distance = Vector3.Distance(player.transform.position, enemy_transform.position);



        //if (distance <= enemy_attackdistance)
        //{

        //    enemy_timer += Time.deltaTime;
        //    if (enemy_timer > enemy_attacktimer)
        //    {
        //        enemy_animator.SetTrigger("attack");
        //        enemy_timer = 0;
        //    }
        //    else
        //    {
        //        enemy_animator.SetBool("run", false);
        //    }

        //}
        //else
        //{
        //    enemy_timer = enemy_attacktimer;
        //    if (enemy_animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        //    {
        //        enemy_agent.SetDestination(player.transform.position);
        //    }

        //}
        //if it is on idle state not on translateion state
        if (enemy_animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && !enemy_animator.IsInTransition(0))
        {
            enemy_animator.SetBool("idle", false);
           // transform.LookAt(player.transform.position);
            //if the distance between player and enemy less than 1.5m, then go to the attack state
            if (distance <=1.5f)
            {
                
                enemy_timer += Time.deltaTime;
                //stop looking the way
                enemy_agent.ResetPath();
                //set the state to the attack state
                if (enemy_timer >= enemy_attacktimer)
                {
                    enemy_animator.SetTrigger("attack");
                    enemy_timer = 0;
                }
            }
            else
            {
                //reset the timer
                enemy_timer = enemy_attacktimer;
                //set the position to find
                enemy_agent.SetDestination(player.transform.position);
                //set the state to run state
                enemy_animator.SetBool("run", true);

            }



            }

            //if is on running state not the translation state
            if (enemy_animator.GetCurrentAnimatorStateInfo(0).IsName("run") && !enemy_animator.IsInTransition(0))
        {
            //transform.LookAt(player.transform.position);
            enemy_animator.SetBool("run", false);
            //every one second re-locate the position of player
            

            //if the distance between player and enemy less than 1.5m, then attack the player
            if (distance <= 1.5f)
            {
                enemy_timer += Time.deltaTime;
                //stop looking the way
                enemy_agent.ResetPath();
               // enemy_animator.SetBool("idle", true);
                if (enemy_timer >= enemy_attacktimer)
                //set the state to the attack state
                {
                    isattack = true;
                    enemy_animator.SetTrigger("attack");
                    enemy_timer = 0;
                }
            }
            else
            {
                enemy_timer = enemy_attacktimer;
                enemy_agent.SetDestination(player.transform.position);

            }

        }

        //if is on attack state not the translation state
        if (enemy_animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && !enemy_animator.IsInTransition(0))
        {
            //face to the player
            Rotateto();
            

            if (isattack)

            {//player takes the damage
                player.OnDamage(1);

                isattack = false;
            }
        }
        //if is on death state not the translation state
        if (enemy_animator.GetCurrentAnimatorStateInfo(0).IsName("death") && !enemy_animator.IsInTransition(0))
        {
            enemy_animator.SetBool("death", false);
            //when played the death animation
            AnimatorStateInfo stateinfo = enemy_animator.GetCurrentAnimatorStateInfo(0);
            if (stateinfo.normalizedTime >= 1.0f)
            {
                //increase the score
                GameManager.Instance.SetScore(100);

                enemy_spawn.enemy_Count -= 1;
                //destroy itself
                Destroy(this.gameObject);
                Instantiate(lifeball, this.enemy_transform.position+Vector3.up, enemy_transform.rotation);
            }

            //}
            //Vector3 targpos = player.transform.position;
            //targpos.y = enemy_transform.position.y;
            //transform.LookAt(player.transform.position);
            //float distance = Vector3.Distance(enemy_transform.position, player.transform.position);
            //enemy_agent.SetDestination(player.transform.position);
            //if (distance < enemy_attackdistance)
            //{
            //    enemy_timer += Time.deltaTime;
            //    if (enemy_timer > enemy_attacktimer)
            //    {
            //        enemy_animator.SetTrigger("attack");
            //        enemy_timer = 0;

            //    }
            //    else
            //    {
            //        enemy_animator.SetBool("run", false);
            //    }
            //}
            //else
            //{
            //    enemy_timer = enemy_attacktimer;
            //    if (enemy_animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
            //    {
            //        enemy_agent.SetDestination(player.transform.position);
            //    }

            //}
        }
    }
    public void OnDamage(int damage)
    {
        enemy_life -= damage;
       //if the life less than 0, play death animation
       if(enemy_life<=0)
        {
            enemy_animator.SetBool("death", true);
            //stop the agent
            enemy_agent.ResetPath();


        }
        


    }

}
