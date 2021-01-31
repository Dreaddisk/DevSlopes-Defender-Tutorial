using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int healthPoints;

    [SerializeField]
    private int rewardAmt;

    [SerializeField]
    private Transform exitPoint;

    [SerializeField]
    private Transform[] wayPoints;

    [SerializeField]
    private float navigationUpdate;

    [SerializeField]
    private Animator anim;

    private int target = 0;

    private float navigationTime = 0;

    private bool isDead = false;

    private Transform enemy;
    private Collider2D enemyCollider;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    #endregion

    #region UnityFunctions

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        // GameManager.Instance.registerEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(wayPoints != null && !isDead)
        {
            navigationTime += Time.deltaTime;
            if(navigationTime > navigationUpdate)
            {
                if(target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, 0.8f * navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, 0.8f * navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WayPoint")
            target += 1;
        else if(collision.tag == "Finish")
        {
            GameManager.Instance.TotalEscaped += 1;
        }
    }

    #endregion

    public void EnemyHit(int hitPoints)
    {
        if(healthPoints - hitPoints > 0)
        {
            anim.Play("Hurt");
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            healthPoints -= hitPoints;
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("didDie");
        
    }
}
