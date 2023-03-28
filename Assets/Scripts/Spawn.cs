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
    bool drop = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float yRotation = Controller.eulerAngles.y;
        if (brickObj != null && drop == true)
        {
            brickObj.transform.eulerAngles = new Vector3(0, yRotation, 0);
            brickObj.transform.parent = null;
            brickObj.GetComponent<Rigidbody>().isKinematic = false;
            //brickObj.transform.position = brickObj.transform.position;
            drop = false;
        }
        
    }


    public void InitializeBrick(Material brickMat)
    {
        brickObj = GameObject.Instantiate(brickPrefab);
        brickObj.GetComponent<MeshRenderer>().material = brickMat;
        brickObj.transform.position = Controller.position + Controller.forward * brickDistance;
        brickObj.transform.localRotation = Controller.rotation;
        brickObj.transform.Rotate(0, 0, 0, Space.Self);
        brickObj.transform.parent = Controller;
        brickObj.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void DropBrick()
    {
        drop = true;
    }

}