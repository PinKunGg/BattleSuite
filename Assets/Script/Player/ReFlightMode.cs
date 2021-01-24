using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReFlightMode : ReGroundMode
{
    #region ตัวแปร
    public bool FirstFlight = true;
    public bool onGroundF = true;
    public float nextRotW = 0f;
    public float nextRotSpace = 0f;
    #endregion

    //Update in FlightMode Only
    protected override void LateUpdate()
    {

    }

    public override void Mode()
    {
        #region FlightMode
        if (!MActive)//MActive = true -> GroundMode, MActive = false -> FlightMode (Override from ReFlightMode.cs)
        {
            ForceShield.regenRate = 3f;

            if (FirstFlight) //Run only FirstTime on ModeChage
            {
                StartCoroutine(FlightFirstmethod()); //Take off (Make player up to the sky when FirstTime FlightMode Active)
                anima.SetBool("GroundMode", false);
                anima.SetTrigger("FlightMode");
            }
            if (ChangeModeBool)
            {
                ChangeModeBool = !ChangeModeBool;
                Thuster.SetActive(true);
                anima.SetFloat("Walk", 0f);
                anima.SetFloat("Run", 0f);
            }

            #region Control Player, Aniamtion
            //Control Player
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 inputVector = this.transform.TransformDirection(h, 0, v);
            inputVector *= Fspeed * Time.deltaTime;

            rg.MovePosition(rg.position + inputVector);

            //Run - Jump
            if (Input.GetKeyDown(KeyCode.Space) && DashGauge.value > 0) //Dash FlightSpeed FirstTime
            {
                DashGauge.value -= 20f;
                Fspeed += PlusSpeed;
                rg.velocity = (inputVector.normalized * FDash);
            }
            if (Input.GetKey(KeyCode.Space) && DashGauge.value <= 200 && DashGauge.value > 0)
            {
                anima.SetFloat("Fly_Run", 1f);
                DashGauge.value -= 20f * Time.deltaTime;

                if (nextRotSpace < 15 && Input.GetKey(KeyCode.W))
                {
                    nextRotSpace += 45f * Time.deltaTime;
                    Thuster.transform.localRotation = Quaternion.Euler(nextRotSpace, 0f, 0f);
                }
                if (nextRotSpace > 15)
                {
                    nextRotSpace = 15f;
                }
            }
            else if (!Input.GetKey(KeyCode.Space) || DashGauge.value <= 0)
            {
                anima.SetFloat("Fly_Run", 0f);

                if (!(Input.GetKey(KeyCode.W)))
                {
                    if (nextRotSpace > 0)
                    {
                        nextRotSpace -= 45f * Time.deltaTime;
                        Thuster.transform.localRotation = Quaternion.Euler(nextRotSpace, 0f, 0f);
                    }
                    if (nextRotSpace <= 0)
                    {
                        nextRotSpace = 0f;
                    }
                }
                else
                {
                    if (nextRotSpace > 4)
                    {
                        nextRotSpace -= 45f * Time.deltaTime;
                        Thuster.transform.localRotation = Quaternion.Euler(nextRotSpace, 0f, 0f);
                    }
                    if (nextRotSpace <= 4)
                    {
                        nextRotSpace = 4f;
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) || DashGauge.value <= 0) //Decress Flight Speed
            {
                Fspeed = Mathf.Pow(rg.mass, 5);
            }
            if (Input.GetKey(KeyCode.E) && DashGauge.value <= 200 && DashGauge.value > 0) //Incress Flight Height
            {
                anima.SetFloat("Fly_Incress", 1f);
                DashGauge.value -= 20f * Time.deltaTime;
                rg.AddForce(rg.velocity.x, FlightAcc * Time.deltaTime, rg.velocity.z, ForceMode.Acceleration);

                if (Input.GetKeyDown(KeyCode.Space) && DashGauge.value > 0) //Dash Up
                {
                    DashGauge.value -= 20f;
                    rg.velocity = Vector3.up * FDash;
                }
            }
            else if (!Input.GetKey(KeyCode.E) || DashGauge.value <= 0)
            {
                anima.SetFloat("Fly_Incress", 0f);
            }
            if (Input.GetKey(KeyCode.C) && !onGroundF && DashGauge.value <= 200 && DashGauge.value > 0) //Decress Flight Height
            {
                anima.SetFloat("Fly_Decress", 1f);
                rg.AddForce(rg.velocity.x, -FlightAcc * Time.deltaTime, rg.velocity.z, ForceMode.Acceleration);

                if (Input.GetKeyDown(KeyCode.Space) && DashGauge.value > 0) //Dash Down
                {
                    DashGauge.value -= 20f;
                    rg.velocity = Vector3.down * FDash;
                }
            }
            else if (!Input.GetKey(KeyCode.C))
            {
                anima.SetFloat("Fly_Decress", 0f);
            }
            if ((!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.C)) && DashGauge.value <= 200)
            {
                DashGauge.value += 10f * Time.deltaTime;
            }
            else
            {
                rg.velocity = new Vector3(rg.velocity.x, rg.velocity.y, rg.velocity.z);
            }
            #endregion

            #region Animation
            //Animation
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Space))
            {
                anima.SetFloat("Fly_Walk", 1f);
                if(nextRotW < 4 && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.C))
                {
                    nextRotW += 12f * Time.deltaTime;
                    Thuster.transform.localRotation = Quaternion.Euler(nextRotW, 0f, 0f);
                }
                else
                {
                    nextRotW -= 12f * Time.deltaTime;
                    Thuster.transform.localRotation = Quaternion.Euler(nextRotW, 0f, 0f);
                    if (nextRotW <= 0)
                    {
                        nextRotW = 0f;
                    }
                }
                if(nextRotW > 4)
                {
                    nextRotW = 4f;
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                anima.SetFloat("Fly_Walk", -1f);
            }
            else if (!(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
            {
                anima.SetFloat("Fly_Walk", 0f);
                if (nextRotW > 0)
                {
                    nextRotW -= 12f * Time.deltaTime;
                    Thuster.transform.localRotation = Quaternion.Euler(nextRotW, 0f, 0f);
                }
                if (nextRotW <= 0)
                {
                    nextRotW = 0f;
                }
            }
            #endregion

            
        }
        #endregion
    }

    IEnumerator FlightFirstmethod() //Run Take off for 2sec
    {
        FirstFlight = !FirstFlight;
        print("FirstFlight-Active");
        rg.AddForce(rg.velocity.x, FlightTakeOff * Time.deltaTime, rg.velocity.z, ForceMode.Acceleration);

        yield return new WaitForSeconds(2f);
        print("FirstFlight-DeActive");
        StopCoroutine("FightFirstmethod");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            onGroundF = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            onGroundF = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
