using PointMove.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PointMove
{
    public class UIPointController : MonoBehaviour
    {
        [Inject] private PointMovementService _pointMovementService;

        public Slider sliderSpeed;
        public Point point;
        public Button clearMove;
        public TextMeshProUGUI textMeshProUGUI;
        public void OnValueChange()
        {
            textMeshProUGUI.text = sliderSpeed.value.ToString("F1");
            point.SetSpeed(sliderSpeed.value);
        }
        public void OnButtonClear()
        {
            _pointMovementService.ClearPath();
        }
    }
}