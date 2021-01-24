using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboBossFlight : EnemyCore
{
    Animator anima;

    protected override void OnEnable()
    {
        base.OnEnable();

        anima = this.GetComponent<Animator>();
        GM_DemoScene.BossF.Add(this);
        speed = Random.Range(10, 40);

        eHp = 500f;
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
            GM_DemoScene.BossF.Remove(this);
            GM_Demo.Score += 5f;
        }
    }

    void targetEngage()
    {
        foundTarget = true;

        try
        {
            anima.SetFloat("Fly_Walk", 1f);
            if (facing == true)
            {
                FaceTarget();
            }
            if (Vector3.Distance(transform.position, target.position) > 100f)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                //agent.SetDestination(target.position);
                PathFinding();
            }
            if (Vector3.Distance(transform.position, target.position) <= 100f)
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
        Gizmos.color = Color.white;
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
