using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClipper : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Entity.Player")
        {
            collision.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        }
    }
}
