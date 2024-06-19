using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeTrigger : MonoBehaviour
{
    [SerializeField] private Tower tower;

    /// <summary>
    ///    Kule menziline giren d��manlar kulenin listesine ekleniyor 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Collision " + collision.gameObject.name);
            tower.AddorRemoveEnemyInList(collision.GetComponent<Enemy>(), true);
        }
    }

    /// <summary>
    /// Kule menzilinden ��kan d��manlar kule listesinden ��kar�l�yor
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            tower.AddorRemoveEnemyInList(collision.GetComponent<Enemy>(), false);
        }
    }
}
