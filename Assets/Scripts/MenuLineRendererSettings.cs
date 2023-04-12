using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLineRendererSettings : MonoBehaviour
{
    public Transform Controller;
    public GameObject Menu;

    public float fireRate = 0.1f;
    float laserRange = 10f;
    WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    LineRenderer laserLine;
    float nextFire;

    float menuTime = 0f;
    float menuRate = 0.5f;

    bool canMenu = false;

    bool activated = false;
    bool menuOn = true;
    public Button btn;
    public GameObject brickSize;
    Color laserColor = Color.red;
    public int brickColor = 1;
    public int brickSizeSetting = 1;
    public Spawn spawn;


    // Start is called before the first frame update
    void Start()
    {
        Menu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (laserLine != null)
        {
            laserLine.material.color = laserColor;
        }
        if (Time.time > menuTime)
        {
            canMenu = true;
            menuTime += menuRate;
        }
        if (Time.time > nextFire && activated == true)
        {
            canMenu = true;
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
                }
                else if (hit.collider.CompareTag("1x1_btn") || hit.collider.CompareTag("2x1_btn") || hit.collider.CompareTag("4x1_btn"))
                {
                    BrickChangeOnClick(hit);
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

    public void ActivateMenu()
    {
        if (menuOn == false && canMenu == true)
        {
            Menu.SetActive(true);
            menuOn = true;
            canMenu = false;
        }
    }

    public void DeactivateMenu()
    {
        if (menuOn && canMenu == true)
        {
            Menu.SetActive(false);
            menuOn = false;
            canMenu = false;
        }
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

    public void BrickChangeOnClick(RaycastHit hit)
    {
        brickSize = hit.collider.gameObject;

        if (brickSize != null)
        {
            if (brickSize.CompareTag("1x1_btn"))
            {
                brickSizeSetting = 1;
            }
            else if (brickSize.CompareTag("2x1_btn"))
            {
                brickSizeSetting = 2;
            }
            else if (brickSize.CompareTag("4x1_btn"))
            {
                brickSizeSetting = 3;
            }
            else
            {
                brickSizeSetting = 1;
            }
        }
        spawn.SetBrick(brickSizeSetting);
    }
}