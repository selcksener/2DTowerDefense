using System;
using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{

    public static Action<GameState, int> GameCurrentState;// Oyunun güncel durumu

    [Header("Managers")] public LevelManager levelManager;
    public BuildManager buildManager;
    public EnemyManager enemyManager;
    public LevelLoaderManager levelLoaderManager;
    public BuildUIMonitor buildUIManager;
    public GridManager gridManager;


    [SerializeField] private AudioSource _audio;
    [SerializeField] private int levelID = 0;

    public AudioSource Audio { get { return _audio; } }
    public int LevelID { get { return levelID; } set { levelID = value; } }

    public override void Awake()
    {
        base.Awake();
        GameCurrentState -= GameCurrentStateListener;
        GameCurrentState += GameCurrentStateListener;
    }

    private void GameCurrentStateListener(GameState _state, int _data)
    {
        switch (_state)
        {
            case GameState.StartGame:
                StartCoroutine(StartLevel(_data, 0));
                break;
            case GameState.NextLevel:

                StartCoroutine(StartLevel(this.levelID + 1, 0.5f));
                break;
            case GameState.ResetLevel:
                StartLevel(this.levelID);
                StartCoroutine(StartLevel(this.levelID, 0.5f));
                break;
        }
    }
    /// <summary>
    /// Seçilen levelin baþlamasý
    /// Start selected level 
    /// </summary>
    /// <param name="levelID"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator StartLevel(int levelID, float waitTime = 0.5f)
    {
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.NewGame);
        this.levelID = levelID;
        GameCurrentState.Invoke(GameState.LevelLoad, -1);
    }


}

public enum GameState
{
    StartGame,
    LevelLoad,
    EnemyLoad,
    NextWave,
    WaveEnded,
    Win,
    Lose,
    PlayingGame,
    NextLevel,
    ResetLevel,
    WaveCountDown
}