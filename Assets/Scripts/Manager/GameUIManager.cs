using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private GameObject _menuMap;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _defeatPanel;

    [Space(10)][Header("Texts")]
    [SerializeField] private Text _GameCurrentText;
    [SerializeField] private Text _countDownText;
    [SerializeField] private Text _winTotalKilledText;
    [SerializeField] private Text _winEscapedEnemyText;

    private float _countDown = 3;

    private void Awake()
    {
        GameManager.GameCurrentState -= GameCurrentStateListener;
        GameManager.GameCurrentState += GameCurrentStateListener;
    }
    //Oyunun güncel durumunu dinleyen kýsým
    private void GameCurrentStateListener(GameState _state, int _data)
    {
        switch (_state)
        {
            case GameState.StartGame://Oyun baþladýðýnda geri sayýmmý baþlatýyor
                _GameCurrentText.text = "StartGame!/" + _data.ToString();
                _countDown = 3;
                break;
            case GameState.LevelLoad:
                _GameCurrentText.text = "LevelLoad!/" + _data.ToString();
                break;

            case GameState.EnemyLoad:
                _GameCurrentText.text = "EnemyLoad!/" + _data.ToString();
                break;

            case GameState.WaveEnded:
                _GameCurrentText.text = "WaveEnded!/" + _data.ToString();

                break;

            case GameState.NextWave:
                _GameCurrentText.text = "NextWave!/" + _data.ToString();

                _countDown = 3;
                StartCoroutine(PlayCountDown(false));
                break;
            case GameState.Win:
                _GameCurrentText.text = "Win!/" + _data.ToString();
                _winEscapedEnemyText.text = GameManager.Instance.enemyManager.EscapedEnemy.ToString() + "/10";
                _winTotalKilledText.text = GameManager.Instance.enemyManager.TotalKilled.ToString();
                _winPanel.SetActive(true);
                break;
            case GameState.Lose:
                _GameCurrentText.text = "Lose!/" + _data.ToString();
                _defeatPanel.SetActive(false);
                break;
            case GameState.PlayingGame:
                _gameUI.SetActive(true);
                break;
            case GameState.WaveCountDown:
                StartCoroutine(PlayCountDown(true));
                break;
        }
    }
    /// <summary>
    /// Geri sayýmý baþlatýyor
    /// Countdown
    /// </summary>
    /// <param name="invoke">if invoke true,  load enemy
    /// 
    /// Bir sonraki dalgada düþman otomatik baþlamasýn diye false gönderiliyor
    /// </param>
    /// <returns></returns>
    private IEnumerator PlayCountDown(bool invoke = true)
    {
        bool isCount = true;
        _countDownText.text = "NEXT WAVE IN " + "\n" + "3";
        _countDownText.transform.parent.gameObject.SetActive(true);
        while (_countDown > 0)
        {
            _countDown -= Time.deltaTime;
            _countDownText.text = "NEXT WAVE IN " + "\n" + ((int)_countDown + 1).ToString();

            yield return null;
        }
        _countDownText.transform.parent.gameObject.SetActive(false);
        if (invoke)
            GameManager.GameCurrentState.Invoke(GameState.EnemyLoad, -1);


    }

    public void StartLevel(int levelID)
    {

        StartCoroutine(GameManager.Instance.StartLevel(levelID - 1, 0f));
        _levelPanel.SetActive(false);
        _menuMap.SetActive(false);
        _menuUI.SetActive(false);
    }

    public void NextLevel()
    {
        GameManager.GameCurrentState.Invoke(GameState.NextLevel, -1);
        ClosePanel();
    }
    public void RestartLevel()
    {
        GameManager.GameCurrentState.Invoke(GameState.ResetLevel, -1);
        ClosePanel();
    }
    private void ClosePanel()
    {
        _levelPanel.SetActive(false);
        _menuMap.SetActive(false);
        _menuUI.SetActive(false);
        _winPanel.SetActive(false);
        _defeatPanel.SetActive(false);
    }
}
