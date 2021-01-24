using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReGroundMode : PlayerCore
{
    private void FixedUpdate()
    {
        Mode(); //Active & Check Center Controller for GroundMode & FlightMode
    }

    //Update in GroundMode Only
    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (rg.velocity.y < 0)
        {
            rg.velocity += Vector3.up * Physics.gravity.y * (5f - 1) * Time.deltaTime;
        }
        if (onGround == false)
        {
            anima.SetFloat("Fly_Decress", 1f);
        }
        else if (onGround == true)
        {
            anima.SetFloat("Fly_Decress", 0f);
        }
    }

    public virtual void Mode()
    {
        #region GroundMode
        if (MActive)//MActive = true -> GroundMode, MActive = false -> FlightMode (Override from ReFlightMode.cs)
        {
            ForceShield.regenRate = 7f;

            if (ChangeModeBool) //Run only FirstTime on ModeChage
            {
                anima.SetBool("GroundMode",true);
                anima.SetBool("FlightMode",false);
                ChangeModeBool = !ChangeModeBool;
                onGround = false;
                anima.SetFloat("Fly_Walk", 0f);
                anima.SetFloat("Fly_Run", 0f);
                anima.SetFloat("Fly_Incress", 0f);;
                Thuster.SetActive(false);
                FlightM.FirstFlight = true;
            }

            #region Control Player
            //Control Player
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 inputVector = this.transform.TransformDirection(h, transform.position.y, v);
            inputVector *= Gspeed * Time.deltaTime;
            inputVector.y = 0;
            rg.MovePosition(rg.position + inputVector);

            //Run - Jump
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && jump && DashGauge.value > 0) //Jump
            {
                jump = !jump;
                anima.SetTrigger("Jump");
                Invoke("ResetJump", 0.1f);
                rg.AddForce(rg.velocity.x, rg.velocity.y + JumpForce * Time.deltaTime, rg.velocity.z, ForceMode.Impulse);
                DashGauge.value -= 20f;
                jumpCount--;
            }
            if (!Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)))
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && DashGauge.value > 0) //Incress Gspeed
                {
                    Gspeed += PlusSpeed;
                }
                if (Input.GetKey(KeyCode.LeftShift) && DashGauge.value <= 200 && DashGauge.value > 0)
                {
                    DashGauge.value -= 20f * Time.deltaTime;
                    anima.SetFloat("Run", 1f);
                }
                else if (DashGauge.value <= 0)
                {
                    anima.SetFloat("Run", 0f);
                }
            }
            if (!Input.GetKey(KeyCode.LeftShift) && DashGauge.value <= 200)
            {
                anima.SetFloat("Run", 0f);
                DashGauge.value += 15f * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || DashGauge.value <= 0) //Decress Gspeed
            {
                Gspeed = Mathf.Pow(rg.mass, 4);
            }
            #endregion

            #region Animation
            //Animation
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anima.SetFloat("Walk", 1f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                anima.SetFloat("Walk",-1f);
            }
            else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)))
            {
                anima.SetFloat("Walk", 0f);
            }
            #endregion
        }
        #endregion
    }

    void ResetJump()
    {
        jump = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "floor")
        {
            if (MActive)
            {
                jumpCount = 1f; //Reset JumpCount
                onGround = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (ForceShield.ForceShieldGauge.value <= 0)
        {
            if (other.gameObject.tag == "eBullet")
            {
                eBullet = other.gameObject.GetComponent<EBullet>();
                Hppoint.TakeDamage(eBullet.BulletDamage);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 50f);
    }
    private void OnDestroy()
    {
        print("Player Destroy");

        sM.StopSound("Rifle");
        sM.StopSound("Piston");
        sM.StopSound("Sniper");
        sM.StopSound("Machine");
        sM.StopSound("Machine_Out");
        sM.StopSound("ShotGun");
        sM.StopSound("OutOfAmmo");
    }
}
