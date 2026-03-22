using BouncingBalls;
using System;
using System.Collections.Generic;
using UnityEngine;

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
            return CreateObject<T>(objectType);
    }

    public void SetObject(MonoBehaviour obj, ObjectType objectType)
    {
        Debug.Log(_objectPools.TryGetValue(objectType, out var _));
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
                newQueue.Push(CreateObject(prefabInfo.Prefab));
        }
    }

    private T CreateObject<T>(ObjectType objectType) where T : MonoBehaviour
    {
        MonoBehaviour prefab = null;

        foreach (var prefabInfo in _prefabs)
        {
            if (prefabInfo.ObjectType == objectType)
            {
                prefab = prefabInfo.Prefab;
                break;
            }
        }

        return (T)CreateObject(prefab);
    }

    private MonoBehaviour CreateObject(MonoBehaviour prefab)
    {
        MonoBehaviour obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);
        obj.transform.position = transform.position;

        return obj;
    }
}