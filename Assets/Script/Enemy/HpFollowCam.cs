using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpFollowCam : MonoBehaviour
{
    Transform cam;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }
    void LateUpdate()
    {
        try
        {
            transform.LookAt(transform.position + cam.forward);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            this.gameObject.SetActive(false);
        }
    }
}
