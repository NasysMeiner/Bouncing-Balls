using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BouncingBalls
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;

        [SerializeField] private List<PoolInfo> _prefabs = new();

        private Dictionary<ObjectType, Stack<MonoBehaviour>> _objectPools;

        public static PoolManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitPoolManager();
        }

        public T GetObject<T>(ObjectType objectType) where T : MonoBehaviour
        {
            Stack<MonoBehaviour> stackObj = _objectPools[objectType];

            if (stackObj.Count != 0)
                return (T)stackObj.Pop();
            else
                return CreateObject<T>(_prefabs.FirstOrDefault(poolInfo => poolInfo.ObjectType == objectType));
        }

        public void SetObject(MonoBehaviour obj, ObjectType objectType)
        {
            if (_objectPools.TryGetValue(objectType, out var stack))
            {
                obj.gameObject.SetActive(false);
                obj.transform.position = transform.position;
                stack.Push(obj);
            }
        }

        private void InitPoolManager()
        {
            _objectPools = new Dictionary<ObjectType, Stack<MonoBehaviour>>(_prefabs.Count);

            foreach (var prefabInfo in _prefabs)
            {
                if (prefabInfo.Prefab == null)
                    return;

                Stack<MonoBehaviour> newQueue = new();
                _objectPools[prefabInfo.ObjectType] = newQueue;

                for (int i = 0; i < prefabInfo.Count; i++)
                    newQueue.Push(CreateObject(prefabInfo.Prefab, prefabInfo.Parent ? prefabInfo.Parent : transform));
            }
        }

        private T CreateObject<T>(PoolInfo poolInfo) where T : MonoBehaviour
        {
            PoolInfo prefabInfoObject = null;

            foreach (var prefabInfo in _prefabs)
            {
                if (prefabInfo.ObjectType == poolInfo.ObjectType)
                {
                    prefabInfoObject = prefabInfo;
                    break;
                }
            }

            return (T)CreateObject(prefabInfoObject.Prefab, prefabInfoObject.Parent ? prefabInfoObject.Parent : transform);
        }

        private MonoBehaviour CreateObject(MonoBehaviour prefab, Transform parent)
        {
            MonoBehaviour obj = Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            obj.transform.position = transform.position;

            return obj;
        }
    }
}