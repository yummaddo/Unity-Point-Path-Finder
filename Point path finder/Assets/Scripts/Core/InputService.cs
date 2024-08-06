using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PointMove.Core.Abstraction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PointMove.Core
{
    public class InputService : AbstractService<InputService>
    {
        [SerializeField] private float daley = 0.015f;
        [SerializeField] private Camera camera;
        
        [SerializeField]  GraphicRaycaster mRaycaster;
        PointerEventData _mPointerEventData;
        [SerializeField] EventSystem mEventSystem;
        
        private bool _pressedClickButtonStatus = false;
        public bool pressedInUI =false;

        public event Action OnClickEnteredVoid;
        public event Action OnClickPrecessedVoid;
        public event Action OnClickCloseVoid;
        protected override void Awake() => _instance = this;
        private void LateUpdate()
        {
            PlayTouch();
        }
        private void PlayTouch()
        {
            pressedInUI = IsPointerOverUIObject();
            if (StartClicked())
            {
                OnClickEnteredVoid?.Invoke();
            }
            else if (ProcessedClicked())
            {
                OnClickPrecessedVoid?.Invoke();
            }
            else if (EndClicked())
            {
                Debug.Log(pressedInUI);
                OnClickCloseVoid?.Invoke();
                _pressedClickButtonStatus = false;
            }
            else
            {
                pressedInUI = false;
            }
        }
        public bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    return true;
                }
            }
            return false;
        }
        private bool StartClicked()
        {
            if (Input.GetButton("Fire1") && !pressedInUI)
            {
                _pressedClickButtonStatus = true;
                return true;
            }
            return false;
        }
        private bool RayCastToUI()
        {
            if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
                Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            {
                return false;
            }

            var myObjectLayer = LayerMask.NameToLayer("UI");
            int layerMask = 1 << myObjectLayer;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000.0f, layerMask))
                if (hit.point != Vector3.zero) return true; 
            return false;
        }
        private bool ProcessedClicked() => Input.GetButton("Fire1") && _pressedClickButtonStatus;
        private bool EndClicked() => !Input.GetButton("Fire1") && _pressedClickButtonStatus;
    }
}