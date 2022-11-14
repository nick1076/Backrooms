using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public GameObject onCollideEffect;
    public float toneDifferenceRange = 0.25f;

    private void OnCollisionEnter(Collision collision)
    {
        if (onCollideEffect != null)
        {
            Instantiate(onCollideEffect, this.transform.position, this.transform.rotation);

            if (onCollideEffect.GetComponent<AudioSource>() != null)
            {
                onCollideEffect.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(onCollideEffect.GetComponent<AudioSource>().pitch - toneDifferenceRange, onCollideEffect.GetComponent<AudioSource>().pitch + toneDifferenceRange);
            }
        }
    }
}
