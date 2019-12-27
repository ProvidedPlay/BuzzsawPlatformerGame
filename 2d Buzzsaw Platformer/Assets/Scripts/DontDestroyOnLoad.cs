using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public string objectTag;
    void Awake()
    {
        objectTag = this.tag;
        if (objectTag != "Untagged")
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(objectTag);
            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }
        
    }
}
