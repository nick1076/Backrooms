using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_SafeEnding : MonoBehaviour
{
    GameObject player;
    public GameObject cutsceneCamera;
    public GameObject cutsceneUI;
    bool mouseOver;
    bool within;
    bool began;

    private void Start()
    {
        player = GameObject.FindWithTag("Entity.Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entity.Player")
        {
            within = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Entity.Player")
        {
            within = false;
        }
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    private void Update()
    {
        if (mouseOver && within)
        {
            cutsceneUI.SetActive(true);
        }
        else
        {
            cutsceneUI.SetActive(false);
        }

        if (mouseOver && within)
        {
            if (!began)
            {
                cutsceneUI.SetActive(true);
            }
            else
            {
                cutsceneUI.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {

                PlayerPrefs.SetInt("Data.Achievement.Butterflies", 1);
                began = true;
                cutsceneCamera.SetActive(true);
                player.SetActive(false);
            }
        }
    }
}
