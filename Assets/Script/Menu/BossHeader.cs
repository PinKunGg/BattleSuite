using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHeader : MonoBehaviour
{
    BossUnityEvent BossEvent = new BossUnityEvent();
    GM_DemoScene GM_Demo;

    void Start()
    {
        EventManager.AddCheckSpawnInvoker(this);
        GM_Demo = GameObject.Find("DEMO_GM").GetComponent<GM_DemoScene>();
    }
    void Update()
    {
        if(GM_Demo.WaveCount % 3 == 0)
        {
            print("EventHander Work!");
            GM_Demo.WaveCount = 1;
            GM_Demo.EventSpawnBoss();
        }
    }

    public void AddBossSpawnEvent(UnityAction listener)
    {
        BossEvent.AddListener(listener);
    }

    public void TestEvent()
    {
        BossEvent.Invoke();
    }
}
