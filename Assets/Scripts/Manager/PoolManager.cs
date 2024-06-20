using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager : Singleton<PoolManager>
{

    public Dictionary<PoolObjectType, List<GameObject>> objectPools = new Dictionary<PoolObjectType, List<GameObject>>();// Havuzdaki objeler // Objects in the pool
    [SerializeField] private List<PoolReference> poolObjectReference = new List<PoolReference>();
    public Dictionary<PoolObjectType, GameObject> poolObjectPrefab = new Dictionary<PoolObjectType, GameObject>();// Havuza eklenecek objelerin prefablarý // Object reference prefab

    
    [SerializeField] private Transform objectParent;
    [SerializeField] public Dictionary<PoolObjectType, int> _initialCount = new Dictionary<PoolObjectType, int>(); // 


    public override void Awake()
    {
        base.Awake();
        foreach(var item in poolObjectReference)
        {
            poolObjectPrefab.Add(item.poolObjectType,item.poolObjectPrefab);
            objectPools.Add(item.poolObjectType, new List<GameObject>()); 
        }
    }
    /// <summary>
    /// objects added pool 
    /// 
    /// </summary>
    private void Start()
    {
        foreach (var item in _initialCount)
        {
            for (int i = 0; i < item.Value; i++)
            {
                InstantiateObject(item.Key);
            }
        }
    }

    /// <summary>
    /// Havuza obje eklenip, obje kapatýlýyor
    /// Object added pool and object deactive
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="go"></param>
    public void AddObjectFromPool(PoolObjectType _type, GameObject go)
    {
        objectPools[_type].Add(go);
        go?.SetActive(false);
    }
    /// <summary>
    /// Havuzdan obje silinip obje aktif ediliyor
    /// Object remove pool and activate
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="go"></param>
    public void RemoveObjectFromPool(PoolObjectType _type, GameObject go,bool isActive)
    {
        objectPools[_type].Remove(go);
        go?.SetActive(isActive);
    }


    /// <summary>
    /// Havuzdan obje isteniyor
    /// Get object from pool
    /// </summary>
    /// <param name="_type">Objenin tipi</param>
    /// <returns></returns>
    public GameObject GetObjectFromPool(PoolObjectType _type,bool isActive=true)
    {
        GameObject go = null;
        if (objectPools[_type].Count <= 0)
        {
            InstantiateObject(_type);
        }
        go = objectPools[_type][0];
        RemoveObjectFromPool(_type, go,isActive);
        return go;
    }

    /// <summary>
    /// Havuzdan obje yoksa oluþturulup havuza ekleniyor
    /// object create and added pool if pool empty
    /// </summary>
    /// <param name="_type"></param>
    public void InstantiateObject(PoolObjectType _type)
    {
        GameObject go = Instantiate(poolObjectPrefab[_type], objectParent);
        AddObjectFromPool(_type, go);
    }
}

public enum PoolObjectType
{
    Enemy1,
    Enemy2,
    Enemy3,
    Tower1,
    Tower2,
    Tower3,
    Arrow,
    Rock,
    Fireball
}

[System.Serializable]
public class PoolReference
{
    public PoolObjectType poolObjectType;
    public GameObject poolObjectPrefab;
}