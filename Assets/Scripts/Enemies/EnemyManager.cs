using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public GameObject universalEnemyPrefab;
    public GameObject universalItemPrefab;

    public List<GameObject> enemies;
    public List<GameObject> disabledEnemies;

    public float spawnTime;
    public float remainingSpawnTime;
    public EnemyData meleeEnemy;
    public EnemyData rangedEnemy;
    public float spawnDistance;
    public List<RewardContainer> lootTable;

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
            if(spawnTime > 0.5f)
            {
                spawnTime *= 0.99f;
            }
        }
    }
    public GameObject InstantiateEnemy(EnemyData enemyData, Vector3 position, Transform parent = null)
    {
        GameObject enemy;
        if (true)
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
        Collider2D col = enemy.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
            col.excludeLayers = 0;
        }
        enemy.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        EnemySetup enemySetup = enemy.GetComponent<EnemySetup>();
        enemySetup.enemyData = enemyData;
        enemySetup.Initialize();
        enemy.SetActive(true);
        return enemy;
    }
    public void DisableEnemy(GameObject enemy)
    {
        if (Random.value >= .90f)
        {
            GameObject newItem = Instantiate(universalItemPrefab, enemy.transform);
            RewardContainer rewardContainer = lootTable[Random.Range(0, lootTable.Count - 1)];
            newItem.GetComponent<ItemController>().rewardContainer = rewardContainer;
            if (rewardContainer.weapon != null)
                newItem.GetComponentInChildren<SpriteRenderer>().sprite = rewardContainer.weapon.icon;
            if (rewardContainer.passive != null)
                newItem.GetComponentInChildren<SpriteRenderer>().sprite = rewardContainer.passive.icon;
        }
        enemy.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        enemy.transform.localScale = Vector3.one;
        enemy.SetActive(false);
        var move = enemy.GetComponent<EnemyBase>();
        if (move != null) move.enabled = false;
        var col = enemy.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        enemy.layer = LayerMask.NameToLayer("Enemy");
        var sr = enemy.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = Color.white;
        foreach (var weapon in enemy.GetComponentsInChildren<Weapon>())
        {
            weapon.gameObject.SetActive(false);
        }

        disabledEnemies.Add(enemy);
    }
}
