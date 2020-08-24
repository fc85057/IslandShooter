using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public EnemySpawner enemySpawner;

    public int maxPlayerHealth = 100;
    public int playerHealth = 100;
    public int skeletonsKilled = 0;
    public HealthBar healthBar;
    public ScoreIndicator scoreIndicator;

    private int level = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.enabled = true;

        healthBar.SetMaxHealth(100);
        healthBar.SetHealth(100);
        scoreIndicator.ChangeScore(0);
    }

    public void ChangeHealth(int health)
    {
        playerHealth = health;
        healthBar.SetHealth(health);
        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void ChangeScore(int points)
    {
        skeletonsKilled += points;
        scoreIndicator.ChangeScore(skeletonsKilled);
    }

    void GameOver()
    {
        Debug.Log("Game Over.");
    }

    /*
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void InitGame()
    {
        // update display
        // get rid of killed enemies (call function in Enemy Spawner)
        // spawn new level / enemies (call function in Enemy Spawner)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
