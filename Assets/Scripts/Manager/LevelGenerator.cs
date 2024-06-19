using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    [Header("Tag Names ")] public string baseGridTag = "BaseGrid";
    public string borderGridTag = "BorderGrid";
    public string borderCornerGridTag = "BorderCorner";
    public string pathTag = "Path";
    public string enemyStartTag = "EnemyStart";
    public string enemyEndTag = "EnemyEndTag";
    public string enemyWaypointTag = "EnemyWaypoint";
    public string BuildableTag = "Buildable";
    public string EnvironmentTag = "Environment";

    [SerializeField] public string environmentName;
    public LevelLoaderManager borderManager;
    public LevelGeneratorType levelGeneratorType = LevelGeneratorType.WallState;
    public LevelData levelData;
    public void ResetLevelInfo()
    {
        levelData = new LevelData();

    }

    public void SaveCurrentLevelGenerator()
    {
        borderManager.levelInfo.levelDatas.Add(levelData);
        return; levelData.centerGridInfo = new Vector2((levelData.levelBorderCornerInfo[0].x + levelData.levelBorderCornerInfo[1].x) / 2,
          (levelData.levelBorderCornerInfo[0].y + levelData.levelBorderCornerInfo[3].y) / 2
          );

    }
}
