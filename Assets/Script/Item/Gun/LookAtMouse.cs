using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField]
    private Transform GunPos;
    [SerializeField]
    private Camera Cam;
    public GameObject CrossHair;

    RaycastHit hitinfo;

    targetController LockOn;

    private void Start()
    {
        LockOn = GameObject.Find("LockOnTarget").GetComponent<targetController>();
    }

    private void FixedUpdate()
    {
        Ray rayOrigin = Cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayOrigin, out hitinfo, Mathf.Infinity))
        {
            if (hitinfo.collider != null && LockOn.lockedOn == false)
            {
                Vector3 direction = hitinfo.point - GunPos.position;
                GunPos.rotation = Quaternion.LookRotation(direction);
            }
            if(hitinfo.collider == null)
            {
                GunPos.rotation = Quaternion.Euler(0f,0f,0f);
            }
        }
    }
}
