using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instant = null;

    public static T Instant => instant;

    protected virtual void Awake()
    {
        if (instant == null)
        {
            instant = GetComponent<T>();
            return;
        }

        if (instant.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }
}
