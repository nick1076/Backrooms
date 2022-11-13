using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityZone : MonoBehaviour
{
    public enum zoneTypeData
    {
        slowRegen,
        set
    };

    public zoneTypeData zoneType;
    public float amount;
    public float cooldown;

    Coroutine regen;
    SanityManager playerSan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Entity.Player")
        {
            playerSan = other.gameObject.GetComponent<SanityManager>();

            if (zoneType == zoneTypeData.set)
            {
                playerSan.AlterSanity(amount);
            }
            else if (zoneType == zoneTypeData.slowRegen)
            {
                regen = StartCoroutine(Regenerate(playerSan));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Entity.Player")
        {
            playerSan = other.gameObject.GetComponent<SanityManager>();

            if (zoneType == zoneTypeData.slowRegen)
            {
                StopCoroutine(regen);
                regen = null;
            }
        }
    }

    IEnumerator Regenerate(SanityManager sanMan)
    {
        yield return new WaitForSeconds(cooldown);
        sanMan.AlterSanity(amount);
        regen = StartCoroutine(Regenerate(sanMan));
    }
}