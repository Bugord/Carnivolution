using System;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Enemy.MeleeEnemy;
using GameEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyDifficulty
{
    public int difficulty;
    public GameObject enemyPrefab;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int enemyCap;
    [SerializeField] private List<GameObject> enemies;

    [SerializeField] private List<EnemyDifficulty> enemiesWithDifficulties;
    
    [SerializeField] private float minSpawnDistance;
    [SerializeField] private float maxSpawnDistance;

    [SerializeField] private List<GameObject> enemiesPrefabs;

    private PlayerController _playerController;

    [SerializeField] private float spawnCooldown;
    private float _spawnTimer;

    private void Start()
    {
        _playerController = GameManager.Instance.playerController;

        enemiesWithDifficulties = enemiesPrefabs.Select(x => new EnemyDifficulty
        {
            difficulty = x.GetComponentInChildren<BaseEnemy>().Difficulty,
            enemyPrefab = x
        }).ToList();
    }

    private void OnEnable()
    {
        GlobalEventManager.EnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        GlobalEventManager.EnemyDied -= OnEnemyDied;
    }

    private void OnEnemyDied(GameObject obj)
    {
        if (enemies.Contains(obj))
        {
            enemies.Remove(obj);
        }
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnCooldown)
        {
            _spawnTimer = 0;
            SpawnIteration();
        }
    }

    private void SpawnIteration()
    {
        if (enemies.Count >= enemyCap)
        {
            var farEnemy = enemies
                .OrderByDescending(x => 
                    Mathf.Abs((_playerController.transform.GetChild(0).position - x.transform.position).sqrMagnitude))
                .First();
            
            if((farEnemy.transform.position - GameManager.Instance.playerController.transform.position).sqrMagnitude < 49)
                return;

            enemies.Remove(farEnemy);
            Destroy(farEnemy);
        }

        var spawnDirection = Random.insideUnitCircle.normalized;
        var spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        var spawnPosition = (Vector2)_playerController.transform.position + spawnDirection * spawnDistance;

        Debug.Log($"Spawn: {spawnDirection}-{spawnDistance}-{spawnPosition}");
        var playerDetailCount = _playerController.GetComponent<Detail>().AllDetails.Count;
        var enemyToSpawn = enemiesWithDifficulties
            .Where(x => x.difficulty >= playerDetailCount - 2 && x.difficulty <= playerDetailCount)
            .Shuffle()
            .FirstOrDefault();

        if (enemyToSpawn != null)
        {
            var spawnedEnemy = Instantiate(enemyToSpawn.enemyPrefab, spawnPosition, Quaternion.identity);
            enemies.Add(spawnedEnemy);
        }
    }
}