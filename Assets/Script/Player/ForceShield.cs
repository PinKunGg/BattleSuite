using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceShield : MonoBehaviour
{
    public Slider ForceShieldGauge;
    public GameObject Shield;
    protected EBullet eBullet;
    bool isAttack = false;
    public float regenRate = 5f;

    private void Start()
    {
        ForceShieldGauge.value = 100f;
    }
    private void Update()
    {
        RegenShield();

        if (ForceShieldGauge.value <= 0)
        {
            Shield.SetActive(false);
        }
    }

    void CalShield(float dmg)
    {
        ForceShieldGauge.value -= dmg;
    }
    void RegenShield()
    {
        if (isAttack == false)
        {
            ForceShieldGauge.value += regenRate * Time.deltaTime;
        }
        if (isAttack == true)
        {
            Invoke("RegenDelay", 5f);
        }
    }
    void RegenDelay()
    {
        isAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "eBullet")
        {
            eBullet = other.gameObject.GetComponent<EBullet>();
            CalShield(eBullet.BulletDamage);
            isAttack = true;
            Destroy(other.gameObject);
        }
    }
}
