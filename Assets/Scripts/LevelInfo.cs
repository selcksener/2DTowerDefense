using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelInfo",menuName =" Level")]
public class LevelInfo : ScriptableObject
{
    //T�m levellerin bilgisinin sakland��� de�i�ken
    public List<LevelData> levelDatas = new List<LevelData>();
}

[System.Serializable]
public class LevelData
{
    
    public int levelID;// Level ID'si
    
    public List<Vector2> levelBorderInfo = new List<Vector2>(); // Levelin s�n�rlar�n�n bilgisi // Level border info
    
    public List<Vector2> levelBorderCornerInfo = new List<Vector2>(); // Level s�n�rlar�n�n 4 k��esi // level 4 corner
    
    public List<Vector2> levelPathInfo = new List<Vector2>();// Levelin yol g�zergah� // level path
    
    public List<Vector2> levelBuildableInfo = new List<Vector2>();//Levelin in�a edilebilir alanlar�n bilgisi // level buildable info
   
    public List<Vector2> levelWaypointInfo = new List<Vector2>();//D��manlar�n gidece�i yol bilgisi // enemy waypoint
    public List<LevelEnvironmentData> levelEnvironmentData = new List<LevelEnvironmentData>();//Levelin �evre objelerinin bilgisi // level environment
    public Vector2 enemyStartInfo;//D��man�n ba�layaca�� konum
    public Vector2 enemyEndInfo;  //D��man�n ka�aca�� konum(Son konum)
    public Vector2 centerGridInfo;//Levelin orta noktas�
    public List<LevelWaveDate> levelWaveData = new List<LevelWaveDate>();//Levelin d��man dalga bilgisi
}


/// <summary>
/// Levellerin dalga bilgilerini tutan s�n�f
/// </summary>
[System.Serializable]
public class LevelWaveDate
{
    public float timeBetweenEnemySpawn=1f;//D��manlar�n olu�aca�� zaman aral���
    public float timeDecrease = 0.1f; // Her d��mandan sonra zaman aral���n�n d��me miktar�
    public float minTimeBetweenEnemySpawn = 0.5f;// Minimum d��man�n olu�aca�� zaman aral���
    public List<LevelEnemyData> enemyDatas = new List<LevelEnemyData>();// Dalgalardaki d��man bilgisi
}

/// <summary>
/// Levellerdeki dalgalarda d��man bilgileri
/// </summary>
[System.Serializable]
public class LevelEnemyData
{
    public EnemyType enemyType;//D��man tipi
    public int enemyCount;//Olu�turulacak d��man say�s�
}

//Levellerdeki �evre objelerinin bilgisi
[System.Serializable]
public class LevelEnvironmentData
{
    public string LevelEnvironmentKey ;//Objenin e�siz id'si
    public Vector2 LevelEnvironementValue;// objenin konum bilgisi

   public LevelEnvironmentData(string k,Vector2 v)
    {
        LevelEnvironmentKey = k;
        LevelEnvironementValue = v;
    }
}