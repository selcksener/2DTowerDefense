using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelInfo",menuName =" Level")]
public class LevelInfo : ScriptableObject
{
    //Tüm levellerin bilgisinin saklandýðý deðiþken
    public List<LevelData> levelDatas = new List<LevelData>();
}

[System.Serializable]
public class LevelData
{
    
    public int levelID;// Level ID'si
    
    public List<Vector2> levelBorderInfo = new List<Vector2>(); // Levelin sýnýrlarýnýn bilgisi // Level border info
    
    public List<Vector2> levelBorderCornerInfo = new List<Vector2>(); // Level sýnýrlarýnýn 4 köþesi // level 4 corner
    
    public List<Vector2> levelPathInfo = new List<Vector2>();// Levelin yol güzergahý // level path
    
    public List<Vector2> levelBuildableInfo = new List<Vector2>();//Levelin inþa edilebilir alanlarýn bilgisi // level buildable info
   
    public List<Vector2> levelWaypointInfo = new List<Vector2>();//Düþmanlarýn gideceði yol bilgisi // enemy waypoint
    public List<LevelEnvironmentData> levelEnvironmentData = new List<LevelEnvironmentData>();//Levelin çevre objelerinin bilgisi // level environment
    public Vector2 enemyStartInfo;//Düþmanýn baþlayacaðý konum
    public Vector2 enemyEndInfo;  //Düþmanýn kaçacaðý konum(Son konum)
    public Vector2 centerGridInfo;//Levelin orta noktasý
    public List<LevelWaveDate> levelWaveData = new List<LevelWaveDate>();//Levelin düþman dalga bilgisi
}


/// <summary>
/// Levellerin dalga bilgilerini tutan sýnýf
/// </summary>
[System.Serializable]
public class LevelWaveDate
{
    public float timeBetweenEnemySpawn=1f;//Düþmanlarýn oluþacaðý zaman aralýðý
    public float timeDecrease = 0.1f; // Her düþmandan sonra zaman aralýðýnýn düþme miktarý
    public float minTimeBetweenEnemySpawn = 0.5f;// Minimum düþmanýn oluþacaðý zaman aralýðý
    public List<LevelEnemyData> enemyDatas = new List<LevelEnemyData>();// Dalgalardaki düþman bilgisi
}

/// <summary>
/// Levellerdeki dalgalarda düþman bilgileri
/// </summary>
[System.Serializable]
public class LevelEnemyData
{
    public EnemyType enemyType;//Düþman tipi
    public int enemyCount;//Oluþturulacak düþman sayýsý
}

//Levellerdeki çevre objelerinin bilgisi
[System.Serializable]
public class LevelEnvironmentData
{
    public string LevelEnvironmentKey ;//Objenin eþsiz id'si
    public Vector2 LevelEnvironementValue;// objenin konum bilgisi

   public LevelEnvironmentData(string k,Vector2 v)
    {
        LevelEnvironmentKey = k;
        LevelEnvironementValue = v;
    }
}