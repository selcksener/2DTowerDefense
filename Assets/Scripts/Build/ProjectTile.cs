using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private Enemy targetEnemy;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float destroyTimer = 2f;
    [SerializeField] public PoolObjectType poolType;
    [SerializeField] private Collider2D collider;
    public int DamageAmount { get { return damageAmount; } }

    /// <summary>
    /// Takip etme sistemi
    /// Follow enemy
    /// </summary>
    /// <param name="startTransform">Start position</param>
    /// <param name="enemy">Target</param>
    /// <param name="damage">Damage amount</param>
    public void FollowTarget(Transform startTransform,Enemy enemy, int damage)
    {
        
        switch(poolType)
        {
            case PoolObjectType.Arrow:
                GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.Arrow);
                break;
            case PoolObjectType.Rock:
                GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.Rock);
                break;
            case PoolObjectType.Fireball:
                GameManager.Instance.Audio.PlayOneShot(SoundManager.Instance.Fireball);
                break;
        }
        damageAmount = damage;
        startPosition = startTransform;
        transform.localPosition = startPosition.localPosition;
        targetEnemy = enemy;
        collider.enabled = true;
        StartCoroutine(MoveProjectTile());
    }


    private IEnumerator MoveProjectTile()
    {
        while ( targetEnemy != null && targetEnemy.IsEnemyAlive && Vector2.Distance(targetEnemy.transform.localPosition, startPosition.localPosition) <50f)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if (targetEnemy == null || targetEnemy.IsEnemyAlive == false)
           PoolManager.Instance.AddObjectFromPool(PoolObjectType.Arrow,gameObject);
    }

   public void ColliderState(bool state)
    {
        collider.enabled = state;
    }
}
