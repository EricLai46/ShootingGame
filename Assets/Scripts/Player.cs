using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform player_transform;
    //character controller 
    CharacterController player_cc;
  
    //player speed
   public float move_speed = 3.0f;
    //gravity
    public float player_gravity = 2.0f;
    //health life
    public int player_life = 3;

    //jump speed
    public float jump_speed = 3.0f;

    //camera transform
    public Transform cameratransform;
    //camera rotate
    Vector3 camerarot;
    //camera height
    public float cameraheight = 1.5f;
    private Vector3 moveDirection = Vector3.zero;

    //gun transform
    public Transform gun_transform;
    //when shoot, the layer mask that layer can hit
    public LayerMask gun_layer;
    //Particle effect after hitting the target
    public Transform gun_fx;
    //the sound of shooting
    public AudioClip gun_audio;
    //the timer between shooting
    public float gun_timer = 0;
    //the speed of bullet
    public float bullet_speed = 30.0f;

    public Slider slider;

    public AudioSource back;
    void Start()
    {
        
        player_transform = this.transform;
        //get player character controller
        player_cc = this.GetComponent<CharacterController>();

        //get camera
        cameratransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //get the initial position of camera
        cameratransform.position = player_transform.TransformPoint(0, cameraheight, 0);
        //set the position and rotation same as player
        cameratransform.rotation = player_transform.rotation;
        camerarot = cameratransform.eulerAngles;
        //lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        back = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if health life equals to 0, do nothing
        if (player_life <= 0)
        {
            return;

        }
        Control();

        back.volume = slider.value;
        
        //update the shooting time
        gun_timer -= Time.deltaTime;

        //click the left mouse button to fire
        if (Input.GetMouseButton(0) && gun_timer <= 0 && player_life > 0 && GameManager.Instance.game_ammo > 0)
        {
            gun_timer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(gun_audio);
            //reduce the bullets,update the bullets UI
            GameManager.Instance.SetAmmo(1);
            //Ray detection result
            RaycastHit info;
            bool hit = Physics.Raycast(gun_transform.position, cameratransform.TransformDirection(Vector3.forward), out info, 100, gun_layer);
            if (hit)
            {
                //if hit the gameobject with tag "enemy"
                if (info.transform.tag.CompareTo("Enemy") == 0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    //enemy reduce life
                    enemy.OnDamage(1);


                }
                //Instantiate(gun_fx, info.point, info.transform.rotation);

            }
        }

            if (Input.GetKeyDown(KeyCode.R)&& player_life > 0)
            {
                
                GameManager.Instance.LoadAmmo(0);

            }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // canvas.enabled = !canvas.enabled;
            GameManager.Instance.Pause();
        }



    }
    //the code to control the player how to move
    void Control()
    {
        //get the distance of mouse
        float rh, rv;
        rh = Input.GetAxis("Mouse X");
        rv = Input.GetAxis("Mouse Y");

        //rotate the camera
        camerarot.x -= rv;
        camerarot.y += rh;
        cameratransform.eulerAngles = camerarot;

        //The facing direction of the bishop agrees with camera
        Vector3 camrot = cameratransform.eulerAngles;
        //camerarot.x = 0;
        //camerarot.z = 0;
        player_transform.eulerAngles = camerarot;




        //float xm = 0, ym = 0, zm = 0;
        ////the movement of gravity
        //ym = -player_gravity * Time.deltaTime;
        // the movement of player
        if (player_cc.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= move_speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jump_speed;
        }
        moveDirection.y -= player_gravity * Time.deltaTime;
        player_cc.Move(moveDirection * Time.deltaTime);
    



        //if (Input.GetButtonDown("W"))
        //{
        //    zm += move_speed * Time.deltaTime;

        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    zm -= move_speed * Time.deltaTime;

        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    xm -= move_speed * Time.deltaTime;

        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    xm += move_speed * Time.deltaTime;

        //}
        //if(Input.GetKey(KeyCode.Space))
        //{
        //    ym += jump_speed * Time.deltaTime;

        //}


        //player_cc.Move(player_transform.TransformDirection(new Vector3(xm, ym, zm)));


        //update the position of camera
        cameratransform.position = player_transform.TransformPoint(0, cameraheight, 0);



    }
    public void OnDamage(int damage)
    {
        player_life -= damage;
        
        //update the UI
        GameManager.Instance.SetLife(player_life);
        //cancel the mouse locked
        if (player_life <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;

    }

}
