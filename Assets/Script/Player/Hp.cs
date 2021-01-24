using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public float maxHp;
    public float currentHp;

    public Slider slider;
    GM_DemoScene GM_Demo;

    GameObject PlayerS;

    private void Start()
    {
        currentHp = maxHp;
        SetHealth(maxHp);

        PlayerS = GameObject.Find("Player");
        GM_Demo = GameObject.Find("DEMO_GM").GetComponent<GM_DemoScene>();
    }

    private void Update()
    {
        SetHealth(currentHp);

        if(currentHp > 100)
        {
            currentHp = 100;
        }

        if (currentHp <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Destroy(PlayerS.gameObject);
            GM_Demo.GameOverMethod();
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
