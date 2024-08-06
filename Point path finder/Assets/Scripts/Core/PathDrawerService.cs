using System.Collections.Generic;
using PointMove.Core.Abstraction;
using UnityEngine;

namespace PointMove.Core
{
    public class PathDrawerService : AbstractService<PathDrawerService>
    {
        public GameObject targetPrefab;
        private Dictionary<Vector3,GameObject> _points;
        protected override void Awake()
        {
            _points = new Dictionary<Vector3, GameObject>();
        }

        public void CreatePoint(Vector3 point)
        {
            var obj = Instantiate(targetPrefab, this.transform);
            obj.SetActive(true);
            obj.transform.position = point;
            _points.Add(point, obj);
        }

        public void RemovePoint(Vector3 point)
        {
            Destroy(_points[point]);
            _points.Remove(point);
        }
    }
}