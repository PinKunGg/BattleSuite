using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyCore : MonoBehaviour
{
    #region ตัวแปร
    protected float lookRadius = 200f;
    protected float speed;
    protected float eHp = 100f;
    protected float currentHp;

    public int GunMode;

    protected bool fire = true;
    protected bool attack = false;

    public bool foundTarget = false;

    protected Transform target;
    protected NavMeshAgent agent;
    protected GameObject player;
    protected GM_DemoScene GM_Demo;
    protected Bullet bullet;

    protected RaycastHit rayhit;

    public GameObject ERifleBeamBullet;
    public GameObject EPistonBeamBullet;
    public GameObject ESniperBeamBullet;
    public GameObject EMachineBeamBullet;
    public GameObject EShotGunBeamBullet;
    public GameObject GunPosition;
    public Slider slider;

    protected float eBulletAmountRi = 30f;
    protected float eBulletAmountPi = 7f;
    protected float eBulletAmountSni = 2f;
    protected float eBulletAmountMa = 100f;
    protected float eBulletAmountShot = 5f;
    protected float eBulletAmountStorageRifle = 30f;
    protected float eBulletAmountStoragePiston = 7f;
    protected float eBulletAmountStorageSniper = 2f;
    protected float eBulletAmountStorageMachine = 100f;
    protected float eBulletAmountStorageShotgun = 5f;
    protected float eReloadTimeRi = 6f;
    protected float eReloadTimePi = 5f;
    protected float eReloadTimeSni = 8f;
    protected float eReloadTimeMa = 12f;
    protected float eReloadTimeShot = 7f;

    protected float rayCastOffset = 2.5f;
    protected float detectionDistance = 20f;
    protected float TurnSpeed = 300f;
    protected bool facing = true;
    #endregion

    protected virtual void OnEnable()
    {
        player = GameObject.Find("TargetPlayerLock");
        GM_Demo = GameObject.Find("DEMO_GM").GetComponent<GM_DemoScene>();
        target = player.transform;

        GunMode = Random.Range(1, 5);
    }

    protected void FaceTarget()
    {
        try
        {
            Vector3 diretion = (target.position - transform.position);
            Quaternion lookRotation = Quaternion.LookRotation(diretion);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100f);
            GunPosition.transform.LookAt(target.transform);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    protected virtual void Attack()
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
                        print("Attack!!");

                        //Rifle
                        if (GunMode == 1 && eBulletAmountRi > 0)
                        {
                            Instantiate(ERifleBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            eBulletAmountRi--;
                            Invoke("ResetFire", 0.5f);
                        }
                        if (eBulletAmountRi <= 0)
                        {
                            Invoke("ReloadRi", eReloadTimeRi);
                        }

                        //Piston
                        else if (GunMode == 2 && eBulletAmountPi > 0)
                        {
                            Instantiate(EPistonBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            eBulletAmountPi--;
                            Invoke("ResetFire", 0.3f);
                        }
                        if (eBulletAmountPi <= 0)
                        {
                            Invoke("ReloadPi", eReloadTimePi);
                        }

                        //Sniper
                        else if (GunMode == 3 && eBulletAmountSni > 0)
                        {
                            Instantiate(ESniperBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            eBulletAmountSni--;
                            Invoke("ResetFire", 0.9f);
                        }
                        if (eBulletAmountSni <= 0)
                        {
                            Invoke("ReloadSni", eReloadTimeSni);
                        }

                        //Machine
                        else if (GunMode == 4 && eBulletAmountMa > 0)
                        {
                            Instantiate(EMachineBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
                            eBulletAmountMa--;
                            Invoke("ResetFire", 0.1f);
                        }
                        if (eBulletAmountMa <= 0)
                        {
                            Invoke("ReloadMa", eReloadTimeMa);
                        }

                        //ShotGun
                        else if (GunMode == 5 && eBulletAmountShot > 0)
                        {
                            Quaternion ShotGunRot = GunPosition.transform.rotation;

                            for (int i = 1; i <= 5; i++)
                            {
                                ShotGunRot.x += Random.Range(-0.01f, 0.01f);
                                ShotGunRot.y += Random.Range(-0.01f, 0.01f);
                                Quaternion ShotGunRotSpawn = ShotGunRot;
                                Instantiate(EShotGunBeamBullet, GunPosition.transform.position, ShotGunRot);
                            }
                            eBulletAmountShot--;
                            Invoke("ResetFire", 1.2f);
                        }
                        if (eBulletAmountShot <= 0)
                        {
                            Invoke("ReloadShot", eReloadTimeShot);
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }
    protected void ResetFire()
    {
        fire = true;
    }
    //Reload
    protected void ReloadRi()
    {
        eBulletAmountRi = eBulletAmountStorageRifle;
        fire = true;
    }
    protected void ReloadPi()
    {
        eBulletAmountPi = eBulletAmountStoragePiston;
        fire = true;
    }
    protected void ReloadSni()
    {
        eBulletAmountSni = eBulletAmountStorageSniper;
        fire = true;
    }
    protected void ReloadMa()
    {
        eBulletAmountMa = eBulletAmountStorageMachine;
        fire = true;
    }
    protected void ReloadShot()
    {
        eBulletAmountShot = eBulletAmountStorageShotgun;
        fire = true;
    }
}
