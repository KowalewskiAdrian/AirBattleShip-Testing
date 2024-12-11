using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolBase : MonoBehaviour 
{
    private GameObject _objectBase;

    private int _totalCount;
    private List<GameObject> _pooledObjects;


    void Start() 
    {
        if (_pooledObjects == null)
            _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _totalCount; i++)
        {
            GameObject clonedObject;
            clonedObject = Instantiate(_objectBase, transform);
            clonedObject.SetActive(false);
            _pooledObjects.Add(clonedObject);
        }
    }

    public void SetPoolingAmount(GameObject baseObject, int amount)
    {
        _objectBase = baseObject;
        _totalCount = amount;

        _pooledObjects = new List<GameObject>();
    }

    public GameObject GetPooledObject() 
    {
        if (_pooledObjects == null)
            _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        GameObject clonedObject;
        clonedObject = Instantiate(_objectBase, transform);
        clonedObject.SetActive(false);
        _pooledObjects.Add(clonedObject);

        return clonedObject;
    }
}
