using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    public static EnemyManagement Instance { get; set; }

    public List<GameObject> allNormalEnemiesOnRoute1 = new();
    public List<GameObject> allNormalEnemiesOnRoute2 = new();
    public List<GameObject> enemies = new(); // List of all enemies we can encounter
    public List<int> enemyPrice = new(); // List of corresponding cost of every enemies in list 'enemies'
    public GameObject towerBreakerPrefab;

    public Vector3[] waypoints_1;

    public Vector3[] waypoints_2;

    private Coroutine spawnTowerBreakerCoroutine;
    private float towerBreakerSpawnInterval = 30;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        waypoints_1 = new Vector3[] {
            new(19.5f, 6.6f, 0), new(19.5f, 10.2f, 0), new(11.4f, 10.2f, 0), 
            new(11.4f, 6, 0), new(4.4f, 6, 0), new(4.4f, 9.2f, 0),
            new(-13.5f, 9.2f, 0), new(-13.5f, 2, 0), new(-16.1f, 2, 0),
            new(-18.8f, 4.1f, 0), new(-24.2f, 4.1f, 0)
        };

        waypoints_2 = new Vector3[] {
            new(19.8f, -0.9f, 0), new(18.4f, 0.9f, 0), new(11.5f, 0.9f, 0),
            new(11.5f, -1, 0), new(13.2f, -3.2f, 0), new(16.4f, -3.2f, 0),
            new(16.4f, -5.6f, 0), new(13.9f, -8, 0), new(4, -8, 0),
            new(1.4f, -6.6f, 0), new(1.4f, -2.9f, 0), new(-3.5f, -2.9f, 0),
            new(-3.5f, 2.1f, 0), new(-10.55f, 2.1f, 0),new(-10.55f, -8.9f, 0),
            new(-13.5f, -8.9f, 0), new(-13.5f, -6.9f, 0), new(-20.55f, -6.9f, 0),
            new(-20.55f, -1, 0), new(-24.2f, -1, 0)
        };

        StartCoroutine(SpawnEnemy());
        SpotManager.Instance.OnTowerCountChanged += SpawnTowerBreakerWhenTowerExists;
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(8, 13));
        while (true)
        {
            // spawn 1-3 enemies every 10 seconds
            for (int enemyCount = Random.Range(1, 4); enemyCount > 0; enemyCount--)
            {
                int route = Random.Range(1, 3);
                if (route == 1)
                {
                    SpawnEnemyOnRoute1(0);
                }
                else
                {
                    SpawnEnemyOnRoute2(0);
                }
            }
            yield return new WaitForSeconds(10);
        }
    }

    public void SpawnEnemyOnRoute1(int enemyType)
    {
        if (EnemyBaseManagement.Instance.souls < enemyPrice[enemyType]) return;
        Vector3 spawnPosition = new(22.2f, 3.9f, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject spawnedEnemy = Instantiate(enemies[enemyType], spawnPosition, spawnRotation);
        EnemyBaseManagement.Instance.souls -= enemyPrice[enemyType];

        EnemyMovement enemyMovemet = spawnedEnemy.GetComponent<EnemyMovement>();
        enemyMovemet.allNormalEnemiesOnRoute = allNormalEnemiesOnRoute1;
        enemyMovemet.waypoints = waypoints_1;

        allNormalEnemiesOnRoute1.Add(spawnedEnemy);
    }
    
    public void SpawnEnemyOnRoute2(int enemyType)
    {
        if (EnemyBaseManagement.Instance.souls < enemyPrice[enemyType]) return;
        Vector3 spawnPosition = new(22.2f, -0.9f, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject spawnedEnemy = Instantiate(enemies[enemyType], spawnPosition, spawnRotation);
        EnemyBaseManagement.Instance.souls -= enemyPrice[enemyType];

        EnemyMovement enemyMovement = spawnedEnemy.GetComponent<EnemyMovement>();
        enemyMovement.allNormalEnemiesOnRoute = allNormalEnemiesOnRoute2;
        enemyMovement.waypoints = waypoints_2;

        allNormalEnemiesOnRoute2.Add(spawnedEnemy);
    }

    private void SpawnTowerBreakerWhenTowerExists(int towerCount)
    {
        if (towerCount == 1)
        {
            spawnTowerBreakerCoroutine ??= StartCoroutine(SpawnTowerBreaker());
        }
        else if (towerCount == 0)
        {
            StopCoroutine(spawnTowerBreakerCoroutine);
        }

        if (towerCount >= 1)
        {
            towerBreakerSpawnInterval -= (towerCount - 1) * 0.5f;
        }
    }

    private IEnumerator SpawnTowerBreaker()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            GameObject towerBreaker1 = Instantiate(towerBreakerPrefab, waypoints_1[0], Quaternion.identity);
            towerBreaker1.GetComponent<TowerBreakerMovement>().waypoints = waypoints_1;

            GameObject towerBreaker2 = Instantiate(towerBreakerPrefab, waypoints_2[0], Quaternion.identity);
            towerBreaker2.GetComponent<TowerBreakerMovement>().waypoints = waypoints_2;

            yield return new WaitForSeconds(towerBreakerSpawnInterval);
        }
    }
}
