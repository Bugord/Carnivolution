using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        protected StateMachine _stateMachine;

        [SerializeField] protected float _movementSpeedMultiplyer = 1f;
        [SerializeField] protected float _rotationSpeedMultiplyer = 1f;
        [SerializeField] protected float _visionRadiusMultiplyer = 1f;
        [SerializeField] protected List<PartScript> parts;

        [SerializeField] private int difficulty;
        public int Difficulty => difficulty;

        protected float _movementSpeed;
        protected float _rotationSpeed;
        protected float _visionRadius;
        protected float _sqrVisionRadius;

        public float RotationSpeed => _rotationSpeed;
        public float MovementSpeed => _movementSpeed;

        private void Awake()
        {
            UpdateStats();
            GlobalEventManager.DetailAdded += detail =>
            {
                AddPart(detail.GetComponent<PartScript>());
            };
            GlobalEventManager.DetailDestroyed += detail =>
            {
                RemovePart(detail.GetComponent<PartScript>());
            };
        }
        
        private void OnValidate()
        {
            _sqrVisionRadius = _visionRadius * _visionRadius;
            UpdateStats();
        }
        
        private void UpdateStats()
        {
            _movementSpeed = parts.Sum(p => p.stats.MovementSpeed) * _movementSpeedMultiplyer;
            _rotationSpeed = parts.Sum(p => p.stats.RotationSpeed) * _rotationSpeedMultiplyer;
            _visionRadius = parts.Sum(p => p.stats.VisionRadius) * _visionRadiusMultiplyer;
        }
        
        public void AddPart(PartScript part)
        {
            parts.Add(part);
            UpdateStats();
        }

        public void RemovePart(PartScript part)
        {
            parts.Remove(part);
            UpdateStats();
        }
    }
}