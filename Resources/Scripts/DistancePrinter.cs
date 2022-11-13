using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePrinter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Entity.Player").transform.position));

        if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Entity.Player").transform.position) > 30)
        {
            this.GetComponent<Light>().enabled = false;
        }
        else
        {
            this.GetComponent<Light>().enabled = true;
        }
    }
}
