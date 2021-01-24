using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : EnemyCore
{
    protected override void OnEnable()
    {
        base.OnEnable();

        GM_DemoScene.GroundEnemyList.Add(this);
        speed = Random.Range(10, 30);

        currentHp = eHp;
        SetHp(currentHp);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        targetEngage();
    }
    void Update()
    {
        try
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius && foundTarget == true && attack == false || currentHp != eHp)
            {
                attack = true;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            Destroy(this.gameObject);
        }

        agent.stoppingDistance = 50f;

        if (foundTarget == true || currentHp != eHp)
        {
            targetEngage();
        }

        //Hp
        SetHp(currentHp);

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
            GM_DemoScene.GroundEnemyList.Remove(this);
            GM_Demo.Score += 5f;
        }
    }

    void targetEngage()
    {
        foundTarget = true;

        Vector3 NotY = target.position;
        NotY.y = 0;
        agent.SetDestination(NotY);
        FaceTarget();
        if (attack == true)
        {
            Attack();
        }

    }
    void HpManager(float dmg)
    {
        currentHp -= dmg;
    }
    void SetHp(float health)
    {
        slider.value = health;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            bullet = other.gameObject.GetComponent<Bullet>();
            HpManager(bullet.BulletDamage);
            Destroy(other.gameObject);
        }
    }
}
