using PointMove.Core.Abstraction;
using UnityEngine;

namespace PointMove.Boot
{
    public class SessionCore : AbstractService<SessionCore>
    {
        public float diversity = 3f;
        public AnimationCurve curve;
        protected override void Awake()
        {
            _instance = this;
        }
    }
}
