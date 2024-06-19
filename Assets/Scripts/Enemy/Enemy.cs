using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Managers")]
    [SerializeField] private EnemyMovement _movement;// Düþman yürüme hareketi
    [SerializeField] private EnemyAnimation _enemyAnimation;// Düþman animasyonu

    [Space(10)]
    [SerializeField] private bool _isEnemyAlive = true;//Düþman canlýlýk durumu
    [SerializeField] private float _health; // Düþman güncel can
    [SerializeField] private float _maxHealth;//Düþmanýn maksimum canýn
    [SerializeField] private float _speed;//Düþmanýn hýzý
    [SerializeField] private int _enemyGoldDie = 2;//Düþmaný öldürdükte sonra ödül miktarý

    [SerializeField] private SpriteRenderer _healthImage;// Düþman caný ui
    [SerializeField] private Transform _priceAnimObject;//Düþman öldükten sonra altýn miktarý yazýsý
    
    [SerializeField] public PoolObjectType poolType;// Düþman, havuz sistemindeki tipi

    private float _healthTemp;
    private Vector3 _defaultPos;

    public bool IsEnemyAlive { get { return _isEnemyAlive; } }
    public int EnemyGoldDie { get { return _enemyGoldDie; } set { _enemyGoldDie = value; } }
    public EnemyMovement EnemyMovement { get { return _movement; } }

    

    private void Start()
    {
        _isEnemyAlive = true;
        _health = _maxHealth;
        _healthTemp = _health;
        _defaultPos = _priceAnimObject.localPosition;
    }

    /// <summary>
    /// Düþmanýn hasar almasý
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (_isEnemyAlive == false) return;
        _health -= damage;
       
        GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.Hit);
        StartCoroutine(StartHealtAnimation());
    }

    /// <summary>
    /// Hasar aldýktan sonra ui'ýn animasyonu
    /// animation after take damage
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartHealtAnimation()
    {
        EnemyHealthControl();
        float time = 0;
        float elapsedTime = 1f;

        while (time < elapsedTime)
        {
            time += Time.deltaTime;
            _healthTemp = Mathf.Lerp(_healthTemp, _health, time / elapsedTime);
            float imgSizeX = Extensions.UnitIntervalRange(0, _maxHealth, 0, 1, _healthTemp);
            Vector2 size = _healthImage.size;
            size.x = imgSizeX;
            _healthImage.size = size;
            yield return null;
        }
        _healthTemp = _health;

    }

    /// <summary>
    /// Enemy health control
    /// </summary>
    private void EnemyHealthControl()
    {
        if (_health <= 0)
        {
            Debug.Log("Enemy Dead!");
            _isEnemyAlive = false;
            EnemyManager.EnemyCollideEvent.Invoke(EnemyCollideType.Die, this);
            _enemyAnimation.DeathAnim();
            _movement.IsWalking = false;
            _healthImage.gameObject.SetActive(false);
            GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.Death);
            StartCoroutine(DestroyEnemy());
        }
    }

    /// <summary>
    /// Price animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyEnemy()
    {
        float time = 0.5f;
        float elapsedTime = 0;
        Vector3 endPos = _priceAnimObject.transform.localPosition;
        endPos.y += 0.25f;
        _priceAnimObject.gameObject.SetActive(true);
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            _priceAnimObject.localPosition = Vector3.MoveTowards(_priceAnimObject.transform.localPosition,endPos, elapsedTime / time);
            yield return null;
        }

        _priceAnimObject.localPosition = _defaultPos;
        _priceAnimObject.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        PoolManager.Instance.AddObjectFromPool(poolType, gameObject);
    }
    /// <summary>
    /// Düþman collider etkileþimi
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ProjectTile"))
        {
            ProjectTile damageable = collision.GetComponent<ProjectTile>();
            if (damageable != null)
            {
                TakeDamage(damageable.DamageAmount);
                PoolManager.Instance.AddObjectFromPool(damageable.poolType, damageable.gameObject);
            }
        }
        if (collision.CompareTag("Finish"))
        {
            EnemyManager.EnemyCollideEvent.Invoke(EnemyCollideType.Escape, this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Düþman bilgilerinin resetlenmesi
    /// </summary>
    public void ResetEnemy()
    {
        _isEnemyAlive = true;
        _health = _maxHealth;
        _healthTemp = _health;
        _healthImage.size = Vector2.one;
        _healthImage.gameObject.SetActive(true);

    }
}



/// <summary>
/// Hasar alýnabilir 
/// </summary>
public interface IDamageable
{
    public void TakeDamage(int damage);
}