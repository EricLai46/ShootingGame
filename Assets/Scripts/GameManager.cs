using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    //game points
    public int game_score = 0;
    //game highest score
    public static int game_highscore = 0;
    //Number of ammunition
    public int game_ammo = 35;
    //game player
    Player player;
    //UI text
    Text text_ammo;
    Text text_hiscore;
    Text text_life;
    Text text_score;
    Text text_dead;
    Text text_sound;
    Button button_restart;
     Slider slider;
    GameObject panel;
    public float timer = 0;
    bool isactive = false;
    public GameObject enemyspawn1;
    public GameObject enemyspawn2;
    public GameObject enemyspawn3;
    public GameObject crosshair;
    void Start()
    {
        Instance = this;

        //get the game player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //get the UI text
        GameObject uicanvas = GameObject.Find("Canvas");
         panel = GameObject.Find("Panel");
        foreach (Transform t in uicanvas.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("text_ammo") == 0)
            {
                text_ammo = t.GetComponent<Text>();

            }
            else if (t.name.CompareTo("text_hiscore") == 0)
            {
                text_hiscore = t.GetComponent<Text>();
                text_hiscore.text = "HIgh Score " + game_highscore;

            }
            else if (t.name.CompareTo("text_life") == 0)
            {
                text_life = t.GetComponent<Text>();

            }
            else if (t.name.CompareTo("text_score") == 0)
            {
                text_score = t.GetComponent<Text>();

            }
            else if (t.name.CompareTo("button_restart") == 0)
            {
                button_restart = t.GetComponent<Button>();
                button_restart.gameObject.SetActive(false);//hide the restart button
                button_restart.onClick.AddListener(Loadscene);

            }

            else if (t.name.CompareTo("text_dead") == 0)
            {
                text_dead = t.GetComponent<Text>();

            }

        }
        text_life.text = "Health: " + player.player_life;
        text_score.text = "Score: " + game_score;
        text_ammo.text = game_ammo.ToString() + "/35";
        text_dead.gameObject.SetActive(false);
        foreach (Transform t in panel.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("text_sound") == 0)
            {
                text_sound = t.GetComponent<Text>();

            }
            else if (t.name.CompareTo("slider_sound") == 0)
            {
                slider = t.GetComponent<Slider>();
                

            }

        }
        panel.GetComponent<CanvasGroup>().alpha = 0;
        //slider.GetComponent<Slider>().onValueChanged.AddListener(ValueChanged);
        crosshair.SetActive(true);
        enemyspawn1 = GameObject.Find("EnemySpawn1");
        enemyspawn2 = GameObject.Find("EnemySpawn2");
        enemyspawn3 = GameObject.Find("EnemySpawn3");
        enemyspawn1.SetActive(true);
        enemyspawn2.SetActive(false);
        enemyspawn3.SetActive(false);

    }
    
    //update score
    public void SetScore(int score)
    {
        game_score += score;
        if(game_score>game_highscore)
        {
            game_highscore = game_score;
        }
        text_score.text = "Score: " + game_score;
        text_hiscore.text = "High Score: " + game_highscore;


    }
    //update the ammo
    public void SetAmmo(int ammo)
    {
        

        if (game_ammo > 0)
        { game_ammo -= ammo; }
        
        text_ammo.text = game_ammo.ToString()+"/35";


    }
    //update the life
    public void SetLife(int life)
    {
        text_life.text = "Health: "+life.ToString();
        // when player is dead, active the restart button
        if(life<=0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            button_restart.gameObject.SetActive(true);
            text_dead.gameObject.SetActive(true);
        }



    }
    public void Loadscene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("start");
        button_restart.gameObject.SetActive(false);
    }

    public void LoadAmmo(int ammo)
    {
   
            game_ammo = 35-ammo;
        text_ammo.text = game_ammo.ToString() + "/35";

    }
    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        if (isactive)
        {
            panel.GetComponent<CanvasGroup>().alpha = 0;
            isactive = false;
            crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
        else
        {
            panel.GetComponent<CanvasGroup>().alpha = 1;
            isactive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            crosshair.SetActive(false);

        }
    }

    

    private void ValueChanged(float value)
    {
        player.back.volume = value;
    }
    public void SetDifficulty()
    {
        IEnumerable<Toggle> toggleGroup = GameObject.Find("Canvas/Panel/ToggleGroup").GetComponent<ToggleGroup>().ActiveToggles();
        foreach (Toggle t in toggleGroup)
        {
            if (t.isOn)
            {
                switch (t.name)
                {
                    case "toggle_easy":
                        enemyspawn1.SetActive(true);
                        enemyspawn2.SetActive(false);
                        enemyspawn3.SetActive(false);
                        break;
                    case "toggle_normal":
                        enemyspawn1.SetActive(true);
                        enemyspawn2.SetActive(true);
                        enemyspawn3.SetActive(false);
                        break;
                    case "toggle_difficult":
                        enemyspawn1.SetActive(true);
                        enemyspawn2.SetActive(true);
                        enemyspawn3.SetActive(true);
                        break;
                }
                break;
            }
        }

    }
}
