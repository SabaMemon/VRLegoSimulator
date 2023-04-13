using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject offsetText;

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
    Vector3 newRot = new Vector3(0, 0, 0);
    bool brickPut = false;
    bool offsetMode = false;
    float offsetTime = 0f;
    float offsetRate = 0.5f;
    bool canOffset = false;
    WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    LineRenderer laserLine;
    float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        //laserLine.material = new Material(Shader.Find("Sprites/Default"));
        //laserLine.material.color = Color.red;

        offsetText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float yRotation = Controller.eulerAngles.y;
        Vector3 newPos = new Vector3(0, 0, 0);

        if (Time.time > offsetTime)
        {
            canOffset = true;
            offsetTime += offsetRate;
        }

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
                            if (offsetMode == false)
                            {
                                StackSameBrick(tempBrick, xAng, zAng, newPos, newRot);
                            }
                            else
                            {
                                if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                                {
                                    if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                    {
                                        if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                        {
                                            if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                        else
                                        {
                                            if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                    }
                                }
                                if (brickPut == true)
                                {
                                    brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                    brickObj = null;
                                    brickPut = false;
                                }
                            }
                        }
                        else if (brickObj.CompareTag("brick_1x1"))
                        {
                            if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                            {
                                if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                {
                                    if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                    {
                                        if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            //newPos = new Vector3(tempBrick.position.x + 0.125f, tempBrick.position.y + 0.2501f, tempBrick.position.z);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.25f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if (laserLine.GetPosition(1).x < tempBrick.position.x)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            // newPos = new Vector3(tempBrick.position.x - 0.125f, tempBrick.position.y + 0.2501f, tempBrick.position.z);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.25f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                    else
                                    {
                                        if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            //newPos = new Vector3(tempBrick.position.x, tempBrick.position.y + 0.2501f, tempBrick.position.z + 0.125f);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.25f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if (laserLine.GetPosition(1).z < tempBrick.position.z)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.25f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                }
                            }
                            if (brickPut == true)
                            {
                                brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                brickObj = null;
                                brickPut = false;
                            }
                        }
                        else if (brickObj.CompareTag("brick_4x1"))
                        {
                            if (offsetMode == false)
                            {
                                if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                                {
                                    if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                    {
                                        if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                        {
                                            if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).x < tempBrick.position.x)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                        else
                                        {
                                            if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).z < tempBrick.position.z)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                    }
                                }
                                if (brickPut == true)
                                {
                                    brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                    brickObj = null;
                                    brickPut = false;
                                }
                            }
                            else //OFFSET MODEEeee
                            {
                                if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                                {
                                    if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                    {
                                        if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                }
                                if (brickPut == true)
                                {
                                    brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                    brickObj = null;
                                    brickPut = false;
                                }
                            }
                        }
                    }
                    else if (hit.collider.CompareTag("brick_1x1"))
                    {
                        Transform tempBrick = hit.collider.GetComponent<Transform>();
                        int xAng = (int)tempBrick.eulerAngles.x;
                        int zAng = (int)tempBrick.eulerAngles.z;
                        int yAng = (int)tempBrick.eulerAngles.y;

                        if (brickObj.CompareTag("brick_1x1")) //For brick attached to controller
                        {
                            StackSameBrick(tempBrick, xAng, zAng, newPos, newRot);
                        }
                        else if (brickObj.CompareTag("brick_2x1"))
                        {
                            if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                            {
                                if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                {
                                    if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                    {
                                        if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if (laserLine.GetPosition(1).x < tempBrick.position.x)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                    else
                                    {
                                        if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if (laserLine.GetPosition(1).z < tempBrick.position.z)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                            brickObj.transform.localScale = new Vector3(2f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                }
                            }
                            if (brickPut == true)
                            {
                                brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                brickObj = null;
                                brickPut = false;
                            }
                        }
                    }
                    else if (hit.collider.CompareTag("brick_4x1"))
                    {
                        Transform tempBrick = hit.collider.GetComponent<Transform>();
                        int xAng = (int)tempBrick.eulerAngles.x;
                        int zAng = (int)tempBrick.eulerAngles.z;
                        int yAng = (int)tempBrick.eulerAngles.y;

                        if (brickObj.CompareTag("brick_4x1")) //For brick attached to controller
                        {
                            if (offsetMode == false)
                            {
                                StackSameBrick(tempBrick, xAng, zAng, newPos, newRot);
                            }
                            else
                            {
                                if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                                {
                                    if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                    {
                                        if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                        {
                                            if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                        else
                                        {
                                            if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(1f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                    }
                                }
                                if (brickPut == true)
                                {
                                    brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                    brickObj = null;
                                    brickPut = false;
                                }
                            }

                        }
                        else if (brickObj.CompareTag("brick_2x1"))
                        {
                            if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                            {
                                if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                {
                                    if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                    {
                                        if (offsetMode == false)
                                        {
                                            if (laserLine.GetPosition(1).x > tempBrick.position.x)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.25f, 1.01f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).x < tempBrick.position.x)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.25f, 1.01f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                        else // offset!
                                        {
                                            if (((laserLine.GetPosition(1).x > tempBrick.position.x) && (laserLine.GetPosition(1).x <= tempBrick.position.x + 0.25f)) ||
                                                ((laserLine.GetPosition(1).x <= tempBrick.position.x) && (laserLine.GetPosition(1).x >= tempBrick.position.x - 0.25f)))
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).x < tempBrick.position.x - 0.25f)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).x > tempBrick.position.x + 0.25f)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (offsetMode == false)
                                        {
                                            if (laserLine.GetPosition(1).z > tempBrick.position.z)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.25f, 1.01f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).z < tempBrick.position.z)
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.25f, 1.01f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                        else // offset!
                                        {
                                            if (((laserLine.GetPosition(1).z > tempBrick.position.z) && (laserLine.GetPosition(1).z <= tempBrick.position.z + 0.25f)) ||
                                                ((laserLine.GetPosition(1).z <= tempBrick.position.z) && (laserLine.GetPosition(1).z >= tempBrick.position.z - 0.25f)))
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).z < tempBrick.position.z - 0.25f) //right
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                            else if (laserLine.GetPosition(1).z > tempBrick.position.z + 0.25f) //left
                                            {
                                                tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                                newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                                                brickObj.transform.parent = tempBrick;
                                                brickObj.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
                                                brickObj.transform.localScale = new Vector3(0.5f, 1f, 1f);
                                                tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                                brickPut = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (brickPut == true)
                            {
                                brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                brickObj = null;
                                brickPut = false;
                            }
                        }
                        else if (brickObj.CompareTag("brick_1x1"))
                        {
                            if (((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.5f))
                            {
                                if (((xAng >= 0 && xAng <= 1) && (zAng >= 0 && zAng <= 1)) || (xAng >= 358 && xAng <= 359) && (zAng >= 358 && zAng <= 359))
                                {
                                    if ((yAng >= 0 && yAng < 46) || (yAng >= 136 && yAng < 226) || (yAng >= 316 && yAng < 360))
                                    {
                                        if (laserLine.GetPosition(1).x > tempBrick.position.x + 0.25f)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.375f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if ((laserLine.GetPosition(1).x > tempBrick.position.x) && (laserLine.GetPosition(1).x < tempBrick.position.x + 0.25f))
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.125f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if ((laserLine.GetPosition(1).x < tempBrick.position.x) && (laserLine.GetPosition(1).x > tempBrick.position.x - 0.25f))
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.125f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if (laserLine.GetPosition(1).x < tempBrick.position.x + 0.25f)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.375f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                    else
                                    {
                                        if (laserLine.GetPosition(1).z > tempBrick.position.z + 0.25f)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.375f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if ((laserLine.GetPosition(1).z > tempBrick.position.z) && (laserLine.GetPosition(1).z < tempBrick.position.z + 0.25f))
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(-0.125f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if ((laserLine.GetPosition(1).z < tempBrick.position.z) && (laserLine.GetPosition(1).z > tempBrick.position.z - 0.25f))
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.125f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                        else if (laserLine.GetPosition(1).z < tempBrick.position.z + 0.25f)
                                        {
                                            tempBrick.eulerAngles = new Vector3(xAng, 0f, zAng);
                                            newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);//DO NOT REMOVE  HAS TO BE AFTER SETTING TEMPBRICK ANGS
                                            brickObj.transform.parent = tempBrick;
                                            brickObj.transform.localPosition = new Vector3(0.375f, 1.01f, 0f);
                                            brickObj.transform.localScale = new Vector3(0.25f, 1f, 1f);
                                            tempBrick.eulerAngles = new Vector3(xAng, yAng, zAng);
                                            brickPut = true;
                                        }
                                    }
                                }
                            }
                            if (brickPut == true)
                            {
                                brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
                                brickObj = null;
                                brickPut = false;
                            }
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
            brickObj = null;
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
            brickObj.GetComponent<Rigidbody>().isKinematic = true;
            brickObj = null;
            brickPut = false;
        }
    }

    void BrickPut(bool brickPut)
    {
        print(brickPut);
        if (brickPut == true)
        {
            brickObj.transform.localEulerAngles = newRot; //DO NOT REMOVE
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

    public void DropBrick() //B drops brick
    {
        drop = true;
    }

    public void OffsetModeOn()
    {
        if (offsetMode == false && canOffset == true)
        {
            offsetText.SetActive(true);
            offsetMode = true;
            canOffset = false;
        }
    }

    public void OffsetModeOff()
    {
        if (offsetMode == true && canOffset == true)
        {
            offsetText.SetActive(false);
            offsetMode = false;
            canOffset = false;
        }
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}