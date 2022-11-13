using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    public Transform raycastOrigin;
    public float damage;
    public float knockback;
    public float range;
    public bool playerControlled = true;
    public List<string> ignoreTags = new List<string>();

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

        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.transform.position, raycastOrigin.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.distance <= range)
            {/*
                if (hit.collider.gameObject.GetComponent<Entity>() != null)
                {
                    Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    if (!ignoreTags.Contains(hit.collider.gameObject.GetComponent<Entity>().GetName()))
                    {
                        hit.collider.gameObject.GetComponent<Entity>().ModifyHealth(damage);

                        if (hit.collider.gameObject.GetComponent<Entity>().hitEffect != null)
                        {
                            GameObject created = Instantiate(hit.collider.gameObject.GetComponent<Entity>().hitEffect, hit.point, this.transform.rotation);
                            created.transform.Rotate(0, 180, 0);
                        }

                        if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            hit.collider.gameObject.GetComponent<Rigidbody>().velocity = this.transform.forward * knockback;
                        }

                        OnWeaponUsed(true);
                        return;
                    }
                }
                else if (hit.collider.gameObject.GetComponent<ObjectMat>() != null)
                {
                    Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.TransformDirection(Vector3.forward) * 1000, Color.cyan);
                    if (hit.collider.gameObject.GetComponent<ObjectMat>().material.projectileHitEffect != null)
                    {
                        GameObject created = Instantiate(hit.collider.gameObject.GetComponent<ObjectMat>().material.projectileHitEffect, hit.point, this.transform.rotation);
                        created.transform.Rotate(0, 180, 0);

                        OnWeaponUsed(true);
                        return;
                    }
                }
                else
                {
                    Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.TransformDirection(Vector3.forward) * 1000, Color.yellow);
                }
                */
            }


            OnWeaponUsed(false);
        }
    }

    public virtual void OnWeaponUsed(bool hit)
    {

    }
}
