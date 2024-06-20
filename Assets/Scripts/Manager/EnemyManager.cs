using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum EnemyType
{
    Enemy1, Enemy2, Enemy3
}
public enum EnemyCollideType
{
    Escape,
    Die
}
public class EnemyManager : MonoBehaviour
{
    public static Action<EnemyCollideType, Enemy> EnemyCollideEvent;//D��man collider etkile�imi // Enemy collide interaction event

    public List<Enemy> enemies = new List<Enemy>();//Aktif d��man listesi // Active enemies
    public List<Enemy> dieEnemies = new List<Enemy>();// �l� d��man listesi // Gereksiz // Die enemies // Useless

    [SerializeField] private GameObject enemy1Prefab, enemy2Prefab, enemy3Prefab;//D��man tipleri // Enemy types
    [SerializeField] private float timeBetweenEnemySpawn = 2f;// D��man olu�turma zaman aral��� // Time between enemy spawn
    [SerializeField] private int enemyPerSpawn = 0; // olu�turulan d��man say�s� // Count of enemies spawn
    [SerializeField] private int currentWave = 0; // G�ncel dalga bilgisi // Current Wave
    [SerializeField] private List<LevelWaveDate> levelWaveData;// Level dalga bilgisi // Level Wave Info
    [SerializeField] private List<LevelEnemyData> levelEnemyData; // Level d��man bilgisi // Curent wave enemies

    [SerializeField] private List<EnemyType> levelEnemyType = new List<EnemyType>();// Dalgadaki t�m d��manlar tek listede toplan�yor // Collecting all enemies in current wave

    [SerializeField] private Text waveText;// dalga yaz�s� 
    [SerializeField] private Text escapedText;// ka�an d��man yaz�s�
    [SerializeField] private int escapedEnemyCount = 0;//ka�an d��man say�s�
    [SerializeField] private int totalKilled = 0;//toplam �ld�r�len d��man say�s�
    public int TotalKilled {  get { return totalKilled; } }
    public int EscapedEnemy {  get { return escapedEnemyCount; } }

    private bool isSpawnEnemy = false;//D��man olu�turma �art�

    private void Awake()
    {
        EnemyCollideEvent -= EnemyCollideEventListener;
        EnemyCollideEvent += EnemyCollideEventListener;
        GameManager.GameCurrentState -= GameCurrentStateListener;
        GameManager.GameCurrentState += GameCurrentStateListener;
    }

    private void EnemyCollideEventListener(EnemyCollideType _type,Enemy enemy)
    {
        switch(_type)
        {
            case EnemyCollideType.Escape:
                escapedEnemyCount++;
                if(escapedEnemyCount>=10)
                {
                    GameManager.GameCurrentState.Invoke(GameState.Lose,1);
                }
                else
                    RemoveEnemyFromList(enemy);
                break;
            case EnemyCollideType.Die:
                totalKilled++;
                RemoveEnemyFromList(enemy);
                break;
        }
        escapedText.text = escapedEnemyCount.ToString() + "/10";
    }

    private void GameCurrentStateListener(GameState _state,int _data)
    {
        switch(_state)
        {
            case GameState.LevelLoad://Level olu�tuktan d��man bilgilerini resetliyor // Enemy info reset after level load
                ClearEnemy();
                GameManager.GameCurrentState.Invoke(GameState.PlayingGame, -1);
                break;
            case GameState.EnemyLoad:// D��manlar�n olu�mas�n� ba�latan k�s�m // Spawn enemy
                LoadEnemy();
                
                break;
            case GameState.WaveEnded:// Dalga bittikten sonra kontrol eden k�s�m // game status control after wave ended
                currentWave = _data;
                currentWave++;
                if(currentWave >= GameManager.Instance.levelLoaderManager.levelInfo.levelDatas[GameManager.Instance.LevelID].levelWaveData.Count)
                {
                    GameManager.GameCurrentState.Invoke(GameState.Win, -1);
                    CancelInvoke("SpawnEnemyLoop");
                }
                else if (_state != GameState.Lose)
                    GameManager.GameCurrentState.Invoke(GameState.NextWave, currentWave);
                break;
            case GameState.NextWave: // Enemy spawn for next wave
                Invoke("LoadEnemy", 3f);
                break;
            case GameState.Win:
                StopAllCoroutines();
                break;
            case GameState.ResetLevel:
                currentWave = 0;
                ClearEnemy();
                break;
            case GameState.NextLevel:
                currentWave = 0;
                ClearEnemy();
                break;
            case GameState.Lose:
                currentWave = 0;
                ClearEnemy();
                CancelInvoke("SpawnEnemyLoop");
                break;
        }
    }

    /// <summary>
    /// Create Enemy
    /// Only once per wave
    /// </summary>
    public void LoadEnemy()
    {
        if (currentWave >= GameManager.Instance.levelLoaderManager.levelInfo.levelDatas[GameManager.Instance.LevelID].levelWaveData.Count ||GameManager.Instance.GameCurrent == GameState.Lose)
        {
            Debug.Log("Oyun Bitti!");
            return;
        }
      
        enemyPerSpawn = 0;//olu�turulan d��man bilgisi s�f�rlan�yor
        levelWaveData = GameManager.Instance.levelLoaderManager.levelInfo.levelDatas[GameManager.Instance.LevelID].levelWaveData; // level dalga bilgisi al�n�yor // Get level's wave data
        levelEnemyData = levelWaveData[currentWave].enemyDatas;// level dalga d��man bilgisi al�n�yor // Get all enemy data of the current wave
        timeBetweenEnemySpawn = levelWaveData[currentWave].timeBetweenEnemySpawn;// D��man olu�turma zaman aral��� // Time Between enemy spawn
        levelEnemyType = new List<EnemyType>();
        waveText.text = (currentWave+1).ToString() + "/" + levelWaveData.Count.ToString();
        escapedText.text = escapedEnemyCount.ToString() + "/10";
        
        for (int i = 0; i < levelEnemyData.Count; i++) // dalgadaki d��manlar�n hepsi tek listede toplan�yor // collecting all enemy in current wave / for random enemy
        {
            for (int j = 0; j < levelEnemyData[i].enemyCount; j++)
            {
                levelEnemyType.Add(levelEnemyData[i].enemyType);
            }
        }
        levelEnemyType = Extensions.Shuffle<EnemyType>(levelEnemyType);//Dalgadaki d��manlar� rasgele gelmesi i�in kar��t�r�l�yor // for random enemy
        isSpawnEnemy = true;
        CancelInvoke();
        Invoke("SpawnEnemyLoop", timeBetweenEnemySpawn);
    }

    /// <summary>
    /// Create enemy 
    /// </summary>
    private void SpawnEnemyLoop()
    {
        if (isSpawnEnemy == false)
            return;
        if (enemyPerSpawn >= levelEnemyType.Count)//Dalgadaki t�m d��manlar� olu�turulduysa fonksiyon devam etmiyor // if all enemies created
        {
            return;
        }
        GameObject go = null;
        // Get enemy from the pool system
        switch (levelEnemyType[enemyPerSpawn])
        {
            case EnemyType.Enemy1:
                go = PoolManager.Instance.GetObjectFromPool(PoolObjectType.Enemy1,false);
                break;
            case EnemyType.Enemy2:
                go = PoolManager.Instance.GetObjectFromPool(PoolObjectType.Enemy2,false);
                break;
            case EnemyType.Enemy3:
                go = PoolManager.Instance.GetObjectFromPool(PoolObjectType.Enemy3,false);
                break;

        }

        enemyPerSpawn++;
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.ResetEnemy();// D��man bilgisi s�f�rlan�yor // Enemy info reset

        AddEnemyFromList(enemy); // D��man havuzdan siliniyor // Enemy remove from pool

        enemy.EnemyMovement.StartWalking();// D��man hareket ettiriliyor // Enemy walking

        //Yeni d��man olu�turma zaman aral��� azalt�l�yor
        // New time between enemy spawn
        timeBetweenEnemySpawn -= levelWaveData[currentWave].timeDecrease; 
        timeBetweenEnemySpawn = Mathf.Clamp(timeBetweenEnemySpawn, levelWaveData[currentWave].minTimeBetweenEnemySpawn, levelWaveData[currentWave].timeBetweenEnemySpawn);
        Invoke("SpawnEnemyLoop", timeBetweenEnemySpawn);
    }
    /// <summary>
    /// Create enemy added the list
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyFromList(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    /// <summary>
    /// Enemy who dies or escapes is deleted from the list
    /// if list count 0 ve all enemy created, wave ended
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyFromList(Enemy enemy)
    {
        enemies.Remove(enemy);
        dieEnemies.Add(enemy);
        if (enemies.Count <= 0 && enemyPerSpawn >= levelEnemyType.Count)
        {
            StopAllCoroutines();
            isSpawnEnemy = false;
            GameManager.GameCurrentState.Invoke(GameState.WaveEnded,currentWave);
        }
    }

    /// <summary>
    /// Enemy system reset
    /// </summary>
    private void ClearEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            PoolManager.Instance.AddObjectFromPool(enemies[i].poolType,enemies[i].gameObject); 
        }
        for (int i = 0; i < dieEnemies.Count; i++)
        {
            if(dieEnemies[i])
                PoolManager.Instance.AddObjectFromPool(dieEnemies[i].poolType, dieEnemies[i].gameObject);
        }
        CancelInvoke("SpawnEnemyLoop");
        enemies = new List<Enemy>();
        escapedEnemyCount = 0;
    }
}
