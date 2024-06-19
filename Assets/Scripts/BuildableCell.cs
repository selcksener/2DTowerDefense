using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableCell : MonoBehaviour, IBuildable
{
    [SerializeField] private GameObject _buildObject; // Ýnþa objesinin referansý // tower reference prefab
    [SerializeField] private GameObject _towerObject; // Ýnþa edilen obje
    [SerializeField] private BaseGrid baseGrid; // Baðlý olduðu hücre
    [SerializeField] private int towerID; // Ýnþa edilen kule id'si
    [SerializeField] private PoolObjectType poolType;// inþa edilen obje tipi
    public int TowerID { get { return towerID; } }

    private void Awake()
    {
        baseGrid = GetComponent<BaseGrid>();
    }
    /// <summary>
    /// Ýnþa alanýna göre iþlem yapan kýsým
    /// </summary>
    /// <param name="build"></param>
    public void SetBuild(GameObject build)
    {
        if(_buildObject != null)//Týklanýlan yerde kule var. Upgrade arayüzü gösterilecek // a tower cell, show upgrade ui
        {
            BuildUIMonitor.BuildState.Invoke(BuildClickType.Upgradable, _towerObject);
            _towerObject.GetComponent<Tower>().rangeObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            // Týklanýlan alan boþ. Ýnþa arayüzü gösterilecek
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
    /// <param name="buildObject">inþa edilecek obje</param>
    /// <param name="towerID">Ýnþa edilecek obje id'si</param>
    public void Build(GameObject buildObject,int towerID)
    {
        if (this._buildObject != null) return;
        this._buildObject = buildObject;
        buildObject = PoolManager.Instance.GetObjectFromPool(buildObject.GetComponent<Tower>().poolType); // Havuzdan kule alýnýyor
        buildObject.GetComponent<Tower>().SetTower();// Kule baþlangýç ayarý yapýlýyor
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
