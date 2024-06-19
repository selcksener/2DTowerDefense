using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance {  get { return instance; } }
    [SerializeField] private AudioClip _arrow;
    [SerializeField] private AudioClip _rock;
    [SerializeField] private AudioClip _fireball;
    [SerializeField] private AudioClip _hit;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _gameOver;
    [SerializeField] private AudioClip _level;
    [SerializeField] private AudioClip _newGame;
    [SerializeField] private AudioClip _towerBuilt;

    public AudioClip Arrow {  get { return _arrow; }  }
    public AudioClip Rock {  get { return _rock; }  }
    public AudioClip Fireball {  get { return _fireball; }  }
    public AudioClip Hit {  get { return _hit; }  }
    public AudioClip Death {  get { return _death; }  }
    public AudioClip GameOver {  get { return _gameOver; } }
    public AudioClip Level {  get { return _level; } }
    public AudioClip NewGame {  get { return _newGame; } }
    public AudioClip TowerBuilt {  get { return _towerBuilt; } }

    private void Awake()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
    }
}
