using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_Cycle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Rotate(0.5f * Time.deltaTime, 0f * Time.deltaTime, 0f * Time.deltaTime);
        transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime * 2f);
        transform.LookAt(Vector3.zero);
    }
}
