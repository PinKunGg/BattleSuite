using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoboBoss : EnemyCore
{
    Animator anima;

    protected override void OnEnable()
    {
        base.OnEnable();

        anima = this.GetComponent<Animator>();
        GM_DemoScene.BossG.Add(this);
        speed = Random.Range(20, 40);

        eHp = 500f;
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

            agent.stoppingDistance = 100f;

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
            GM_DemoScene.BossG.Remove(this);
            GM_Demo.Score += 5f;
        }
    }

    void targetEngage()
    {
        foundTarget = true;

        try
        {
            anima.SetFloat("Walk", 1f);
            Vector3 NotY = target.position;
            NotY.y = 0;
            agent.SetDestination(NotY);
            FaceTarget();
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
    protected override void Attack()
    {
        try
        {
            Ray sight = new Ray();

            sight.origin = transform.position + 0.5f * transform.up;
            Vector3 localPlayerPos = transform.InverseTransformPoint(target.position);
            Vector3 localEnemyPos = transform.InverseTransformPoint(sight.origin);
            Vector3 localPlayerDir = localPlayerPos - localEnemyPos;
            Vector3 v = localPlayerDir;
            v.y = 0.0f;
            localPlayerDir = Quaternion.FromToRotation(v, Vector3.forward) * localPlayerDir;
            sight.direction = transform.TransformDirection(localPlayerDir);

            Debug.DrawRay(this.transform.position, sight.direction, Color.yellow);

            if (Physics.Raycast(sight, out rayhit))
            {
                if (rayhit.collider.CompareTag("Player"))
                {
                    if (fire)
                    {
                        fire = !fire;
                        anima.SetBool("Attack", true);

                        //Rifle
                        if (GunMode == 1)
                        {
                            Instantiate(ERifleBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            Invoke("ResetFire", 0.5f);
                        }

                        //Piston
                        else if (GunMode == 2)
                        {
                            Instantiate(EPistonBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            Invoke("ResetFire", 0.3f);
                        }

                        //Sniper
                        else if (GunMode == 3)
                        {
                            Instantiate(ESniperBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            Invoke("ResetFire", 0.9f);
                        }

                        //Machine
                        else if (GunMode == 4)
                        {
                            Instantiate(EMachineBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            Invoke("ResetFire", 0.1f);
                        }

                        //ShotGun
                        else if (GunMode == 5)
                        {
                            Quaternion ShotGunRot = GunPosition.transform.rotation;

                            for (int i = 1; i <= 5; i++)
                            {
                                ShotGunRot.x += Random.Range(-0.01f, 0.01f);
                                ShotGunRot.y += Random.Range(-0.01f, 0.01f);
                                Instantiate(EShotGunBeamBullet, GunPosition.transform.position, ShotGunRot);
                            }
                            Invoke("ResetFire", 1.2f);
                        }
                    }
                }
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
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
        Gizmos.color = Color.cyan;
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
