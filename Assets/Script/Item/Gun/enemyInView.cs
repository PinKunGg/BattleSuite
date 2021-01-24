using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyInView : MonoBehaviour
{
    bool addOnlyOnce;//This Boolean Is Used To Only Allow The Enemy To Be Added To The List Once
    bool removeOnlyOnce;

    void Start()
    {
        addOnlyOnce = true;
        removeOnlyOnce = true;
    }
    void OnBecameVisible()
    {
        if (addOnlyOnce)
        {
            addOnlyOnce = false;
            removeOnlyOnce = true;
            targetController.nearByEnemies.Add(this);
        }
    }
    void OnBecameInvisible()
    {
        if (removeOnlyOnce)
        {
            addOnlyOnce = true;
            removeOnlyOnce = false;
            targetController.nearByEnemies.Remove(this);
        }
    }
}