using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public GameController gc;
    public float timeBetweenWaves = 10f;
    private float timeTillNextWave;
    public float enemiesPerWave = 5f;
    public float timeBetweenEnemies = 0.2f;
    private float timeTillNextEnemy;

    public bool waveInProgress = false;
    private bool waveSpawningComplete = false;

    public List<GameObject> enemiesList;
    public int enemiesRemaining;

    public GameObject[] Enemies;

    void Start()
    {
        enemiesList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveInProgress)
        {
            timeTillNextWave = timeBetweenWaves;
            if (!waveSpawningComplete)
            {
                timeTillNextEnemy -= Time.deltaTime;
                if (timeTillNextEnemy <= 0)
                {
                    timeTillNextEnemy = timeBetweenEnemies;
                    GameObject enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)]);
                    enemy.GetComponent<Enemy>().deathCallback += this.enemyDeathAction;
                    enemy.gameObject.transform.position = gc.world.getRandomPosition();
                    enemiesList.Add(enemy);
                }

                Debug.Log(enemiesList.Count.ToString());

                if (enemiesList.Count >= enemiesPerWave)
                {
                    waveSpawningComplete = true;
                    enemiesRemaining = enemiesList.Count;
                }
            }
        }
        else
        {
            timeTillNextWave -= Time.deltaTime;

            if (timeTillNextWave <= 0)
            {
                waveSpawningComplete = false;
                waveInProgress = true;
                enemiesList.Clear();
            }
        }
    }

    public void enemyDeathAction(Enemy enemy)
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0) waveInProgress = false;
    }
}