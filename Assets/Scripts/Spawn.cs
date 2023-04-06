using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//https://stackoverflow.com/questions/40752083/how-to-find-child-of-a-gameobject-or-the-script-attached-to-child-gameobject-via 

public class Spawn : MonoBehaviour
{
    GameObject brickObj = null; //made public because it's referenced in multiple functions
    public GameObject brickPrefab;
    public GameObject brick1x1Prefab;
    public GameObject brick2x1Prefab;
    public GameObject brick4x1Prefab;
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
    bool brickPut = false;
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
        Vector3 newPos = new Vector3(0, 0, 0);

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

                if (brickObj != null)
                {
                    if (hit.collider.CompareTag("brick_2x1"))
                    {
                        Transform tempBrick = hit.collider.GetComponent<Transform>();
                        int xAng = (int)tempBrick.eulerAngles.x;
                        int zAng = (int)tempBrick.eulerAngles.z;
                        int yAng = (int)tempBrick.eulerAngles.y;

                        if (brickObj.CompareTag("brick_2x1")) //For brick attached to controller
                        {
                            StackSameBrick(tempBrick, xAng, zAng, newPos, newRot);
                        }
                        else if (brickObj.CompareTag("brick_1x1"))
                        {
                            if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.75f))
                            {
                                if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                {
                                    if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                    {
                                        print("check x");
                                        if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                        {
                                            newPos = new Vector3(tempBrick.position.x + 0.125f, tempBrick.position.y + 0.2501f, tempBrick.position.z);
                                        }
                                        else if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                        {
                                            newPos = new Vector3(tempBrick.position.x - 0.125f, tempBrick.position.y + 0.2501f, tempBrick.position.z);
                                        }
                                    }
                                    else
                                    {
                                        print("check z");
                                        if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                        {
                                            newPos = new Vector3(tempBrick.position.x, tempBrick.position.y + 0.2501f, tempBrick.position.z + 0.125f);
                                        }
                                        else if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                        {
                                            newPos = new Vector3(tempBrick.position.x, tempBrick.position.y + 0.2501f, tempBrick.position.z - 0.125f);
                                        }
                                    }
                                    newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                    brickObj.transform.position = newPos;
                                    brickPut = true;
                                }
                            }
                            if (brickPut == true)
                            {
                                brickObj.transform.eulerAngles = newRot;
                                brickObj.transform.parent = tempBrick;
                                brickObj = null;
                                brickPut = false;
                            }
                        }
                        else if (brickObj.CompareTag("brick_4x1"))
                        {

                        }
                    }
                    else if (hit.collider.CompareTag("brick_1x1"))
                    {
                        Transform tempBrick = hit.collider.GetComponent<Transform>();
                        int xAng = (int)tempBrick.eulerAngles.x;
                        int zAng = (int)tempBrick.eulerAngles.z;

                        if (brickObj.CompareTag("brick_1x1")) //For brick attached to controller
                        {
                            StackSameBrick(tempBrick, xAng, zAng, newPos, newRot);
                        }
                    }
                    else if (hit.collider.CompareTag("brick_4x1"))
                    {
                        Transform tempBrick = hit.collider.GetComponent<Transform>();
                        int xAng = (int)tempBrick.eulerAngles.x;
                        int zAng = (int)tempBrick.eulerAngles.z;

                        if (brickObj.CompareTag("brick_4x1")) //For brick attached to controller
                        {
                            StackSameBrick(tempBrick, xAng, zAng, newPos, newRot);
                        }
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

    void StackSameBrick(Transform tempBrick, int xAng, int zAng, Vector3 newPos, Vector3 newRot)
    {
        if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.75f))
        {
            if (((xAng >= 0 && xAng <= 1) || (xAng >= 358 && xAng < 360)) && ((zAng >= 0 && zAng <= 1) || (zAng >= 358 && zAng < 360)))
            {
                newPos = new Vector3(tempBrick.position.x, tempBrick.position.y + 0.2501f, tempBrick.position.z);
                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                brickObj.transform.position = newPos;
                brickPut = true;
            }
        }
        if (brickPut == true)
        {
            brickObj.transform.eulerAngles = newRot;
            brickObj.transform.parent = tempBrick;
            brickObj = null;
            brickPut = false;
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

    public void SetBrick(int size)
    {
        switch (size)
        {
            case 1:
                brickPrefab = brick1x1Prefab;
                break;
            case 2:
                brickPrefab = brick2x1Prefab;
                break;
            case 3:
                brickPrefab = brick4x1Prefab;
                break;
            default:
                brickPrefab = brick2x1Prefab;
                break;
        }
    }

    public void InitializeBrick()
    {
        if (brickObj == null)
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
