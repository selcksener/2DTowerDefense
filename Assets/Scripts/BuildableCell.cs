using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableCell : MonoBehaviour, IBuildable
{
    [SerializeField] private GameObject _buildObject; // �n�a objesinin referans� // tower reference prefab
    [SerializeField] private GameObject _towerObject; // �n�a edilen obje
    [SerializeField] private BaseGrid baseGrid; // Ba�l� oldu�u h�cre
    [SerializeField] private int towerID; // �n�a edilen kule id'si
    [SerializeField] private PoolObjectType poolType;// in�a edilen obje tipi
    public int TowerID { get { return towerID; } }

    private void Awake()
    {
        baseGrid = GetComponent<BaseGrid>();
    }
    /// <summary>
    /// �n�a alan�na g�re i�lem yapan k�s�m
    /// </summary>
    /// <param name="build"></param>
    public void SetBuild(GameObject build)
    {
        if(_buildObject != null)//T�klan�lan yerde kule var. Upgrade aray�z� g�sterilecek // a tower cell, show upgrade ui
        {
            BuildUIMonitor.BuildState.Invoke(BuildClickType.Upgradable, _towerObject);
            _towerObject.GetComponent<Tower>().rangeObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            // T�klan�lan alan bo�. �n�a aray�z� g�sterilecek
            BuildUIMonitor.BuildState.Invoke(BuildClickType.Buildable, _towerObject); // empty cell, show buildable ui
        }
    }
    public void DestroyBuild(GameObject build)
    {
        Destroy(_buildObject);
    }

    /// <summary>
    /// Build tower
    /// </summary>
    /// <param name="buildObject">in�a edilecek obje</param>
    /// <param name="towerID">�n�a edilecek obje id'si</param>
    public void Build(GameObject buildObject,int towerID)
    {
        if (this._buildObject != null) return;
        this._buildObject = buildObject;
        buildObject = PoolManager.Instance.GetObjectFromPool(buildObject.GetComponent<Tower>().poolType); // Havuzdan kule al�n�yor
        buildObject.GetComponent<Tower>().SetTower();// Kule ba�lang�� ayar� yap�l�yor
        buildObject.GetComponent<Tower>().AddBuildFromList();// Kule, sisteme ekleniyor
        buildObject.transform.position = transform.position;
        _towerObject = buildObject;
        this.towerID = towerID;
    }
}

public interface IBuildable
{
    public void SetBuild(GameObject build);
    public void DestroyBuild(GameObject build);
    public void Build(GameObject buildID,int towerID);
}
