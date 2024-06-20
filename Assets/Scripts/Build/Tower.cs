using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private int towerID;
    [SerializeField] private int towerLevel = 1;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private float attackRange;

    [SerializeField] private float attackCounter;
    [SerializeField] private Enemy targetEnemy;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private PoolObjectType projectTileType;
    [SerializeField] public PoolObjectType poolType;

    [SerializeField] private ProjectTile projectTile;
    [SerializeField] private List<Enemy> enemyInRange = new List<Enemy>();
    [SerializeField]public GameObject rangeObject;

    public int TowerID { get { return towerID; } set { towerID = value; } }
    public int TowerLevel { get { return towerLevel; } set { towerLevel = value; } }
    private List<Enemy> EnemyInRange { get { return enemyInRange; } set { enemyInRange = value; } }


    private void Awake()
    {
        EnemyManager.EnemyCollideEvent -= EnemyCollideEventListener;
        EnemyManager.EnemyCollideEvent += EnemyCollideEventListener;
        GameManager.GameCurrentState -= GameCurrentStateListener;
        GameManager.GameCurrentState += GameCurrentStateListener;

    }

    private void GameCurrentStateListener(GameState state, int data)
    {
        switch(state)
        {
            case GameState.NextWave:
                enemyInRange = new List<Enemy>();
                targetEnemy = null;
                break;
        }
    }

    /// <summary>
    /// Kule in�a edildikten sonra listeye kay�t ediliyor
    /// </summary>
    public void AddBuildFromList()
    {
        GameManager.Instance.buildManager.buildTower.Add(this);
    }
    /// <summary>
    /// Kule ilk kez in�a oldu�unda gerekli g�ncellemeler yap�l�yor
    /// </summary>
    public void SetTower()
    {
        GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.TowerBuilt);
        var info = GameManager.Instance.buildUIManager.TowerUpgradeData.towerLevelData[towerID - 1].towerLevelUpgradeInfo[towerLevel-1];
        attackDamage = info.towerAttackDamage;
        attackRange = info.towerAttackRange;
        timeBetweenAttack = info.towerTimeBetweenAttack;
        UpgradeTower();
        BuildUIMonitor.BuildState.Invoke( BuildClickType.None,gameObject);
      
    }

    /// <summary>
    /// Kulenin d��man bulmas� ve ate� etmesi i�lemleri yap�l�yor
    /// </summary>
    private void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsEnemyAlive == false)
        {
            Enemy closed = GetNearestEnemy();
            if(closed != null && closed.IsEnemyAlive && Vector2.Distance(transform.position,closed.gameObject.transform.position)<=Mathf.Max(rangeObject.transform.localScale.x, rangeObject.transform.localScale.y))
            {
                targetEnemy = closed;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;
                Attack();
                attackCounter = timeBetweenAttack;
            }
            else
                isAttacking = false;
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) > Mathf.Max(rangeObject.transform.localScale.x, rangeObject.transform.localScale.y))
            {
                targetEnemy = null;
            }
        }
      
    }

    /// <summary>
    /// Kulenin ate� etmesini sa�layan k�s�m
    /// </summary>
    private void Attack()
    {
        isAttacking = false;
        ProjectTile project = PoolManager.Instance.GetObjectFromPool(projectTileType).GetComponent<ProjectTile>();
        project.FollowTarget(transform,targetEnemy,attackDamage);
    }

    /// <summary>
    /// Kulenin y�kseltme i�lemleri
    /// </summary>
    public void UpgradeTower()
    {
        var info = GameManager.Instance.buildUIManager.TowerUpgradeData.towerLevelData[towerID - 1].towerLevelUpgradeInfo[towerLevel-1];
        attackDamage = info.towerAttackDamage;
        attackRange = info.towerAttackRange;
        timeBetweenAttack = info.towerTimeBetweenAttack;
        rangeObject.transform.localScale = new Vector3(attackRange,attackRange,attackRange);
    }
    /// <summary>
    /// Kule menziline giren en yak�n d��man
    /// </summary>
    /// <returns></returns>
    private Enemy GetNearestEnemy()
    {

        Enemy closedEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        for(int i =0;i< enemyInRange.Count;i++)
        {
            float distance = Vector2.Distance(transform.position, enemyInRange[i].transform.position);
            if (distance<smallestDistance)
            {
                distance = smallestDistance;
                closedEnemy = enemyInRange[i];
            }
        }
        return closedEnemy;
    }

    /// <summary>
    /// D��man�n collider etkile�imi
    /// </summary>
    /// <param name="type"></param>
    /// <param name="enemy"></param>
    private void EnemyCollideEventListener(EnemyCollideType type, Enemy enemy)
    {
        switch(type)
        {
            case EnemyCollideType.Escape:
                if (enemyInRange.Contains(enemy))
                    enemyInRange.Remove(enemy);
                break;

            case EnemyCollideType.Die:
               if( enemyInRange.Contains(enemy))
                    enemyInRange.Remove(enemy);
                break;
        }
    }
    //�len-ka�an d��man menzildeyse siliniyor
    public void AddorRemoveEnemyInList(Enemy enemy,bool add)
    {
        if (add)
        {
            enemyInRange.Add(enemy);
        }
        else
        {
            if (enemyInRange.Contains(enemy))
            {
                if (targetEnemy == enemy)
                    targetEnemy = null;
                enemyInRange.Remove(enemy);
         
            }
        }
    }


}
