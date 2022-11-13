using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float cooldownPlayer;
    public float cooldownEntities;
    public bool isPlayers;
    [HideInInspector] public bool disabled = false;

    public void TryUseWeapon()
    {
        if (disabled)
        {
            return;
        }
        else
        {
            UseWeapon();
            disabled = true;
            StartCoroutine(Cooldown());
        }
    }

    public virtual void UseWeapon()
    {

    }

    IEnumerator Cooldown()
    {
        if (isPlayers)
        {
            yield return new WaitForSeconds(cooldownPlayer);
        }
        else
        {
            yield return new WaitForSeconds(cooldownEntities);
        }
        disabled = false;
    }
}
