
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{
    public GameObject Prefab;

    private Stack<GameObject> pool = new Stack<GameObject>();
    
    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            var clone =Instantiate(Prefab, this.transform);
            return clone;
        }
        
        var poppedObj = pool.Pop();
        poppedObj.SetActive(true);
        return poppedObj;
    }

    public void Put(GameObject prefabClone)
    {
        if (prefabClone == null) return;
        
        prefabClone.SetActive(false);
        pool.Push(prefabClone);
    }
}
