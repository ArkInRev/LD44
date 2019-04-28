using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{

    [Header("Basic")]
    public float maximumTime = 20;
    private float timeRemaining;


    [Header("Scoring")]
    private int[] currency = new int[2];
    public int[] curValue = new int[2];



    //private int score = 0;

    [Header("UI Elements")]
    public Canvas[] gameCanvas = new Canvas[5];

    public TMP_Text score;
    public TMP_Text mushrooms;
    public TMP_Text lifeforms;
    public TMP_Text secondsLeft;
    public Slider timeSlider;

    [Header("Enemy Spawning")]
    public Transform myTilemap;
    public Tilemap terrainTilemap;
    public Transform playertransform;
    private Vector2 posToSpawn;
    public int spawnMaxRange = 20;
    public int safeRange = 10;
    public float timeBetweenSpawn = 5;
    private float timeUntilNextSpawn;
    public GameObject[] spawnables;





    [SerializeField] private int thisGame = GameState.NewgameAtTitle;


    void Awake()
    {
        timeRemaining = maximumTime;
        for (int i = 0; i < gameCanvas.Length; i++)
        {
            gameCanvas[i].enabled = true;


        }
        myTilemap = GameObject.Find("Others").transform;


    }

    private void Update()
    {
        ShowProperUI();
        if (Input.GetButtonDown("Pause"))
        {

            switch (thisGame)
            {
                case GameState.NewgameAtTitle:
                    thisGame = GameState.GameStarted;
                    break;
                case GameState.InProgressShop:
                    thisGame = GameState.GameStarted;
                    break;
                case GameState.GameStarted:
                    thisGame = GameState.InProgressShop;
                    break;
                case GameState.GameOver:
                    thisGame = GameState.NewgameAtTitle;
                    RestartGame();
                    break;
                case GameState.NewgameAtControls:
                    thisGame = GameState.GameStarted;
                    break;
                default:
                    break;
            }

        }
        if (Input.GetButtonDown("Next"))
        {
            switch (thisGame)
            {
                case GameState.NewgameAtTitle:
                    thisGame = GameState.NewgameAtControls;
                    break;
                case GameState.GameOver:
                    thisGame = GameState.NewgameAtTitle;
                    RestartGame();
                    break;
                default:
                    break;

            }

        }

        if (Input.GetButtonDown("SellMushroom"))
        {
            switch (thisGame)
            {
                case GameState.InProgressShop:
                    //try to sell a mushroom
                    Sell(0);
                    break;
                default:
                    break;

            }

        }
        if (Input.GetButtonDown("SellLifeform"))
        {
            switch (thisGame)
            {
                case GameState.InProgressShop:
                    //try to sell a mushroom
                    Sell(1);
                    break;
                default:
                    break;

            }

        }



    }

    void FixedUpdate()
    {
        if (timeRemaining > maximumTime) timeRemaining = maximumTime;
        timeRemaining -= Time.deltaTime;

        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn <= 0) TryToSpawn(ref posToSpawn);


        UpdateMainUI();

        if (timeRemaining <= 0)
        {
            thisGame = GameState.GameOver;
        }
    }

    public int AddCurrency(int type,int amount)
    {
        //currency[type] += Mathf.Abs(amount); // only work with positive integers
        currency[type] += 1; // count instead
        return currency[type];
    }

    public int LoseCurrency(int type, int amount)
    {
        //currency[type] -= Mathf.Abs(amount); // only work with positive integers
        currency[type] -= 1; // count instead
        return currency[type];
    }

    public int GetCurrency(int type)
    {
        return currency[type];
    }

    public void ClearCurrency(ref int[] currency)
    {
        currency = new int[2];
    }

    private int calculateScore()
    {
        int score = 0;
        for(int i = 0; i < currency.Length; i++)
        {
            score += currency[i] * curValue[i];
        }


        return score;
    }

    private void ShowProperUI()
    {
        for(int i = 0; i < gameCanvas.Length; i++)
        {
            gameCanvas[i].enabled = (thisGame == i)?true:false;

        }
        gameCanvas[2].enabled = (thisGame >= 2) ? true : false;

        if (thisGame == 2)
        {
            Time.timeScale = 1f;
        } else
        {
            Time.timeScale = 0f;
        }
        
    }

    private void UpdateMainUI()
    {
        score.text = "$"+calculateScore().ToString();
        mushrooms.text = GetCurrency(0).ToString();
        lifeforms.text = GetCurrency(1).ToString();

        secondsLeft.text = Mathf.RoundToInt(timeRemaining).ToString() + "s";

        timeSlider.value = timeRemaining / maximumTime;
    }

    private void Sell(int thing)
    {
        if (GetCurrency(thing) > 0 && timeRemaining > 0)
        {
            LoseCurrency(thing, 1);
            switch (thing)
            {
                case 0:
                    timeRemaining += 5;
                    break;
                case 1:
                    timeRemaining += 60;
                    break;
                default:
                    break;
            }
            if (timeRemaining > maximumTime) timeRemaining = maximumTime;
            UpdateMainUI();
        }



    }

    public void ReduceTimer(float amount)
    {
        timeRemaining -= amount;
        UpdateMainUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void TryToSpawn(ref Vector2 posToSpawn)
    {
        Vector2 playerPos = new Vector2(playertransform.position.x,playertransform.position.y);
        Vector2 randPos = (playerPos+ Random.insideUnitCircle*spawnMaxRange);
        posToSpawn = randPos;
        float dist = Vector2.Distance(playerPos,randPos);

        //Debug.Log("Trying to Spawn Dist: "+dist.ToString()+" @"+posToSpawn.ToString());
        if (dist > safeRange)
        {
            Vector3Int tilePos = terrainTilemap.WorldToCell(new Vector3(posToSpawn.x, posToSpawn.y, 0));
            TileBase thisTile = terrainTilemap.GetTile(tilePos);

            if (thisTile != null)
            {
                //something was there, cancel spawn
                Debug.Log("Tile hit: " + thisTile.name);
            }
            else
            {
                Debug.Log("Spawning: Nothing was here");
                //actually spawn something
                //0 robot 1 lifeform
                int thingIndex = Random.Range(0, 2);
                GameObject thing = spawnables[thingIndex];

                GameObject go = Instantiate(thing, randPos, Quaternion.identity) as GameObject;
                go.transform.SetParent(myTilemap);
                timeUntilNextSpawn = timeBetweenSpawn;
            }

        }


    }


    public static class GameState
    {
        public const int NewgameAtTitle = 0; // newgame at the title Menu PAUSED
        public const int NewgameAtControls = 1; // new game at the controls UI Menu PAUSED
        public const int GameStarted = 2; // the main game is running RUNNING
        public const int InProgressShop = 3; // game is in progress at the Shop PAUSED
        public const int GameOver = 4; // game is showing Ending screen with restart option PAUSED
    }
    
}
