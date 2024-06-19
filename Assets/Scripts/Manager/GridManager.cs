using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject _gridPrefab; // Grid prefab 
    [SerializeField] private int _width, _height;

    [SerializeField] private List<GameObject> grid = new List<GameObject>();
    [SerializeField] private Transform gridParent;
    [SerializeField] private Camera cam;

    private float gridSize;
    private Vector3 cameraBorder;
    private GameObject centerTileOfGrid;

    public Dictionary<Vector2, GameObject> gridObjects = new Dictionary<Vector2, GameObject>();// Grid bilgisine karþýlýk gelen objeyi tutuyor
    private void Start()
    {
        gridSize = _gridPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        GetBorder();
        GenerateGrid();
    }

   

    /// <summary>
    /// Oyun baþladýðýnda grid sistemini oluþturan kýsým
    /// </summary>
    private void GenerateGrid()
    {
        float posX = 0;
        float posY = 0;
        for(int y =0;y<_height;y++)
        {
            for (int x = 0; x < _width; x++)
            {
                GameObject go = Instantiate(_gridPrefab, new Vector3(posX, posY, 0), Quaternion.identity,gridParent);
                BaseGrid baseGrid = go.GetComponent<BaseGrid>();
                baseGrid.Init(new Vector2(x, y),CellType.BaseCell,null);
                go.name = "Grid_" + x + "," + y;
                posX += gridSize;
                grid.Add(go);
                gridObjects.Add(new Vector2(x, y), go);
                if (x == (_width / 2) && y == (_height / 2))
                    centerTileOfGrid = go;
              
            }
            posX = 0;
            posY += gridSize;
        }
        Vector3 camPos = centerTileOfGrid.transform.position;
        camPos.z = -10;
        cam.transform.position = camPos;
    }



    #region OLD SYSTEM
    private void GridGenerator()
    {
        float posX = borderX.x + gridSize * 0.5f;
        float posY = borderY.y + gridSize * 0.5f;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {

                GameObject go = Instantiate(_gridPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
                go.name = "Grid_" + x + "," + y;
                posX += gridSize;
                grid.Add(go);
                if (x == (_width / 2) && y == (_height / 2))
                    centerTileOfGrid = go;
            }
            posX = borderX.x + gridSize * 0.5f;
            posY += gridSize;
        }

        centerTileOfGrid.GetComponent<SpriteRenderer>().color = Color.red;
        //cam.transform.position = centerTileOfGrid.transform.position;
        CalculateCameraBorder();
    }

    public void CalculateCameraBorder()
    {
        float camAspect = (Screen.width / Screen.height);
        float camHeight = cam.orthographicSize / 2;
        cameraBorder = new Vector3(camHeight*camAspect,camHeight,0);
        GetBorder();
    }

    public Vector2 screen;
    public Vector2 screenBounds;
    public Vector2 borderX, borderY;
    public void GetBorder()
    {
        screen = new Vector2(Screen.width, Screen.height);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));


        var upperLeftScreen = new Vector3(0, Screen.height, 0);
        var upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
        var lowerLeftScreen = new Vector3(0, 0, 0);
        var lowerRightScreen = new Vector3(Screen.width, 0, 0);

        //Corner locations in world coordinates
        var uLeft = GetComponent<Camera>().ScreenToWorldPoint(upperLeftScreen);
        var lLeft = GetComponent<Camera>().ScreenToWorldPoint(lowerLeftScreen);
        Debug.Log(uLeft + "   " + lLeft);

        borderX = new Vector2(uLeft.x, Mathf.Abs(lLeft.x));
        borderY = new Vector2(Mathf.Abs(lLeft.y), (lLeft.y));

    }
#endregion
}
