using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public GameObject onCollideEffect;
    public float toneDifferenceRange = 0.25f;
    public List<AudioClip> potentialSounds = new List<AudioClip>();
    public bool propItem = true;
    bool canCreateEffects = false;

    private void Start()
    {
        if (propItem)
        {
            StartCoroutine(allowEffects());
        }
        else
        {
            canCreateEffects = true;
        }
    }

    IEnumerator allowEffects()
    {
        yield return new WaitForSeconds(1);
        canCreateEffects = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (onCollideEffect != null && canCreateEffects)
        {
            GameObject eff = Instantiate(onCollideEffect, this.transform.position, this.transform.rotation);

            if (eff.GetComponent<AudioSource>() != null)
            {
                eff.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(onCollideEffect.GetComponent<AudioSource>().pitch - toneDifferenceRange, onCollideEffect.GetComponent<AudioSource>().pitch + toneDifferenceRange);

                if (potentialSounds.Count > 1)
                {
                    eff.GetComponent<AudioSource>().clip = potentialSounds[UnityEngine.Random.Range(0, potentialSounds.Count)];
                    eff.GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
