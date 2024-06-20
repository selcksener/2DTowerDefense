using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


//Level oluşturma sistemi için
public enum LevelGeneratorType
{
    BaseState,
    WallState,
    WallCornerState,
    PathState,
    EnemyWaypointState,
    EnemyStartState,
    EnemyEndState,
    BuildableState,
    EnvironmentState
}
//Levelin yüklenmesi için
public enum CellType
{
    BaseCell,
    WallCell,
    WallCornerCell,
    PathCell,
    EnemyWaypointCell,
    EnemyStartCell,
    EnemyEndCell,
    BuildableCell,
    EnvironmentCell
}

/// <summary>
/// Komşu Hücre bilgisi
/// </summary>
[System.Serializable]
public struct ControlGrid
{
    public Vector2 index;
    public bool findChild;
}
public class LevelLoaderManager : MonoBehaviour
{

    public LevelInfo levelInfo;// Level bilgisini saklayan

    [Header("Level Key-Value")]
    public List<LevelGeneratorReference> levelGeneratorReferenceList = new List<LevelGeneratorReference>();
    public List<LevelLoadEnvironmentReference> levelEnvironmentReference = new List<LevelLoadEnvironmentReference>();
    public Dictionary<LevelGeneratorType, Sprite> levelGeneratorInfo = new Dictionary<LevelGeneratorType, Sprite>();

    [Header("Lists")] public List<ControlGrid> controlGridInfos = new List<ControlGrid>();// Level yolları için komşu kontrol bilgisi
    public List<Vector2> pathInfo = new List<Vector2>();//Level yol bilgisi
    public List<LevelLoadReference> levelLoadReference = new List<LevelLoadReference>(); // Levelle ilgili objelerin görsel referansları



    private void Awake()
    {
        for (int i = 0; i < levelGeneratorReferenceList.Count; i++)
        {
            levelGeneratorInfo.Add(levelGeneratorReferenceList[i].type, levelGeneratorReferenceList[i].image);
        }
       
    }


    /// <summary>
    /// İstenen hücrenin görselinin değişmesi
    /// </summary>
    /// <param name="neighbour"> yol hücreleri için </param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Sprite GetGridSprite(List<Vector2> neighbour, CellType type)
    {
        if (type == CellType.PathCell)
        {
            int count = 0;

            for (int j = 0; j < levelLoadReference.Count; j++)
            {
                for (int i = 0; i < neighbour.Count; i++)
                {
                    if (levelLoadReference[j].neighbourIndex.All((neighbour.Contains)) && levelLoadReference[j].neighbourIndex.Count == neighbour.Count)

                        return levelLoadReference[j].image;

                }
            }
        }
        else
        {
            for (int j = 0; j < levelLoadReference.Count; j++)
            {

                if (levelLoadReference[j].cellType == type)
                    return levelLoadReference[j].image;
            }

        }
        return null;
    }

    /// <summary>
    /// Level çevre objelerinin referansları
    /// </summary>
    /// <param name="_key"></param>
    /// <returns></returns>
    public Sprite GetGridSprite(string _key)
    {

        for (int j = 0; j < levelEnvironmentReference.Count; j++)
        {

            if (levelEnvironmentReference[j].key == _key)
                return levelEnvironmentReference[j].image;
        }


        return null;
    }
}



[System.Serializable]
public class LevelGeneratorReference
{
    public LevelGeneratorType type;
    public Sprite image;
}

[System.Serializable]
public class LevelLoadReference
{
    public List<Vector2> neighbourIndex;
    public CellType cellType;
    public Sprite image;
}
[System.Serializable]
public class LevelLoadEnvironmentReference
{
    public string key;
    public Sprite image;
}


[System.Serializable]
public class LevelGeneratorReferenceDict : UnitySerializedDictionary<LevelGeneratorType, LevelGeneratorReference> { }