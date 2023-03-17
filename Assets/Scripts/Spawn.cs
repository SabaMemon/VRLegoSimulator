using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour
{
    GameObject brickObj = null; //made public because it's referenced in multiple functions
    public GameObject brickPrefab;
    public Transform Controller;

    float brickDistance = 0.1f;
    //Vector3 initialScale = new Vector3(0.3f, 0.3f, 0.3f);
    //Vector3 finallScale = new Vector3(1.0f, 1.0f, 1.0f);

 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (brickObj != null)
        {
            brickObj.transform.parent = null;
            brickObj.GetComponent<Rigidbody>().isKinematic = false;
            //brickObj.transform.position = brickObj.transform.position;
            brickObj.transform.localRotation = Quaternion.identity;
        }
        
    }


    public void InitializeBrick(Material brickMat)
    {
        brickObj = GameObject.Instantiate(brickPrefab);
        brickObj.GetComponent<MeshRenderer>().material = brickMat;
        brickObj.transform.position = Controller.transform.position + Controller.transform.forward * brickDistance;
        brickObj.transform.localRotation = transform.rotation;
        brickObj.transform.Rotate(0, 0, 0, Space.Self);
        brickObj.transform.parent = Controller.transform;
        brickObj.GetComponent<Rigidbody>().isKinematic = true;
    }


}