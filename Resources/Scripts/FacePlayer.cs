using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    public bool allAxis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allAxis)
        {
            if (GameObject.FindWithTag("Entity.Player") != null)
            {
                transform.LookAt(GameObject.FindWithTag("Entity.Player").transform);
            }
        }
        else
        {
            if (GameObject.FindWithTag("Entity.Player") != null)
            {
                transform.LookAt(GameObject.FindWithTag("Entity.Player").transform);
                this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            }
        }
    }
}
