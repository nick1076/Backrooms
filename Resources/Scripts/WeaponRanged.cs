using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRanged : Weapon
{

    public Transform throwableOrigin;
    public GameObject projectile;
    public float force;
    public bool playerControlled = true;
    public List<string> projectileIgnoreTags;
    private void Start()
    {
        if (playerControlled)
        {
            isPlayers = true;
        }
        else
        {
            isPlayers = false;
        }
    }

    public override void UseWeapon()
    {
        base.UseWeapon();

        GameObject madeProjectile = Instantiate(projectile, throwableOrigin.transform.position, throwableOrigin.transform.rotation);

        if (madeProjectile.GetComponent<Rigidbody>() != null)
        {
            madeProjectile.GetComponent<Rigidbody>().velocity = throwableOrigin.transform.forward * force;
        }

        OnWeaponUsed();
    }

    public virtual void OnWeaponUsed()
    {

    }
}
