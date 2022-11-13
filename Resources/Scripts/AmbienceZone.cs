using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceZone : MonoBehaviour
{

    public FpsControllerLPFP player;
    public int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entity.Player")
        {
            player.SetAmbience(id);
        }
    }
}
