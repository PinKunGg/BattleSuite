using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class targetController : MonoBehaviour
{
    #region ตัวแปร
    [SerializeField]
    Camera cam; //Main Camera

    public GameObject GunPos;

    public enemyInView target; //Current Focused Enemy In List
    public Image image;//Image Of Crosshair

    public bool lockedOn;//Keeps Track Of Lock On Status    

    //Tracks Which Enemy In List Is Current Target
    public int lockedEnemy;

    //List of nearby enemies
    public static List<enemyInView> nearByEnemies = new List<enemyInView>();

    public bool Lock = true;
    #endregion

    void Start()
    {
        nearByEnemies.Clear();
        image = gameObject.GetComponent<Image>();
        image.enabled = false;

        lockedOn = false;
        lockedEnemy = 0;
    }
    void Update()
    {
        LockTarget();
        Tracking();
    }

    void LockTarget()
    {
        //Press RightClick Key To Lock On Target
        if (Input.GetMouseButtonDown(1) && !lockedOn)
        {
            if (nearByEnemies.Count >= 1)
            {
                lockedOn = true;
                image.enabled = true;

                //Lock On To First Enemy In List By Default
                lockedEnemy = 0;
                target = nearByEnemies[lockedEnemy];
            }
        }
        //Press RightClick Key To Un Lock Target
        else if ((Input.GetMouseButtonDown(1) && lockedOn) || nearByEnemies.Count == 0)
        {
            lockedOn = false;
            image.enabled = false;
            lockedEnemy = 0;
            target = null;
        }

        //Press X To Switch Targets
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (lockedEnemy == nearByEnemies.Count - 1)
            {
                //If End Of List Has Been Reached, Start Over
                lockedEnemy = 0;
                target = nearByEnemies[lockedEnemy];
            }
            else
            {
                //Move To Next Enemy In List
                lockedEnemy++;
                target = nearByEnemies[lockedEnemy];
            }
        }
    }
    void Tracking()
    {
        if (lockedOn)
        {
            try
            {
                target = nearByEnemies[lockedEnemy];
                GunPos.transform.LookAt(target.transform);

                //Determine Crosshair Location Based On The Current Target
                gameObject.transform.position = cam.WorldToScreenPoint(target.transform.position);

                //Rotate Crosshair
                gameObject.transform.Rotate(new Vector3(0f, 0f, -100f * Time.deltaTime));
            }
            catch
            {
                print("Out of Range");
                lockedOn = false;
                image.enabled = false;
                lockedEnemy = 0;
                target = null;
            }
        }
    }
}