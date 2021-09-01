using UnityEngine;

namespace Enemy.MeleeEnemy.States
{
    public class WanderState : IState
    {
        private readonly MeleeEnemy _enemy;
        private readonly PlayerController _playerController;
        private readonly MovementController _movementController;
        private readonly RotationController _rotationController;

        private readonly float _initialRotationCooldown = 2.5f;
        private float _currentRotationCooldown;
        private float _rotationTimer;
        private Vector2 _currentDirection;

        public WanderState(MeleeEnemy enemy, PlayerController player, MovementController movementController, RotationController rotationController)
        {
            _enemy = enemy;
            _playerController = player;
            _movementController = movementController;
            _rotationController = rotationController;
        }
        
        public void Tick()
        {
            _rotationTimer += Time.deltaTime;
            if (_rotationTimer > _currentRotationCooldown)
            {
                _rotationTimer = 0;
                _currentRotationCooldown = Random.Range(_initialRotationCooldown - 2f, _initialRotationCooldown + 2f);
                GetNewDirection();
            }
            
            _rotationController.Rotate(_currentDirection, _enemy.RotationSpeed);
            if (Vector2.Angle(_enemy.transform.up, _currentDirection) < 15)
            {
                _movementController.Move(_enemy.transform.up * _enemy.MovementSpeed);
            }
        }

        public void OnEnter()
        {
            GetNewDirection();
            Debug.Log($"Wander: {_currentDirection}");
        }

        public void OnExit()
        {
        }

        private void GetNewDirection()
        {
            _currentDirection = Random.insideUnitCircle;
        }
    }
}
