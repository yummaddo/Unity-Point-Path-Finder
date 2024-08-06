using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PointMove.Boot;
using PointMove.Core.Base;
using UnityEngine;

namespace PointMove.Core.Realization
{
    public class Movement : IMovement
    {
        public static float MovementDeltaDisposition = 0.05f;
        private Vector3 _destination = Vector3.zero;
        private Vector3 _startPointPosition = Vector3.zero;
        private float _speed = 0;
        private bool _available = false;
        private bool _runed = false;
        private CancellationToken _token;
        private Transform _target;
        private float _time = 0;
        public Movement(float speed, Transform target) => MovementInit(speed, target, target.gameObject.GetCancellationTokenOnDestroy());
        public Movement(float speed, Transform target , CancellationToken token) => MovementInit(speed, target, token);
        private void MovementInit(float speed, Transform target , CancellationToken token)
        {
            _token = token;
            _speed = speed;
            _target = target;
            var position = target.position;
            _destination = position;
            _startPointPosition = position;
            Process().Forget();
        }

        private async UniTask Process()
        {
            Debugger.Logger($"Process of movement init", PointMove.Process.Initial);
            while (!_token.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => this.Available && _runed, cancellationToken: _token);
                await UpdateProcessCall();
            }
        }

        private async UniTask UpdateProcessCall()
        {
            Vector3 VectorDestinationCurrent(float timeDelta1, Vector3 pointTarget1)
            {
                var speedCurrent = timeDelta1 * _speed;
                return Vector3.ClampMagnitude((_destination - pointTarget1), speedCurrent);
            }

            var timeDelta = Time.deltaTime;
            var targetPosition = _target.position;

            var vectorDestinationCurrent = VectorDestinationCurrent(timeDelta, targetPosition);
            var newPosition = targetPosition + vectorDestinationCurrent;
            if (Vector3.Distance(newPosition, _destination) < MovementDeltaDisposition)
            {
                _target.position = _destination;
                StopMovement();
                OnMovementCompleted?.Invoke();
                Debugger.Logger($"OnMovementCompleted", PointMove.Process.Action);
            }
            else
                _target.position = newPosition;

            // Debugger.Logger($"{e.ToString()}", PointMove.Process.TrashHold);
            _time += Time.deltaTime;
            await UniTask.Yield(_token);
        }

        public void SetMovementDestination(Vector3 pointer)
        {
            _runed = true;
            _destination = pointer;
            _startPointPosition = _target.position;
            _time = 0;
            Debugger.Logger($"Update SetMovementDestination", PointMove.Process.Update);
        }

        public void SetMovementSpeed(float speed) => _speed = speed;
        public void StopMovement() => _available = false;
        public void StartMovement() => _available = true;
        
        public bool Available => _available;
        public Vector3 Destination => _destination;
        public Action OnMovementCompleted { get; set; }
        public float Speed => _speed;
    }
}