using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCore : MonoBehaviour
{
    #region ตัวแปร
    protected float Gspeed;
    protected float Fspeed;
    protected float JumpForce;
    protected float FlightAcc;
    protected float FlightTakeOff;
    protected float FDash;
    protected float jumpCount = 1f;
    protected float PlusSpeed = 10f;
    protected float Firedelay = 0.5f;
    protected float GunModeNum = 1f;

    public bool MActive = true;
    protected bool useGravity = true;
    protected bool CamMode = true;
    protected bool ChangeModeBool = true;
    protected bool FireGun = true;
    protected bool jump = true;
    protected bool run = true;
    protected bool OpenFlashLight = false;
    protected bool onGround = false;

    public GameObject FlashLight;
    public Slider DashGauge;
    public Animator anima;
    public GameObject Thuster;
    public SoundManager sM;

    protected Rigidbody rg;
    protected ReFlightMode FlightM;
    protected FireBullet FireBulletS;
    protected Hp Hppoint;
    protected CrossHair CrossDot;
    protected ForceShield ForceShield;
    protected EBullet eBullet;
    #endregion

    void Start()
    {
        GetComponentMethod(); //Get Component
        CalculateSpeed(); //Set All Movement Speed
        SetPoint(); //Set Hp
        FlashLight.SetActive(false); //Turn of FlashLight
        GunModeNum = 1; //Set GunMode to Rifle
        CrossDot.ChangeDotMaterial(1); //Set CrossHair to Rifle
        rg.useGravity = useGravity; //Set Gravity
    }
    void Update()
    {
        Controller(); //Active & Check General PlayerController
        ChangeMode();//Check if player want to ChangeMode or not
    }

    //Update that use in all mode
    protected virtual void LateUpdate()
    {
        Combat(); //Active Combat System
        GunModeSelect(); //Active, Check & Set GunMaterial realtive to GunMode
    }
    protected void GetComponentMethod()
    {
        rg = this.GetComponent<Rigidbody>();
        FlightM = this.GetComponent<ReFlightMode>();
        FireBulletS = this.GetComponent<FireBullet>();
        Hppoint = GameObject.Find("DEMO_GM").GetComponent<Hp>();
        CrossDot = GameObject.Find("Dot").GetComponent<CrossHair>();
        ForceShield = GameObject.Find("PlayerShield").GetComponent<ForceShield>();
    }
    protected void CalculateSpeed()
    {
        Gspeed = Mathf.Pow(rg.mass, 4);
        Fspeed = Mathf.Pow(rg.mass, 5);
        JumpForce = Mathf.Pow(rg.mass, 11);
        FlightAcc = Mathf.Pow(rg.mass, 11);
        FlightTakeOff = Mathf.Pow(rg.mass, 13);
        FDash = Fspeed + 10f;
    }
    protected void SetPoint()
    {
        Hppoint.currentHp = 100f;
        DashGauge.value = 200f;
    }
    protected void Controller()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OpenFlashLight = !OpenFlashLight;
            FlashLight.SetActive(OpenFlashLight);
        }
    }
    protected void ChangeMode()
    {
        //ChangeMode
        if (Input.GetKeyDown(KeyCode.F))
        {
            MActive = !MActive; //ChangeMode between Ground & Flight
            ChangeModeBool = true; //Active FirstTime Mode Change
            useGravity = !useGravity; //Toggle Gravity
            rg.useGravity = useGravity; //Set Gravity
            CamMode = !CamMode; //Toggle Camera Mode
        }
    }
    protected void GunModeSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //Rifle Mode
        {
            GunModeNum = 1;
            Firedelay = 0.5f;
            CrossDot.ChangeDotMaterial(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Piston Mode
        {
            GunModeNum = 2;
            Firedelay = 0.3f;
            CrossDot.ChangeDotMaterial(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) //Sniper Mode
        {
            GunModeNum = 3;
            Firedelay = 0.9f;
            CrossDot.ChangeDotMaterial(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) //Machine Mode
        {
            GunModeNum = 4;
            Firedelay = 0.1f;
            CrossDot.ChangeDotMaterial(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) //ShotGun Mode
        {
            GunModeNum = 5;
            Firedelay = 1.2f;
            CrossDot.ChangeDotMaterial(5);
        }
    }
    protected void Combat()
    {
        if (Input.GetMouseButton(0) && FireGun)
        {
            FireGun = !FireGun;
            FireBulletS.GunMode(GunModeNum);
            Invoke("ResetFire", Firedelay);
            anima.SetFloat("Walk", 0f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            anima.SetBool("Attack", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anima.SetBool("Attack", false);
        }
    }
    protected void ResetFire()
    {
        FireGun = true;
    }
}
