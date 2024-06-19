using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemyWaypointInfo = new List<GameObject>();//Level de d��manlar�n gidece�i konum bilgisi // The path that enemies will follow
    [SerializeField] private List<BaseGrid> levelCells = new List<BaseGrid>();// Leveli s�f�rlamak i�in level h�creleri saklan�yor // For level system reset

    public List<GameObject> EnemyWaypointInfo { get { return _enemyWaypointInfo; } }

    private void Awake()
    {
        GameManager.GameCurrentState -= GameCurrentStateListener;
        GameManager.GameCurrentState += GameCurrentStateListener;
    }

    private void GameCurrentStateListener(GameState _state,int _data)
    {
        switch(_state)
        {
            case GameState.LevelLoad:// Level y�klenmesini ba�latan k�s�m // Starts loading level
                ClearLevel();
                LevelLoad();
                GameManager.GameCurrentState.Invoke(GameState.WaveCountDown, -1);
                break;
            case GameState.NextLevel:
                ClearLevel();
                break;
            case GameState.ResetLevel:
                ClearLevel();
                break;
        }
    }

    /// <summary>
    /// level se�ildikten sonra levelin y�klenmesini sa�layan fonksiyon
    /// Level loading after level selected
    /// </summary>
    public void LevelLoad()
    {
        if(GameManager.Instance.levelLoaderManager.levelInfo.levelDatas.Count<=0)
        {
            Debug.Log("There are no levels !");
            return;
        }

        LevelData levelData = GameManager.Instance.levelLoaderManager.levelInfo.levelDatas[GameManager.Instance.LevelID];

        for(int i =0;i<levelData.levelPathInfo.Count;i++)// Levelin yolu y�kleniyor // loading level's path
        {
            GameObject go = GameManager.Instance.gridManager.gridObjects[levelData.levelPathInfo[i]];
            go.GetComponent<BaseGrid>().Init(levelData.levelPathInfo[i], CellType.PathCell,levelData.levelPathInfo);
            levelCells.Add(go.GetComponent<BaseGrid>());
        }
        for(int i =0;i<levelData.levelBorderInfo.Count;i++)// Level s�n�rlar� y�kleniyor // loading level's border
        {
            GameObject go = GameManager.Instance.gridManager.gridObjects[levelData.levelBorderInfo[i]];
            go.GetComponent<BaseGrid>().Init(levelData.levelBorderInfo[i], CellType.WallCell, levelData.levelBorderInfo);
            levelCells.Add(go.GetComponent<BaseGrid>());
        }
        for (int i = 0; i < levelData.levelBorderCornerInfo.Count; i++)//Levelin s�n�rlar�n�n 4 k��esi y�kleniyor // loading level's 4 corner
        {
            GameObject go = GameManager.Instance.gridManager.gridObjects[levelData.levelBorderCornerInfo[i]];
            go.GetComponent<BaseGrid>().Init(levelData.levelBorderCornerInfo[i], CellType.WallCornerCell, levelData.levelBorderCornerInfo);
            levelCells.Add(go.GetComponent<BaseGrid>());
        }
        for (int i = 0; i < levelData.levelBuildableInfo.Count; i++)// �n�a edilebilir h�creler y�kleniyor // loading level's buildable cells
        {
            GameObject go = GameManager.Instance.gridManager.gridObjects[levelData.levelBuildableInfo[i]];
            go.GetComponent<BaseGrid>().Init(levelData.levelBuildableInfo[i], CellType.BuildableCell, levelData.levelBuildableInfo,"Buildable");
            levelCells.Add(go.GetComponent<BaseGrid>());
        }
        for (int i = 0; i < levelData.levelWaypointInfo.Count; i++)//D��manlar�n gidece�i yol bilgisi y�kleniyor // loading level's waypoint 
        {
            GameObject go = GameManager.Instance.gridManager.gridObjects[levelData.levelWaypointInfo[i]];
            _enemyWaypointInfo.Add(go);
            go.GetComponent<BaseGrid>().Init(levelData.levelWaypointInfo[i], CellType.PathCell, levelData.levelPathInfo);
            levelCells.Add(go.GetComponent<BaseGrid>());
        }
        for (int i = 0; i < levelData.levelEnvironmentData.Count; i++)//Levelin �evre bilgisi y�kleniyor // loading level's environment 
        {
            GameObject go = GameManager.Instance.gridManager.gridObjects[levelData.levelEnvironmentData[i].LevelEnvironementValue];
            go.GetComponent<BaseGrid>().Initenvironment(levelData.levelEnvironmentData[i]);
            levelCells.Add(go.GetComponent<BaseGrid>());
        }

        //D��manlar�n ba�lang�� konumu y�kleniyor // Enemy's start position
        GameObject enemyStart = GameManager.Instance.gridManager.gridObjects[levelData.enemyStartInfo];
        enemyStart.GetComponent<BaseGrid>().Init(levelData.enemyStartInfo, CellType.EnemyStartCell, new List<Vector2>() { levelData.enemyStartInfo});
        levelCells.Add(enemyStart.GetComponent<BaseGrid>());

        //D��manlar�n biti� konumu y�kleniyor // Enemy's end position
        GameObject enemyEnd = GameManager.Instance.gridManager.gridObjects[levelData.enemyEndInfo];
        enemyEnd.GetComponent<BaseGrid>().Init(levelData.enemyEndInfo, CellType.EnemyEndCell, new List<Vector2>() { levelData.enemyEndInfo });
        levelCells.Add(enemyEnd.GetComponent<BaseGrid>());
    }

    /// <summary>
    /// level system reset
    /// </summary>
    public void ClearLevel()
    {
        for (int i = 0; i < levelCells.Count; i++)
        {
            levelCells[i].ClearCell();
        }
    }
}
