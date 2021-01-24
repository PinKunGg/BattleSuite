using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Bullet : MonoBehaviour
{
    public Rigidbody rg;

    public Vector3 StartPos;

    public float BulletNum;
    public float BulletLength;
    public float BulletSpeed;

    protected string ConfigDataFile;
    public float BulletDamage;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = this.transform.position;

        rg.GetComponent<Rigidbody>();

        //ConfigData
        ConfigDataFile = Application.streamingAssetsPath + "/DataCSV/BulletDmg.csv";
        DataCall();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);

        if (Vector3.Distance(this.transform.position, StartPos) > BulletLength)
        {
            Destroy(this.gameObject);
        }

    }

    void DataCall()
    {
        StreamReader input = null;
        try
        {
            input = File.OpenText(ConfigDataFile);

            string BulletNumData = input.ReadLine();
            string BulletDamageData = input.ReadLine();

            while (BulletDamageData != null)
            {
                SetConfig(BulletDamageData);
                BulletDamageData = input.ReadLine();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            if (input != null)
            {
                input.Close();
            }
        }
    }
    void SetConfig(string csvValue)
    {
        string[] values = csvValue.Split(',');

        if(BulletNum == float.Parse(values[0]))
        {
            BulletDamage = float.Parse(values[1]);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Bullet")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        print("Who kill me");
    }
}
