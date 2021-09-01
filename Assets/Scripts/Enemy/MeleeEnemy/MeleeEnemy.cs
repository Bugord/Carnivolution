using System;
using System.Linq;
using Enemy.MeleeEnemy.States;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

namespace Enemy.MeleeEnemy
{
    public class MeleeEnemy : BaseEnemy
    {
        private Transform _meatTransform;
        public Transform MeatTransform => _meatTransform;

        void Start()
        {
            _stateMachine = new StateMachine();
            _sqrVisionRadius = _visionRadius * _visionRadius;


            var movementController = GetComponent<MovementController>();
            var rotationController = GetComponent<RotationController>();

            if (GameManager.Instance.playerController == null)
            {
                return;
            }

            var wanderState = new WanderState(this, GameManager.Instance.playerController, movementController,
                rotationController);
            var chaseState = new ChaseState(this, GameManager.Instance.playerController, movementController,
                rotationController);
            var eatingState = new EatingState(this, movementController, rotationController);
            var runAwayState = new RunAwayState(this, GameManager.Instance.playerController, movementController,
                rotationController);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

            At(wanderState, chaseState, SeePlayer);
            At(eatingState, chaseState, SeePlayer);
            At(eatingState, wanderState, () => !SeeFood());
            At(chaseState, wanderState, () => !SeePlayer());
            At(wanderState, eatingState, SeeFood);

            _stateMachine.SetState(wanderState);
        }

        private bool SeePlayer()
        {
            var distanceToPlayer = (GameManager.Instance.playerController.transform.position - transform.position)
                .sqrMagnitude;
            return distanceToPlayer < _sqrVisionRadius;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private bool SeeFood()
        {
            if (_meatTransform != null)
                return true;

            var randomMeat = Physics2D
                .OverlapCircleAll(transform.position, 20, 1 << 10)
                .Shuffle()
                .FirstOrDefault();

            if (randomMeat != null)
            {
                _meatTransform = randomMeat.transform;
            }

            return randomMeat != null;
        }
    }
}