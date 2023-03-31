using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLineRendererSettings : MonoBehaviour
{
    public Transform Controller;
    public float fireRate = 0.1f;
    public float laserRange = 1f;

    WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    LineRenderer laserLine;
    float nextFire;
    bool activated = false;
    public Button btn;
    

    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.material = new Material(Shader.Find("Sprites/Default"));
        laserLine.material.color = Color.green;
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
            }
            else
            {
                print("activated");
                laserLine.SetPosition(1, rayOrigin + (Controller.forward * laserRange));
            }
        }
    }

    public void ActivateLaser()
    {
        activated = true;
        laserLine.enabled = true;
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

    public void ColorChangeOnClick()
    {
        btn = hit.collider.gameObject.GetComponent<Button>();

        if (btn != null)
        {
            if (btn.name == "red_btn")
            {
                img.color = Color.red;
            }
            else if (btn.name == "blue_btn")
            {
                img.color = Color.blue;
            }
            else if (btn.name == "green_btn")
            {
                img.color = Color.green;
            }
        }
    }

}

