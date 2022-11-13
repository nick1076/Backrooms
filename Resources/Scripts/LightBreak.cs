using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBreak : MonoBehaviour
{

    private List<GameObject> lightBreakers = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Entity.Smiler")
        {
            print("SMILER ENTERED");
            lightBreakers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Entity.Smiler" && lightBreakers.Contains(other.gameObject))
        {
            lightBreakers.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (lightBreakers.Count > 0)
        {
            this.gameObject.GetComponent<Light>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<Light>().enabled = true;
        }
    }
}
