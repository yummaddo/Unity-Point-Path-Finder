using System;
using PointMove.Core.Base;
using PointMove.Core.Realization;
using UnityEngine;
using Zenject;

namespace PointMove.Core
{
    public class Point : MonoBehaviour
    {
        [field: SerializeField] private float Speed { get; set; } = 0.2f;
        internal Vector3 TargetToMovement;
        internal event Action OnMovementCompleted;
        private IMovement _movement;
        private void Awake()
        {
            _movement = new Movement(Speed, this.transform);
            _movement.OnMovementCompleted += MovementCompleted;
        }
        private void MovementCompleted() => OnMovementCompleted?.Invoke();
        public void Start() => _movement.StartMovement();
        public void SetPointDestination(Vector3 destination)
        {
            TargetToMovement = destination;
            _movement.SetMovementDestination(destination);
        }
        public void SetSpeed(float speed) => _movement.SetMovementSpeed(speed);
        public void Move() => _movement.StartMovement();
        public void Stop() => _movement.StopMovement();
    }
}