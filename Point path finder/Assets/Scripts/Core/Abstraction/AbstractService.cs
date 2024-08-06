using UnityEngine;

namespace PointMove.Core.Abstraction
{
    public abstract class AbstractService<TService> : MonoBehaviour
     where TService : MonoBehaviour
    {
        protected static TService _instance;
        internal static TService Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<TService>();
                    _instance.name = _instance.GetType().ToString();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        protected abstract void Awake();
        protected virtual void Start()
        {
            
        }
    }
}