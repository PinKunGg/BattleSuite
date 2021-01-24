using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyScript : EnemyCore
{
    protected override void OnEnable()
    {
        base.OnEnable();

        GM_DemoScene.FlyingEnemyList.Add(this);
        speed = Random.Range(10, 40);

        currentHp = eHp;
        SetHp(currentHp);

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
            if (foundTarget == true || currentHp != eHp)
            {
                targetEngage();
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            Destroy(this.gameObject);
        }

        //Hp
        SetHp(currentHp);

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
            GM_DemoScene.FlyingEnemyList.Remove(this);
            GM_Demo.Score += 5f;
        }
    }

    void targetEngage()
    {
        foundTarget = true;

        try
        {
            if (facing == true)
            {
                FaceTarget();
            }
            if (Vector3.Distance(transform.position, target.position) > 50f)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                //agent.SetDestination(target.position);
                PathFinding();
            }
            if (Vector3.Distance(transform.position, target.position) <= 50f)
            {
                facing = true;
            }
            if (transform.position.y <= 15f)
            {
                transform.position = new Vector3(transform.position.x, 15f, transform.position.z);
            }
            if (attack == true)
            {
                Attack();
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    void PathFinding()
    {
        RaycastHit hit;
        Vector3 rayCastOffsetZero = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        Debug.DrawRay(left, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.green);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.red);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.yellow);

        //Front Detect
        if (Physics.Raycast(left, transform.forward, out hit, detectionDistance))
        {
            rayCastOffsetZero += Vector3.right;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance))
        {
            rayCastOffsetZero += Vector3.left;
        }
        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance))
        {
            rayCastOffsetZero += Vector3.down;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance))
        {
            rayCastOffsetZero += Vector3.up;
        }

        //Rotate
        if (rayCastOffsetZero != Vector3.zero)
        {
            facing = false;
            transform.Rotate(rayCastOffsetZero * TurnSpeed * Time.deltaTime);
        }
        else
        {
            facing = true;
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
        Gizmos.color = Color.blue;
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
