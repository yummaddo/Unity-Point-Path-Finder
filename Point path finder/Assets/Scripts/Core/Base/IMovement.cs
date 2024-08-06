using System;
using UnityEngine;

namespace PointMove.Core.Base
{
    internal interface IMovement
    {
        public Vector3 Destination { get; }
        public Action OnMovementCompleted { get; set; }
        public float Speed { get; }
        public bool Available { get; }
        public void SetMovementDestination(Vector3 pointer);
        public void SetMovementSpeed(float speed);
        public void StopMovement();
        public void StartMovement();
    }
}