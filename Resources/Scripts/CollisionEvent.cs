using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{

    public string use = "General.Player";
    public UnityEvent onContactEvent;
    public UnityEvent onLostContactEvent;
    public bool destroyOnTrigger;
    public bool destroyJustComponent;

    private void Start()
    {
        if (this.GetComponent<MeshRenderer>() != null)
        {
            Destroy(this.GetComponent<MeshRenderer>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (use == "General.Player")
        {
            if (other.tag == "Entity.Player")
            {
                onContactEvent.Invoke();

                if (destroyOnTrigger)
                {
                    if (destroyJustComponent)
                    {
                        Destroy(this);
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }
        else if (use == "Special.Smiler.Turnback")
        {
            if (other.tag == "Entity.Smiler")
            {
                onContactEvent.Invoke();

                if (destroyOnTrigger)
                {
                    if (destroyJustComponent)
                    {
                        Destroy(this);
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (use == "General.Player")
        {
            if (other.tag == "Entity.Player")
            {
                onLostContactEvent.Invoke();
            }

            if (destroyOnTrigger)
            {
                if (destroyJustComponent)
                {
                    Destroy(this);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
