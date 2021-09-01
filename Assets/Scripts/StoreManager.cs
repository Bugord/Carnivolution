using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> pickupsPrefabs;
    [SerializeField] private List<Transform> slotTransforms;
    [SerializeField] private RectTransform storeCanvas;
    private Camera _camera;

    private List<GameObject> _spawnedItems;

    private void Awake()
    {
        _spawnedItems = new List<GameObject>();
        _camera = Camera.main;
        
        
        
    }

    private void OnEnable()
    {
        ProgressManager.ScoreToGainDetailReached += SpawnPickups;
        
        // StoreUI.StoreOpened += SpawnPickups;
        StoreUI.StoreClosed += DestroyPickups;
    }

    private void OnDisable()
    {
        // StoreUI.StoreOpened -= SpawnPickups;
        ProgressManager.ScoreToGainDetailReached -= SpawnPickups;
        StoreUI.StoreOpened -= DestroyPickups;
    }

    private void SpawnPickups()
    {
        _spawnedItems.Clear();
        
        var randomPrefabs = pickupsPrefabs.Shuffle().Take(3).ToList();
        for (int i = 0; i < slotTransforms.Count; i++)
        {
            var spawnedPickup = Instantiate(randomPrefabs[i], slotTransforms[i].position, Quaternion.identity, slotTransforms[i]);
            var scale = new Vector3(1f / storeCanvas.localScale.x, 1f/storeCanvas.localScale.y, 1);
            
            spawnedPickup.transform.localScale = scale;
            _spawnedItems.Add(spawnedPickup);
        }
    }

    private void DestroyPickups()
    {
        foreach (var spawnedItem in _spawnedItems)
        {
            Destroy(spawnedItem);
        }
    }
}