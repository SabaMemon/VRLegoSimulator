using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour
{
    GameObject brickObj = null; //made public because it's referenced in multiple functions
    public GameObject brickPrefab;
    public Transform Controller;
    public Material brickMat;
    public Material blueBrick;
    public Material greenBrick;
    public Material yellowBrick;
    public Material redBrick;
    public Material purpleBrick;
    public MenuLineRendererSettings menu;


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

    public void SetBrickMat(int color)
    {
        switch (color)
        {
            case 1:
                brickMat = redBrick;
                break;
            case 2:
                brickMat = yellowBrick;
                break;
            case 3:
                brickMat = greenBrick;
                break;
            case 4:
                brickMat = purpleBrick;
                break;
            case 5:
                brickMat = blueBrick;
                break;
            default:
                brickMat = redBrick;
                break;
        }
        print("brick mat: " + brickMat);
        print("menu color num: " + color);
    }

    public void InitializeBrick()
    {
        brickObj = GameObject.Instantiate(brickPrefab);
        print("MAT AFTER SETTING" + brickMat);
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