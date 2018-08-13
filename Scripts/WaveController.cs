using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public List<Enemy> enemiesList;
    public int enemiesRemaining;

    public GameObject[] Enemies;

    public int waveCount = 0;
    public TextMeshProUGUI waveCountText;
    public Slider waveEnemyCount;

    void Start()
    {
        enemiesList = new List<Enemy>();

        waveCount++;
        waveCountText.text = waveCount.ToString();
        enemiesRemaining = (int) enemiesPerWave;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveInProgress)
        {
            timeTillNextWave = timeBetweenWaves;
            if (enemiesRemaining > 0)
            {
                timeTillNextEnemy -= Time.deltaTime;
                if (timeTillNextEnemy <= 0)
                {
                    timeTillNextEnemy = timeBetweenEnemies;
                    GameObject enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)]);
                    enemy.GetComponent<Enemy>().deathCallback += this.enemyDeathAction;
                    enemy.gameObject.transform.position = gc.world.getRandomPosition();
                    enemy.transform.parent = this.transform;

                    enemiesList.Add(enemy.GetComponent<Enemy>());
                    enemiesRemaining--;
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
                enemiesRemaining = (int) enemiesPerWave;

                waveCount++;
                waveCountText.text = waveCount.ToString();
            }
        }

        waveEnemyCount.value = enemiesList.Count / enemiesPerWave;
    }

    public void enemyDeathAction(Enemy enemy)
    {
        enemiesList.Remove(enemy);

        if (enemiesRemaining <= 0) waveInProgress = false;
    }
}