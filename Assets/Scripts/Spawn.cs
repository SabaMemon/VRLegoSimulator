using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//https://stackoverflow.com/questions/40752083/how-to-find-child-of-a-gameobject-or-the-script-attached-to-child-gameobject-via 

public class Spawn : MonoBehaviour
{
    GameObject brickObj = null; //made public because it's referenced in multiple functions
    public GameObject brickPrefab;
    public Transform Controller;

    public float fireRate = 0.1f;
    float laserRange = 1.5f;

    public Material brickMat;
    public Material blueBrick;
    public Material greenBrick;
    public Material yellowBrick;
    public Material redBrick;
    public Material purpleBrick;

    float brickDistance = 0.1f;
    bool drop = false;
    WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    LineRenderer laserLine;
    float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.material = new Material(Shader.Find("Sprites/Default"));
        laserLine.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        float yRotation = Controller.eulerAngles.y;
        Vector3 newRot = new Vector3(0, 0, 0);
        bool brickPut = false;

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = Controller.position;

            RaycastHit hit;
            laserLine.SetPosition(0, Controller.position);

            if (Physics.Raycast(Controller.position, (-1f) * Controller.up, out hit, laserRange))
            {
                laserLine.SetPosition(1, hit.point);

                if (hit.collider.CompareTag("Brick"))
                {
                    print("brick found");
                    Transform tempBrick = hit.collider.GetComponent<Transform>();

                    if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 1f))
                    {
                        print("position works");
                        /*print("xAng = " + (int)tempBrick.eulerAngles.x);
                        print("zAng = " + (int)tempBrick.eulerAngles.z);
                        if (tempBrick.rotation.x < 0 && (int)tempBrick.eulerAngles.x >= 350)
                        {
                            tempBrick.eulerAngles = new Vector3(0, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                        }
                        if (tempBrick.rotation.z < 0 && (int)tempBrick.eulerAngles.z >= 350)
                        {
                            tempBrick.eulerAngles = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, 0);
                        }
                        print("xAng2 = " + (int)tempBrick.eulerAngles.x);
                        print("zAng2 = " + (int)tempBrick.eulerAngles.z);*/
                        int xAng = (int)tempBrick.eulerAngles.x;
                        int zAng = (int)tempBrick.eulerAngles.z;
                        if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                        {
                            print("angles work");
                            if (brickObj != null)
                            {
                                if (tempBrick.parent == null)
                                {
                                    brickObj.transform.parent = null;
                                    brickObj.GetComponent<Rigidbody>().isKinematic = false;
                                    Vector3 newPos = new Vector3(tempBrick.position.x, tempBrick.position.y + 0.36f, tempBrick.position.z);
                                    newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                    brickObj.transform.position = newPos;
                                    brickPut = true;
                                }
                            }
                        }
                    }
                    if (brickPut == true)
                    {
                        brickObj.transform.eulerAngles = newRot;
                        brickPut = false;
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + ((-1f) * Controller.up * laserRange));
            }
        }

        if (brickObj != null && drop == true)
        {
            brickObj.transform.eulerAngles = new Vector3(0, yRotation, 0);
            brickObj.transform.parent = null;
            brickObj.GetComponent<Rigidbody>().isKinematic = false;
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
                brickMat = blueBrick;
                break;
            case 5:
                brickMat = purpleBrick;
                break;
            default:
                brickMat = redBrick;
                break;
        }
    }

    public void InitializeBrick()
    {
        brickObj = GameObject.Instantiate(brickPrefab);

        brickObj.GetComponent<MeshRenderer>().material = brickMat;
        for (int i = 0; i < brickObj.transform.childCount; i++)
        {
            GameObject child = brickObj.transform.GetChild(i).gameObject;
            child.GetComponent<MeshRenderer>().material = brickMat;
        }

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

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

}


