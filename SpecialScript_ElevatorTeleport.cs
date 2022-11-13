using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialScript_ElevatorTeleport : MonoBehaviour
{
    public GameObject player;
    public DoorObject elevDoors;

    public void TPPlayer()
    {
        Invoke("TP", 5);
    }

    void TP()
    {
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y -5, player.transform.position.z);
        Invoke("ToggleDoors", 1);
    }
    void ToggleDoors()
    {
        elevDoors.Open();
    }
}
