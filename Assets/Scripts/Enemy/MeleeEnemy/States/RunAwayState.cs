using UnityEngine;

namespace Enemy.MeleeEnemy.States
{
    public class RunAwayState : IState
    {
        private readonly MeleeEnemy _enemy;
        private readonly PlayerController _playerController;
        private readonly MovementController _movementController;
        private readonly RotationController _rotationController;

        public RunAwayState(MeleeEnemy enemy, PlayerController player, MovementController movementController, RotationController rotationController)
        {
            _enemy = enemy;
            _playerController = player;
            _movementController = movementController;
            _rotationController = rotationController;
        }
        
        public void Tick()
        {
            var direction = -(_playerController.transform.position - _enemy.transform.position).normalized;
            _rotationController.Rotate(direction, _enemy.RotationSpeed);
            if (Vector2.Angle(_enemy.transform.up, direction) < 15)
            {
                _movementController.Move(_enemy.transform.up * _enemy.MovementSpeed);
            }
        }

        public void OnEnter()
        {
            Debug.Log($"Run away: {_playerController.transform}");
        }

        public void OnExit()
        {
        }
    }
}
