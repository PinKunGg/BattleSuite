using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    static BossHeader BossSpawnInvoker;
    static UnityAction BossSpawnListener;

    public static void AddCheckSpawnInvoker(BossHeader invoker)
    {
        BossSpawnInvoker = invoker;
        if (BossSpawnListener != null)
        {
            BossSpawnInvoker.AddBossSpawnEvent(BossSpawnListener);
        }
    }
    public static void AddBossSpawnListener(UnityAction listener)
    {
        BossSpawnListener = listener;
        if (BossSpawnInvoker != null)
        {
            BossSpawnInvoker.AddBossSpawnEvent(BossSpawnListener);
        }
    }
}
