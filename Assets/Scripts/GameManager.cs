using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public EnemySpawner enemySpawner;

    public int maxPlayerHealth = 100;
    public int playerHealth = 100;
    public int skeletonsKilled = 0;
    public HealthBar healthBar;
    public ScoreIndicator scoreIndicator;
    public GameObject gameOverMenu;
    public GameObject mainMenu;
    public GameObject pauseMenu;

    private int level = 0;

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
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        //StopCoroutine(GameOver());

        ChangeHealth(maxPlayerHealth);
        // healthBar.SetMaxHealth(maxPlayerHealth);
        // healthBar.SetHealth(maxPlayerHealth);
        skeletonsKilled = 0;
        scoreIndicator.ChangeScore(0);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(0f, -4f, 0f);
        player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        player.GetComponent<PlayerController>().health = maxPlayerHealth;

        player.GetComponent<Animator>().SetTrigger("isAlive");
        player.GetComponent<Animator>().Play("PlayerIdle");
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerController>().isDead = false;

        mainMenu.SetActive(false);
        healthBar.gameObject.SetActive(true);
        scoreIndicator.gameObject.SetActive(true);

        Time.timeScale = 1f;

        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.enabled = true;
    }

    public void ChangeHealth(int health)
    {
        playerHealth = health;
        healthBar.SetHealth(health);
        if (playerHealth <= 0)
        {
            //StartCoroutine(GameOver());
            //Invoke("GameOver", 2f);
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
        gameOverMenu.SetActive(true);
        // Time.timeScale = 0;
        enemySpawner.enabled = false;
    }
    /*

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }
    */
    public void EndGame()
    {
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        enemySpawner.ResetSpawner();
        enemySpawner.enabled = false;
        healthBar.gameObject.SetActive(false);
        scoreIndicator.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
