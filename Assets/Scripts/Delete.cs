using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public Transform Controller;
    public float fireRate = 0.1f;
    float laserRange = 15f;
    WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    LineRenderer laserLine;
    float nextFire;
    bool activated = false;
    bool delete = false;

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
        if (Time.time > nextFire && activated == true)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = Controller.position;

            RaycastHit hit;
            laserLine.SetPosition(0, Controller.position);

            if (Physics.Raycast(Controller.position, Controller.forward, out hit, laserRange))
            {
                laserLine.SetPosition(1, hit.point);

                if (hit.collider.CompareTag("brick_1x1") || hit.collider.CompareTag("brick_2x1") || hit.collider.CompareTag("brick_4x1"))
                {
                    if (delete == true)
                    {
                        hit.collider.gameObject.SetActive(false);
                        delete = false;
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (Controller.forward * laserRange));
            }
        }
    }

    public void ActivateLaser()
    {
        activated = true;
        laserLine.enabled = true;
    }

    public void DeactivateLaser()
    {
        activated = false;
        laserLine.enabled = false;
    }

    public void DeleteBrick()
    {
        delete = true;
    }

    public void DeleteBrickFalse()
    {
        delete = false;
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
