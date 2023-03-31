using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLineRendererSettings : MonoBehaviour
{
    public Transform Controller;
    public float fireRate = 0.1f;
    float laserRange = 20f;
    // RANGE NOT LONG ENOUGH - NUMBER NOT CHANGING RANGE??*******************************************

    WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    LineRenderer laserLine;
    float nextFire;
    bool activated = false;
    public Button btn;
    Color laserColor = Color.red;
    public int brickColor = 1;
    public Spawn spawn;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (laserLine != null)
        {
            laserLine.material.color = laserColor;
        }
        if (Time.time > nextFire && activated == true)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = Controller.position;

            RaycastHit hit;
            laserLine.SetPosition(0, Controller.position);

            if (Physics.Raycast(Controller.position, Controller.forward, out hit, laserRange))
            //if (Physics.Raycast(Controller.position, Controller.forward, out hit, 10f))
            {
                laserLine.SetPosition(1, hit.point);
                if (hit.collider.CompareTag("menu_btn"))
                {
                    ColorChangeOnClick(hit);
                    print(laserColor);
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
        laserLine = GetComponent<LineRenderer>();
        laserLine.material = new Material(Shader.Find("Sprites/Default"));
        laserLine.material.color = laserColor;
        activated = true;
        laserLine.enabled = true;
    }

    public void DeactivateLaser()
    {
        activated = false;
        laserLine.enabled = false;
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

    public void ColorChangeOnClick(RaycastHit hit)
    {
        btn = hit.collider.gameObject.GetComponent<Button>();

        if (btn != null)
        {
            if (btn.name == "red_btn")
            {
                laserColor = Color.red;
                brickColor = 1;
            }
            else if (btn.name == "yellow_btn")
            {
                laserColor = Color.yellow;
                brickColor = 2;
            }
            else if (btn.name == "green_btn")
            {
                laserColor = Color.green;
                brickColor = 3;
            }
            else if (btn.name == "blue_btn")
            {
                laserColor = Color.blue;
                brickColor = 4;
            }
            else if (btn.name == "purple_btn")
            {
                laserColor = Color.magenta;
                brickColor = 5;
            }
            else
            {
                brickColor = 1;
            }
        }
        spawn.SetBrickMat(brickColor);
    }
    //switch (expression)
    //{
    //    case 1:

    //        laserLine.material.color = Color.yellow;
    //        break;
    //    case 2:
    //        laserLine.material.color = Color.green;
    //        break;
    //    case 3:
    //        laserLine.material.color = Color.blue;
    //        break;
    //    case 4:
    //        laserLine.material.color = Color.purple;
    //        break;
    //    default:
    //        laserLine.material.color = Color.red;
    //        break;
    //}
}