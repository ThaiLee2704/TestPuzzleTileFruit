using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    Dictionary<GameObject, List<GameObject>> poolingObjects = new Dictionary<GameObject, List<GameObject>>();

    public GameObject GetObject(GameObject prefab, Transform parent)
    {
        if (!poolingObjects.TryGetValue(prefab, out List<GameObject> prefabsPool))
        {
            prefabsPool = new List<GameObject>();
            poolingObjects.Add(prefab, prefabsPool);
        }

        foreach (GameObject o in prefabsPool)
        {
            if (o.activeSelf)
                continue;
            o.transform.SetParent(parent);
            return o;
        }

        GameObject newObject = Instantiate(prefab, parent); //spawn prefabs trong GameObject cha trong Hierarchy cho dễ nhìn
        newObject.SetActive(false);

        prefabsPool.Add(newObject);

        return newObject;
    }
}