using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public GameObject universalEnemyPrefab;

    public List<GameObject> enemies;
    public List<GameObject> disabledEnemies;

    public float spawnTime;
    public float remainingSpawnTime;
    public EnemyData meleeEnemy;
    public EnemyData rangedEnemy;
    public float spawnDistance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        remainingSpawnTime -= Time.fixedDeltaTime;
        if (remainingSpawnTime <= 0)
        {
            remainingSpawnTime += spawnTime;
            float angle = Random.Range(0f, Mathf.PI * 2);
            InstantiateEnemy((Random.value < 0.9f) ? meleeEnemy : rangedEnemy, new Vector3(Mathf.Cos(angle) * spawnDistance, Mathf.Sin(angle) * spawnDistance, 0));
        }
    }
    public GameObject InstantiateEnemy(EnemyData enemyData, Vector3 position, Transform parent = null)
    {
        GameObject enemy;
        if (disabledEnemies.Count == 0)
        {
            enemy = Instantiate(universalEnemyPrefab);
            enemies.Add(enemy);
        }
        else
        {
            enemy = disabledEnemies[0];
            disabledEnemies.RemoveAt(0);
        }
        if (parent != null)
            enemy.transform.SetParent(parent.transform, false);
        else
            enemy.transform.SetParent(null);

        enemy.transform.SetPositionAndRotation(position, Quaternion.identity);
        enemy.transform.localScale = Vector3.one;

        EnemySetup enemySetup = enemy.GetComponent<EnemySetup>();
        enemySetup.enemyData = enemyData;
        enemySetup.Initialize();
        enemy.SetActive(true);
        return enemy;
    }
    public void DisableEnemy(GameObject enemy)
    {
        enemy.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        enemy.transform.localScale = Vector3.one;
        enemy.SetActive(false);
        foreach (var weapon in enemy.GetComponentsInChildren<Weapon>())
        { Destroy(weapon); }
        disabledEnemies.Add(enemy);
    }
}
