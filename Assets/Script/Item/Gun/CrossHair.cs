using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ChangeDotMaterial(float GunMode)
    {
        if (GunMode == 1)
        {
            this.GetComponent<Image>().color = new Color32(255, 51, 51, 255);
        }
        if (GunMode == 2)
        {
            this.GetComponent<Image>().color = new Color32(255, 51, 255, 255);
        }
        if (GunMode == 3)
        {
            this.GetComponent<Image>().color = new Color32(51, 255, 51, 255);
        }
        if (GunMode == 4)
        {
            this.GetComponent<Image>().color = new Color32(255, 255, 51, 255);
        }
        if (GunMode == 5)
        {
            this.GetComponent<Image>().color = new Color32(255, 153, 51, 255);
        }
    }
}
