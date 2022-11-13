using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class roomEntry
{

    public RoomData room;
    public int chance;

    //Leave as -1 for there to be no cap
    public int maxPerLevel = -1;
    public bool required;

    //Leave as -1 for there to be no cap
    public float distanceFromSpawn = -1;

}
