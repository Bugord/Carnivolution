using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController playerController;
    public Transform playerParentTransform;
    public ProgressManager progressManager;
    
    private void Awake()
    {
        Instance = this; 
    }
    
    
    
}