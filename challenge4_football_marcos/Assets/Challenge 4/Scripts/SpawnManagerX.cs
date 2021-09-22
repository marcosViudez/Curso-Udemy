using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 1;
    public float enemySpeedLevelUp = 0.1f;

    public int level = 0;

    public GameObject player;

    void Start()
    {
        if(level == 0)
        {
            // reseteamos la velocidad de los enemigos para el nivel 1
            enemyPrefab.GetComponent<EnemyX>().speed = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // calcula el numero de enemigos que hay en pantalla
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }

    }

    // Generate random spawn position for powerups and enemy balls
    Vector3 GenerateSpawnPosition ()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }


    /// <summary>
    /// Crea la dificultad al spawnear powerUps aleatorios segun el numero de enemigos en pantalla
    /// </summary>
    /// <param name="numPowerUps"> Numero de powerUps que estan en pantalla </param>
    void randomLevelPowerUps(int numPowerUps)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // make powerups spawn at player end

        if (level < 4)
        {
            // si por debajo del level 4 queda un powerUp sin usar no instancio nada
            if(numPowerUps == 0)
            {
                Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
            }
        }
        else
        {
            // dificultad de powerUps
            int minimalPowerup = waveCount / 2;
            int maximalPowerUp = waveCount - 2;

            int PowerUpsForLevel = Random.Range(minimalPowerup, maximalPowerUp);
            int maxPowerUpsForLevel = PowerUpsForLevel - numPowerUps;

            for (int i = 0; i < maxPowerUpsForLevel; i++)
            {
                Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
            }
        }
    }


    void SpawnEnemyWave(int enemiesToSpawn)
    {

        // If no powerups remain, spawn a powerup
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) // check that there are zero powerups
        {
            randomLevelPowerUps(0);
        }
        else
        {
            randomLevelPowerUps(GameObject.FindGameObjectsWithTag("Powerup").Length);
        }


        // Spawn number of enemy balls based on wave number
        for (int i = 0; i < waveCount; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        // aumentamos en la ola nueva un enemigo mas
        waveCount++;
        // siguiente level
        level++;
        // aumentamos la velocidad de los enemigos un porcentaje enemySpeedLevelUp
        enemyPrefab.GetComponent<EnemyX>().speed += enemySpeedLevelUp;

        ResetPlayerPosition(); // put player back at start

    }

    // Move player back to position in front of own goal
    void ResetPlayerPosition ()
    {
        // reseteamos la posicion al estado inicial
        player.transform.position = new Vector3(0, 1, -7);

        // reseteamos la rotacion del player a cero
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        // resetamos la rotacion de la camara con el focal point
        player.GetComponent<PlayerControllerX>().FocalPoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }

}
