using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour
{
    GameObject brickObj = null; //made public because it's referenced in multiple functions
    public GameObject brickPrefab;
    public Transform Controller;
    public float fireRate = 0.1f;
    public float laserRange = 1f;

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

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = Controller.position;

            RaycastHit hit;
            laserLine.SetPosition(0, Controller.position);

            if (Physics.Raycast(Controller.position, Controller.forward, out hit, laserRange))
            {
                laserLine.SetPosition(1, hit.point);
                
                if (hit.collider.CompareTag("Brick"))
                {
                    Transform tempBrick = hit.collider.GetComponent<Transform>();
                    //print((int)tempBrick.eulerAngles.x);
                    //print((int)tempBrick.eulerAngles.z);
                    
                    if ((int)tempBrick.eulerAngles.x == 0 && (int)tempBrick.eulerAngles.z == 0 && ((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude <= 0.75f) && brickObj != null && tempBrick.parent == null)
                    {
                        //print((laserLine.GetPosition(1) - laserLine.GetPosition(0)).magnitude);
                        brickObj.transform.parent = null;
                        brickObj.GetComponent<Rigidbody>().isKinematic = false;
                        Vector3 newPos = new Vector3(tempBrick.position.x, tempBrick.position.y + 0.35f, tempBrick.position.z);
                        //Vector3 newRot = new Vector3(tempBrick.eulerAngles.x, tempBrick.eulerAngles.y, tempBrick.eulerAngles.z);
                        brickObj.transform.position = newPos;
                        //brickObj.transform.eulerAngles = newRot;
                        //brickObj.transform.position.y = tempBrick.transform.position.y + 0.35f;
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (Controller.forward * laserRange));
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

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

}