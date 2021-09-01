using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeatSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> meatPrefabs;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnTimer;
    private float _timer = 0;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_timer < spawnTimer)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = 0;
            Vector2 spawnAreaCenter = _camera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
            var pos = spawnAreaCenter + Random.insideUnitCircle.normalized * spawnRadius;
            Instantiate(meatPrefabs.OrderBy(x => Guid.NewGuid()).FirstOrDefault(), pos, Quaternion.identity);
        }
    }

    private void OnEnable()
    {
        GlobalEventManager.DetailDestroyed += OnDetailDestroy;
    }
    
    private void OnDisable()
    {
        GlobalEventManager.DetailDestroyed -= OnDetailDestroy;
    }

    private void OnDetailDestroy(Detail detail)
    {
        Vector2 spawnAreaCenter = detail.transform.position;
        for (int i = 0; i < Random.Range(2, 3); i++)
        {
            var pos = spawnAreaCenter + Random.insideUnitCircle.normalized * 0.5f;
            Instantiate(meatPrefabs.OrderBy(x => Guid.NewGuid()).FirstOrDefault(), pos, Quaternion.identity);
        }
    }
}
