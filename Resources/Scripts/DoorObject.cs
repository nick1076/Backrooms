using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour
{

    bool playerWithin;
    public Animation anim;
    public AnimationClip clip;
    public AnimationClip clipClose;
    public BoxCollider triggopen;
    public bool manual;
    public bool stopAfterOpening;

    [HideInInspector] public bool open;

    private void Start()
    {
        anim.AddClip(clip, "Open");
        anim.AddClip(clipClose, "Close");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entity.Player")
        {
            playerWithin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Entity.Player")
        {
            playerWithin = false;
        }
    }

    private void Update()
    {
        if (playerWithin)
        {
            if (!manual)
            {
                Open();
                Destroy(triggopen);
                playerWithin = false;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (open)
                    {
                        Close();
                    }
                    else
                    {
                        Open();
                    }
                }
            }
        }
    }

    public void Open()
    {
        open = true;
        anim.Play("Open");
        if (stopAfterOpening)
        {
            Destroy(this);
        }
    }

    public void Close()
    {
        open = false;
        anim.Play("Close");
    }
}
