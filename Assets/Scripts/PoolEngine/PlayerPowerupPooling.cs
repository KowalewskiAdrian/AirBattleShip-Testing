using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPowerupPooling : MonoBehaviour
{
    private List<GameObject> _pooledObjects;

    public static PlayerPowerupPooling SharedInstance;
    public PowerupConfig Config;


    void Awake() 
    {
        SharedInstance = this;
        _pooledObjects = new List<GameObject>();
    }

    void Start()
    {
        if (Config.PowerUpPrefabList == null)
            return;

        if (_pooledObjects == null)
            _pooledObjects = new List<GameObject>();

        for (int i = 0; i < Config.PowerUpPrefabList.Count; i++)
        {
            if (Config.PowerUpPrefabList[i] == null)
                continue;

            if (Config.PowerUpPrefabList[i].GetComponent<Powerup>() == null)
                continue;

            GameObject clonedObject;
            clonedObject = Instantiate(Config.PowerUpPrefabList[i], transform);
            clonedObject.SetActive(false);
            _pooledObjects.Add(clonedObject);
        }
    }

    public GameObject GetPooledObject()
    {
        if (_pooledObjects == null)
            _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy && _pooledObjects[i].GetComponent<Powerup>() != null)
            {
                return _pooledObjects[i];
            }
        }

        List<Powerup.PowerupType> types = Enum.GetValues(typeof(Powerup.PowerupType)).Cast<Powerup.PowerupType>().ToList();
        GameObject baseObject = GetBaseObject(types[Random.Range(0, types.Count)]);
        if (baseObject == null)
            return null;

        GameObject clonedObject;
        clonedObject = Instantiate(baseObject, transform);
        clonedObject.SetActive(false);
        _pooledObjects.Add(clonedObject);

        return clonedObject;
    }

    public GameObject GetPooledObject(Powerup.PowerupType type)
    {
        if (_pooledObjects == null)
            _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (_pooledObjects[i].activeInHierarchy || _pooledObjects[i].GetComponent<Powerup>() == null)
                continue;

            if (_pooledObjects[i].GetComponent<Powerup>().Type == type)
            {
                return _pooledObjects[i];
            }
        }

        GameObject baseObject = GetBaseObject(type);
        if (baseObject == null)
            return null;

        GameObject clonedObject;
        clonedObject = Instantiate(baseObject, transform);
        clonedObject.SetActive(false);
        _pooledObjects.Add(clonedObject);

        return clonedObject;
    }

    private GameObject GetBaseObject(Powerup.PowerupType type)
    {
        if (Config.PowerUpPrefabList == null)
            return null;

        if (_pooledObjects == null)
            _pooledObjects = new List<GameObject>();

        for (int i = 0; i < Config.PowerUpPrefabList.Count; i++)
        {
            if (Config.PowerUpPrefabList[i] == null)
                continue;

            if(Config.PowerUpPrefabList[i].GetComponent<Powerup>() == null)
                continue;

            if (Config.PowerUpPrefabList[i].GetComponent<Powerup>().Type == type)
            {
                return Config.PowerUpPrefabList[i];
            }
        }

        return null;
    }
}
