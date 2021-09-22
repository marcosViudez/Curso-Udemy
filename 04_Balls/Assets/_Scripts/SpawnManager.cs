using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    // array si hay varios modelos de enemigos
    public GameObject enemyPrefab;

    private float spawnRange = 9.0f;
    public int enemyCount;
    public int enemyWave = 1;

    public GameObject powerUpPrefab;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(enemyWave); 
        
    }

    void Update()
    {
        enemyCount = GameObject.FindObjectsOfType<Enemy>().Length;

        if(enemyCount == 0)
        {
            enemyWave++;    // aumenta un enemigo por oleada
            SpawnEnemyWave(enemyWave);

            // instanciar un powerUp por oleada
            Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
        }
    }

    /// <summary>
    /// Genera una posicion aleatoria dentro de la zona del juego
    /// </summary>
    /// <returns> Devuelve la posicion aleatoria dentro del juego</returns>
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPositionX = Random.Range(-spawnRange, spawnRange);
        float spawnPositionZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        return randomPosition;
    }

    /// <summary>
    /// Generacion de olas de enemigos.
    /// </summary>
    /// <param name="numberOfEnemies"> Numero de enemigos a spawnear.</param>
    private void SpawnEnemyWave(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    
}
