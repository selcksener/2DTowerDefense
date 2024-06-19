using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseGrid : MonoBehaviour
{
    [SerializeField] private CellType _cellType;// Hücre tipi // cell type
    [SerializeField] private Vector2 _gridIndex;// X-Y bilgisi // cell x,y info
    [SerializeField] private List<Vector2> neighbour = new List<Vector2>(); // Hücrenin komþu hücre bilgileri // cell's neighbour info
    [SerializeField] private List<Vector2> infoList = new List<Vector2>();// Komuþu hücre için kontrol bilgileri // cell's neighbour control info
    [SerializeField] private SpriteRenderer _childObject; // Hücrenin resmi için kullanýlan deðiþken // cell image
    [SerializeField] private GameObject finishObject; // düþmanlarýn kaçacaðý obje // enemy escape object


    public Vector2 GridIndex { get { return _gridIndex; } }



    /// <summary>
    /// Cell update
    /// </summary>
    /// <param name="index">Cell info(x-y)</param>
    /// <param name="type">Cell type</param>
    /// <param name="info">Info</param>
    /// <param name="tag">Cell tag</param>
    public void Init(Vector2 index,CellType type,List<Vector2> info,string tag="BaseGrid")
    {
        _gridIndex = index;
        _cellType = type;
        gameObject.tag = tag;

        if(type != CellType.EnemyWaypointCell)// Waypoint için deðiþken deðiþmemeli // variable should  not change for waypoint
            infoList = info;
        if(_cellType == CellType.PathCell)// Hücre yolsa diðer komþularý kontrol ediliyor // if cell is path , control neighnour
        {
            ControlNeighbour();
        }
        else if(_cellType != CellType.BaseCell)
            _childObject.sprite = GameManager.Instance.levelLoaderManager.GetGridSprite(neighbour,_cellType);// Hücre varsayýlan deðilse sprite'ý deðiþiyor // if cell isn't base cell, change sprite
        if(_cellType == CellType.BuildableCell)
        {
            BuildableCell cell = gameObject.AddComponent<BuildableCell>();// Hücre build edilebilirse eðer script ekleniyor // if cell is buildable add script
        }
        finishObject.SetActive(_cellType == CellType.EnemyEndCell);// Hücre, düþmanlarýn kaçacaðý hücreyse obje aktif oluyor. // if cell is enemy escape object, object activated
    }
    /// <summary>
    /// Hücre çevre objeleri hücresiyse bu fonksiyon çalýþýyor
    /// This method call if the cell is environment
    /// </summary>
    /// <param name="data"></param>
    public void Initenvironment(LevelEnvironmentData data)
    {
        _childObject.sprite = GameManager.Instance.levelLoaderManager.GetGridSprite(data.LevelEnvironmentKey);
    }
   
   
    /// <summary>
    /// Yol hücresi, diðer komþu hücreleri kontrol edip sprite'ýný deðiþtiriyor
    /// Path cell controls neighobur and change sprite
    /// </summary>
    public void ControlNeighbour()
    {
        List<Vector2> pathInfo = infoList;
        neighbour = new List<Vector2>();
        for (int i = 0; i < GameManager.Instance.levelLoaderManager.controlGridInfos.Count; i++)
        {
            var item = GameManager.Instance.levelLoaderManager.controlGridInfos[i];
            Vector2 newGrid = _gridIndex + GameManager.Instance.levelLoaderManager.controlGridInfos[i].index;
         
            if(pathInfo.Contains(newGrid))
            {
                neighbour.Add(GameManager.Instance.levelLoaderManager.controlGridInfos[i].index);
            }
          
        }
        _childObject.sprite =  GameManager.Instance.levelLoaderManager.GetGridSprite(neighbour, _cellType);
    }
    /// <summary>
    /// Cell clear for reset
    /// </summary>
    public void ClearCell()
    {
        _cellType = CellType.BaseCell;
        gameObject.tag = "BaseGrid";
        _childObject.sprite = GameManager.Instance.levelLoaderManager.GetGridSprite(neighbour, _cellType);

        BuildableCell cell = gameObject.GetComponent<BuildableCell>();
        if (cell != null)
            Destroy(cell);
        finishObject.SetActive(false);
    }
}

