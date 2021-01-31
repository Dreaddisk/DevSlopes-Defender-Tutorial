using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float timeBetweenAttacks, attackRadius;

    [SerializeField]
    private Projectile projectile;

    private bool isAttack = false;

    private Enemy targetEnemy = null;
    private float attackCounter;
    private AudioSource audioSource;
    #endregion


    #region UnityFunctions
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemyInRange();

            if(nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if(attackCounter <= 0f)
            {
                isAttack = true;
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttack = false;
            }
            if(Vector2.Distance(transform.position, targetEnemy.transform.position) > attackRadius)
            {
                targetEnemy = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if(isAttack)
        {
            Attack();
        }
    }

    #endregion

    public void Attack()
    {
        isAttack = false;

        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        if (newProjectile.ProjectileType == ProType.arrow)
            audioSource.PlayOneShot(SoundManager.Instance.Arrow);
        else if (newProjectile.ProjectileType == ProType.fireball)
            audioSource.PlayOneShot(SoundManager.Instance.Fireball);
        else if (newProjectile.ProjectileType == ProType.rock)
            audioSource.PlayOneShot(SoundManager.Instance.Rock);

        if(targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while (GetTargetDirection(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }

        if (projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }

    private float GetTargetDirection(Enemy thisEnemy)
    {
        if(thisEnemy == null)
        {
            thisEnemy = GetNearestEnemyInRange();
            if(thisEnemy == null)
            {
                return 0f;
            }
        }

        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    private Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    private List<Enemy>GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius && !enemy.IsDead)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }



} // Tower class
