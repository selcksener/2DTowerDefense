using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{

    [SerializeField] private TowerButton selectedTowerButton;// �n�a aray�z�nde se�ili kule 
    [SerializeField] private SpriteRenderer spriteRenderer; // 
    [SerializeField] private bool isDrag = false;
    [SerializeField] private Vector3 lastMousePosition;
    [SerializeField] private int gold; // Oyun paras�
    public int Gold { get { return gold; } set { gold = value; } }


    [SerializeField] private Text goldText;
    [SerializeField] private Button speedButton;
    
    public List<Tower> buildTower = new List<Tower>();

    private void Awake()
    {
        EnemyManager.EnemyCollideEvent -= EnemyCollideEventListener;
        EnemyManager.EnemyCollideEvent += EnemyCollideEventListener;

        GameManager.GameCurrentState -= GameCurrentStateListener;
        GameManager.GameCurrentState += GameCurrentStateListener;
    }
    private void GameCurrentStateListener(GameState _state,int _data)
    {
        switch(_state)
        {
            case GameState.LevelLoad:
                ClearBuildObject();
                break;
            case GameState.NextLevel:
                ClearBuildObject();
                break;
            case GameState.ResetLevel:
                ClearBuildObject();
                break;
        }
    }

    private void Update()
    {

        if (isDrag == false && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)// �n�a etmek i�in mouse konumu kontrol ediliyor
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                IBuildable buildable = hit.collider.GetComponent<IBuildable>();
                if (buildable != null)
                    buildable.SetBuild(hit.collider.gameObject);
                else
                    BuildUIMonitor.BuildState.Invoke(BuildClickType.None,null);
            }
            //else
              //  BuildUIMonitor.BuildState.Invoke(BuildClickType.None,null);
        }
        else if (isDrag && Input.GetMouseButton(0))// �n�a aray�z�nde se�ili kulenin mouse ' u takip etmesi
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            lastMousePosition = Input.mousePosition;
            spriteRenderer.transform.position = pos;
            if (spriteRenderer.gameObject.activeInHierarchy == false)
            {
                spriteRenderer.gameObject.SetActive(true);
            }
            lastMousePosition = pos;
        }
        goldText.text = gold.ToString();
    }



    #region TOWER
    /// <summary>
    /// Aray�zde se�ilen kule
    /// </summary>
    /// <param name="towerButton"></param>
    public void TowerSelected(TowerButton towerButton)
    {
        selectedTowerButton = towerButton;
        DragSpriteSetup();
    }
    /// <summary>
    /// Aray�zde se�ilen kulenin s�r�klenmesi
    /// </summary>
    /// <param name="towerButton"></param>
    public void TowerDrag(TowerButton towerButton)
    {
        isDrag = true;
    }

    /// <summary>
    /// S�r�kleme i�leminin bitmesi
    /// </summary>
    /// <param name="towerButton"></param>
    public void TowerDeSelected(TowerButton towerButton)
    {
        Debug.Log("Kule uygun konum i�in kontrol ediliyor... ");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            IBuildable buildable = hit.collider.GetComponent<IBuildable>();
            if (buildable != null)
            {
                if (towerButton.TowerPrice <= Gold)
                {
                    Gold -= towerButton.TowerPrice;
                    Debug.Log("Build");
                    buildable.Build(selectedTowerButton.TowerObject,selectedTowerButton.TowerID);
                    
                }
                else
                {

                }
            }
        }
        selectedTowerButton = null;
        spriteRenderer.gameObject.SetActive(false);
        isDrag = false;
    }

    /// <summary>
    /// Aray�zde se�ilen kule, s�r�kleme g�rseline atan�yor
    /// </summary>
    private void DragSpriteSetup()
    {
        spriteRenderer.sprite = selectedTowerButton.DragSprite;
        isDrag = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void BuildTower()
    {
        isDrag = false;
        spriteRenderer.gameObject.SetActive(false);
    }
    
    #endregion



    private void EnemyCollideEventListener(EnemyCollideType type,Enemy enemy)
    {
        switch(type)
        {
            case EnemyCollideType.Die:
                Gold += enemy.EnemyGoldDie;
                break;
        }
    }
    /// <summary>
    /// Oyunu h�zland�ran k�s�m
    /// </summary>
    public void SpeedButton()
    {
        Time.timeScale = Time.timeScale == 1 ? 2 : 1;
        speedButton.GetComponent<Image>().color = Time.timeScale == 1 ? Color.white : Color.green;
    }


    /// <summary>
    /// �n�a sistemini s�f�rlayan k�s�m
    /// </summary>
    private void ClearBuildObject()
    {
        for(int i =0;i<buildTower.Count;i++)
        {
            buildTower[i].TowerLevel = 1;
            PoolManager.Instance.AddObjectFromPool(buildTower[i].poolType, buildTower[i].gameObject);
        }
        buildTower = new List<Tower>();
    }
}
