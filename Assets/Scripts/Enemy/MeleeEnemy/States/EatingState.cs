using System.Linq;
using UnityEngine;

namespace Enemy.MeleeEnemy.States
{
    public class EatingState : IState
    {
        private readonly MeleeEnemy _enemy;
        private readonly MovementController _movementController;
        private readonly RotationController _rotationController;
        
        public EatingState(MeleeEnemy enemy, MovementController movementController,
            RotationController rotationController)
        {
            _enemy = enemy;
            _movementController = movementController;
            _rotationController = rotationController;
        }

        public void Tick()
        {
            var direction = (_enemy.MeatTransform.position - _enemy.transform.position).normalized;
            _rotationController.Rotate(direction, _enemy.RotationSpeed);
            if (Vector2.Angle(_enemy.transform.up, direction) < 15)
            {
                _movementController.Move(_enemy.transform.up * _enemy.MovementSpeed);
            }
        }

        public void OnEnter()
        {
            Debug.Log($"Eating: {_enemy.MeatTransform}");
        }

        public void OnExit()
        {
        }
    }
}