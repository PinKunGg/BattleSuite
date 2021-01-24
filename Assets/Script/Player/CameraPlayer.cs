using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    #region ตัวแปร
    public float minimumX = -90f;
    public float maximumX = 90f;

    public float minimumY = -90f;
    public float maximumY = 90f;

    float rotationY = 0F;

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 80f;
    public float sensitivityY = 80f;

    public Transform targetpos;
    #endregion

    void Start()
    {

    }
    public enum RotationAxes
    {
        MouseXAndY = 0, MouseX = 1, MouseY = 2
    }
    void Update()
    {
        if (Cursor.visible == true)
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = new float();

                if (transform.parent != null)
                {
                    rotationX = transform.parent.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    transform.parent.transform.localEulerAngles = new Vector3(0, rotationX, 0);
                    transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
                }
            }
            else if (axes == RotationAxes.MouseX)
            {
                if (transform.parent != null)
                {
                    transform.parent.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
                }
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }
    }
}
