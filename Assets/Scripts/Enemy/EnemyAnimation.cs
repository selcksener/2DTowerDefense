using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;

    public void IdleAnim()// Duruþ animasyonu // Gereksiz // Idle animation // Useless
    {
        enemyAnimator.SetBool("isIdle",true);
        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetBool("isDie", false);
    }
 
    public void WalkingAnim()// Yürüme animasyonu // Idle animation
    {
        enemyAnimator.SetBool("isIdle", false);
        enemyAnimator.SetBool("isWalking", true);
        enemyAnimator.SetBool("isDie", false);
    }

    public void DeathAnim()// Ölme animasyonu // Die animation
    {
        enemyAnimator.SetBool("isIdle", false);
        enemyAnimator.SetBool("isWalking", false);
        enemyAnimator.SetBool("isDie", true);
    }
}
