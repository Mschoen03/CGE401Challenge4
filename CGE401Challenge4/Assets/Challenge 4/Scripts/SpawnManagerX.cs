using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRangeX = 10;
    private float spawnZMin = 15;
    private float spawnZMax = 25;

    public int enemyCount;
    public int waveCount = 1;

    public float enemySpeed = 60f;

    public GameObject player;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI messageText;

    private bool gameStarted = false;
    private bool gameOver = false;

    private int enemiesEnteredGoal = 0;
    private int enemiesThisWave = 0;

    void Start()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (!gameStarted)
        {
            messageText.text = "Press SPACE to Start";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                messageText.text = "";
                gameStarted = true;
                Time.timeScale = 1f;
            }
            return;
        }

        if (waveCount > 10 && !gameOver)
        {
            gameOver = true;
            messageText.text = "YOU WIN! Press R to Restart";
        }

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0 && !gameOver)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    public void LoseGame()
    {
        if (!gameOver)
        {
            gameOver = true;
            messageText.text = "YOU LOSE! Press R to Restart";
        }
    }

    public void EnemyReachedGoal()
    {
        enemiesEnteredGoal++;

        if (enemiesEnteredGoal >= enemiesThisWave && !gameOver)
        {
            LoseGame();
        }
    }

    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15);

        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // track wave enemy count
        enemiesThisWave = enemiesToSpawn;
        enemiesEnteredGoal = 0;

        // spawn enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        waveText.text = "Wave: " + waveCount;

        waveCount++;

        ResetPlayerPosition();
        enemySpeed += 10;
    }

    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
