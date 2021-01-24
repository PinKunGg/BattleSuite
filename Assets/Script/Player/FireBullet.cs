using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FireBullet : MonoBehaviour
{
    #region ตัวแปร
    public GameObject RifleBeamBullet;
    public GameObject PistonBeamBullet;
    public GameObject SniperBeamBullet;
    public GameObject MachineBeamBullet;
    public GameObject ShotGunBeamBullet;
    public GameObject GunPosition;

    public Slider SliderRi;
    public Slider SliderPi;
    public Slider SliderSni;
    public Slider SliderMa;
    public Slider SliderShot;

    PlayerCore playerS;
    SoundManager sM;

    public float BulletAmountRi = 30f;
    public float BulletAmountPi = 7f;
    public float BulletAmountSni = 2f;
    public float BulletAmountMa = 100f;
    public float BulletAmountShot = 5f;
    float BulletAmountStorageRifle = 30f;
    float BulletAmountStoragePiston = 7f;
    float BulletAmountStorageSniper = 2f;
    float BulletAmountStorageMachine = 100f;
    float BulletAmountStorageShotgun = 5f;
    public float ReloadTimeRi = 6f;
    public float ReloadTimePi = 5f;
    public float ReloadTimeSni = 8f;
    public float ReloadTimeMa = 12f;
    public float ReloadTimeShot = 7f;

    float GunNum;
    #endregion

    private void Start()
    {
        playerS = this.GetComponent<PlayerCore>();
        sM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (playerS.MActive == true)
        {
            GunPosition.transform.localPosition = new Vector3(0.74f, 5.45f, 12f);
        }
        else if(playerS.MActive == false)
        {
            GunPosition.transform.localPosition = new Vector3(0.74f, 6.5f, 12f);
        }

        SliderRi.value = BulletAmountRi;
        SliderPi.value = BulletAmountPi;
        SliderSni.value = BulletAmountSni;
        SliderMa.value = BulletAmountMa;
        SliderShot.value = BulletAmountShot;

        if (GunNum == 4 && Input.GetMouseButtonUp(0))
        {
            sM.StopSound("Machine");
            sM.PlaySound("Machine_Out");
        }
    }

    public void GunMode(float ModeNum)
    {
        GunNum = ModeNum;
        //Rifle
        if (ModeNum == 1 && BulletAmountRi > 0)
        {
            RifleMode();
            sM.PlaySound("Rifle");
            BulletAmountRi--;
        }
        if(ModeNum == 1 && BulletAmountRi <= 0)
        {
            sM.PlaySound("OutOfAmmo");
            Invoke("ReloadRi", ReloadTimeRi);
        }

        //Piston
        if (ModeNum == 2 && BulletAmountPi > 0)
        {
            PistonMode();
            sM.PlaySound("Piston");
            BulletAmountPi--;
        }
        if (ModeNum == 2 && BulletAmountPi <= 0)
        {
            sM.PlaySound("OutOfAmmo");
            Invoke("ReloadPi", ReloadTimePi);
        }

        //Sniper
        if (ModeNum == 3 && BulletAmountSni > 0)
        {
            SniperMode();
            sM.PlaySound("Sniper");
            BulletAmountSni--;
        }
        if (ModeNum == 3 && BulletAmountSni <= 0)
        {
            sM.PlaySound("OutOfAmmo");
            Invoke("ReloadSni", ReloadTimeSni);
        }

        //Machine
        if (ModeNum == 4 && BulletAmountMa > 0)
        {
            MachineMode();
            sM.PlaySound("Machine");
            BulletAmountMa--;
        }
        if (ModeNum == 4 && BulletAmountMa <= 0)
        {
            sM.StopSound("Machine");
            sM.PlaySound("Machine_Out");
            Invoke("ReloadMa", ReloadTimeMa);
        }

        //ShotGun
        if (ModeNum == 5 && BulletAmountShot > 0)
        {
            ShotGunMode();
            sM.PlaySound("ShotGun");
            BulletAmountShot--;
        }
        if (ModeNum == 5 && BulletAmountShot <= 0)
        {
            sM.PlaySound("OutOfAmmo");
            Invoke("ReloadShot", ReloadTimeShot);
        }
    }
    void ReloadRi()
    {
        BulletAmountRi = BulletAmountStorageRifle;
    }
    void ReloadPi()
    {
        BulletAmountPi = BulletAmountStoragePiston;
    }
    void ReloadSni()
    {
        BulletAmountSni = BulletAmountStorageSniper;
    }
    void ReloadMa()
    {
        BulletAmountMa = BulletAmountStorageMachine;
    }
    void ReloadShot()
    {
        BulletAmountShot = BulletAmountStorageShotgun;
    }

    void RifleMode()
    {
        Instantiate(RifleBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
    }
    void PistonMode()
    {
        Instantiate(PistonBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
    }
    void SniperMode()
    {
        Instantiate(SniperBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
    }
    void MachineMode()
    {
        Instantiate(MachineBeamBullet, GunPosition.transform.position, GunPosition.transform.rotation);
    }
    void ShotGunMode()
    {
        Quaternion ShotGunRot = GunPosition.transform.rotation;

        for (int i = 1; i <= 5; i++)
        {
            ShotGunRot.x += Random.Range(-0.01f, 0.01f);
            ShotGunRot.y += Random.Range(-0.01f, 0.01f);
            Instantiate(ShotGunBeamBullet, GunPosition.transform.position, ShotGunRot);
        }
    }

}
