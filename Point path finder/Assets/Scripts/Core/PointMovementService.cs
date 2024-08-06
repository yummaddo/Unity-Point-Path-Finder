using System;
using System.Collections.Generic;
using PointMove.Core.Abstraction;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace PointMove.Core
{
    public class PointMovementService : AbstractService<PointMovementService>
    {
        public Point point;
        public RectTransform canvasPlayZone;
        
        private Queue<Vector3> _movementQueue = new Queue<Vector3>();
        private bool _isMoving = false;
        private InputService _inputService;
        [Inject] private PathDrawerService _pathDrawerService;
        [Inject]
        public void Construct(InputService service)
        {
            service.OnClickCloseVoid += ServiceOnOnClickCloseVoid;
            point.OnMovementCompleted += PointOnOnMovementCompleted;
            _inputService = service;
        }

        private void PointOnOnMovementCompleted()
        {
            Debugger.Logger("point OnMovementCompleted", Process.Action);
            try
            {
                _pathDrawerService.RemovePoint(point.TargetToMovement);
            }
            catch (Exception e)
            {
                Debugger.Logger(e.ToString(), Process.TrashHold);
            }

            _isMoving = false;
            ProcessNextMovement();
        }
        private void ServiceOnOnClickCloseVoid()
        {
            if (_inputService.pressedInUI)
            {
                Debugger.Logger("Clicked on UI element, ignoring input.", Process.Info);
                return;
            }
            Debugger.Logger("OnClickCloseVoid", Process.Action);
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    canvasPlayZone,
                    Input.mousePosition,
                    Camera.main,
                    out var worldPoint))
            {
                Debugger.Logger($"point set destination to worldPoint={worldPoint}", Process.Info);
                EnqueueMovement(worldPoint);
            }
        }

        public void ClearPath()
        {
            _movementQueue.Clear();
        }

        private void EnqueueMovement(Vector3 destination)
        {
            _movementQueue.Enqueue(destination);
            _pathDrawerService.CreatePoint(destination);
            if (!_isMoving)
            {
                ProcessNextMovement();
            }
        }

        private void ProcessNextMovement()
        {
            if (_movementQueue.Count > 0)
            {
                var nextDestination = _movementQueue.Dequeue();

                point.SetPointDestination(nextDestination);
                point.Move();
                _isMoving = true;
            }
        }
        protected override void Awake() => _instance = this;
    }
}