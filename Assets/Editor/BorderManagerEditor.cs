using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(LevelGenerator))]
public class BorderManagerEditor : Editor
{
    LevelGenerator levelGenerator;
    //BorderManager borderManager;
    private void OnSceneGUI()
    {
        Event e = Event.current;
        levelGenerator = (LevelGenerator)target;
        //borderManager = levelGenerator.borderManager;
        if (e!=null)
        {
            if(e.isMouse && e.shift && e.type == EventType.MouseDown)
            {
                Debug.Log("Triggered ");
                switch(levelGenerator.levelGeneratorType)
                {
                    case LevelGeneratorType.WallState:
                        WallState();
                        break;
                    case LevelGeneratorType.PathState:
                        PathState();
                        break;
                    case LevelGeneratorType.EnemyStartState:
                        EnemyStartState();
                        break;
                    case LevelGeneratorType.EnemyEndState:
                        EnemyEndState();
                        break;
                    case LevelGeneratorType.EnemyWaypointState:
                        EnemyWaypointState();
                        break;
                    case LevelGeneratorType.WallCornerState:
                        WallCornerState();
                        break;
                    case LevelGeneratorType.BuildableState:
                        BuildableState();
                        break;
                    case LevelGeneratorType.EnvironmentState:
                        EnvironmentState();
                        break;
                }
               
            }
            if (levelGenerator)
                Selection.activeGameObject = levelGenerator.gameObject;
        }

    }
      public override void OnInspectorGUI()
      {
          base.OnInspectorGUI();
          serializedObject.Update();
          EditorGUILayout.BeginVertical(GUI.skin.box);

          EditorGUILayout.EndVertical();
         if(GUILayout.Button("ResetData"))
          {
              levelGenerator.ResetLevelInfo();
              EditorUtility.SetDirty(this);
          }
         if(GUILayout.Button("Save Level"))
          {
              levelGenerator.SaveCurrentLevelGenerator();
              EditorUtility.SetDirty(this);
          }
      }

    #region STATES
    private void WallState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.baseGridTag))
        {

           ChangeGridInfo(hit2d, LevelGeneratorType.WallState);
            hit2d.collider.tag = levelGenerator.borderGridTag;
            levelGenerator.levelData.levelBorderInfo.Add(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.borderGridTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.BaseState);
            hit2d.collider.tag = levelGenerator.baseGridTag;
            levelGenerator.levelData.levelBorderInfo.Remove(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
    }
    private void WallCornerState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.borderGridTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.WallCornerState);
            hit2d.collider.tag = levelGenerator.borderCornerGridTag;
            levelGenerator.levelData.levelBorderCornerInfo.Add(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.borderCornerGridTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.WallState);
            hit2d.collider.tag = levelGenerator.borderGridTag;
            levelGenerator.levelData.levelBorderCornerInfo.Remove(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
    }
    private void PathState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.baseGridTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.PathState);
            hit2d.collider.tag = levelGenerator.pathTag;
            levelGenerator.levelData.levelPathInfo.Add(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.pathTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.BaseState);
            hit2d.collider.tag = levelGenerator.baseGridTag;
            levelGenerator.levelData.levelPathInfo.Remove(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
    }
    private void EnemyStartState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.baseGridTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.EnemyStartState);
            hit2d.collider.tag = levelGenerator.enemyStartTag;
            levelGenerator.levelData.enemyStartInfo = hit2d.collider.GetComponent<BaseGrid>().GridIndex;
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.enemyStartTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.BaseState);
            hit2d.collider.tag = levelGenerator.baseGridTag;
            levelGenerator.levelData.enemyStartInfo = new Vector2(-1, -1);

        }
    }
    private void EnemyEndState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.baseGridTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.EnemyEndState);
            hit2d.collider.tag = levelGenerator.enemyEndTag;
            levelGenerator.levelData.enemyEndInfo = hit2d.collider.GetComponent<BaseGrid>().GridIndex;
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.enemyEndTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.BaseState);
            hit2d.collider.tag = levelGenerator.baseGridTag;
            levelGenerator.levelData.enemyEndInfo = new Vector2(-1, -1);

        }
    }
    private void EnemyWaypointState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && (hit2d.collider.CompareTag(levelGenerator.pathTag) || hit2d.collider.CompareTag(levelGenerator.enemyEndTag)))
        {
            Debug.Log("Collider " + hit2d.collider.tag + "  " + hit2d.collider.name);
            hit2d.collider.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
          
            hit2d.collider.tag = levelGenerator.enemyWaypointTag;
            levelGenerator.levelData.levelWaypointInfo.Add(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.enemyWaypointTag))
        {
           
            hit2d.collider.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            hit2d.collider.tag = hit2d.collider.tag == levelGenerator.pathTag ?  levelGenerator.pathTag : levelGenerator.enemyEndTag;
            levelGenerator.levelData.levelWaypointInfo.Remove(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
    }

    private void BuildableState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.baseGridTag))
        {

            ChangeGridInfo(hit2d, LevelGeneratorType.BuildableState);
            hit2d.collider.tag = levelGenerator.BuildableTag;
            levelGenerator.levelData.levelBuildableInfo.Add(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.BuildableTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.BaseState);
            hit2d.collider.tag = levelGenerator.baseGridTag;
            levelGenerator.levelData.levelBuildableInfo.Remove(hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
    }
    private void EnvironmentState()
    {
        RaycastHit2D hit2d = GetRayCastHit2D();
        if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.baseGridTag))
        {

            ChangeGridInfo(hit2d, LevelGeneratorType.EnvironmentState);
            hit2d.collider.tag = levelGenerator.EnvironmentTag;
            levelGenerator.levelData.levelEnvironmentData.Add(new LevelEnvironmentData(levelGenerator.environmentName, hit2d.collider.GetComponent<BaseGrid>().GridIndex) );
        }
        else if (hit2d.collider != null && hit2d.collider.CompareTag(levelGenerator.EnvironmentTag))
        {
            ChangeGridInfo(hit2d, LevelGeneratorType.BaseState);
            hit2d.collider.tag = levelGenerator.baseGridTag;
            levelGenerator.levelData.levelEnvironmentData.RemoveAll((x)=>x.LevelEnvironementValue == hit2d.collider.GetComponent<BaseGrid>().GridIndex);
        }
    }

    #endregion
    private RaycastHit2D GetRayCastHit2D()
    {
        Ray hit = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast(hit.origin, Vector2.zero);
        return hit2d;
    }
    public void ChangeGridInfo(RaycastHit2D hit2D, LevelGeneratorType _type)
    {
        if (_type != LevelGeneratorType.EnvironmentState)
        {
            hit2D.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = levelGenerator.borderManager.levelValue[levelGenerator.borderManager.levelKey.IndexOf(_type)].image;
            hit2D.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().color = levelGenerator.borderManager.levelValue[levelGenerator.borderManager.levelKey.IndexOf(_type)].color;
        }
        else
        {
            hit2D.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
                levelGenerator.borderManager.levelEnvironmentReference.First(x => x.key == levelGenerator.environmentName).image;
            //levelGenerator.borderManager.levelEnvironmentReference.Where(x=>x.key == levelGenerator.environmentName).Select(y=>y.image);
            hit2D.collider.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
        //hit2D.collider.GetComponent<SpriteRenderer>().sprite = levelGenerator.levelValue[levelGenerator.levelKey.IndexOf(_type)].image;
        //hit2D.collider.GetComponent<SpriteRenderer>().color = levelGenerator.levelValue[levelGenerator.levelKey.IndexOf(_type)].color;
    }
}
