using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="New Room", fileName="newRoom")]
public class RoomData : ScriptableObject
{

    public bool northConnectionAllowed = false;
    public bool southConnectionAllowed = false;
    public bool eastConnectionAllowed = false;
    public bool westConnectionAllowed = false;

    public GameObject prefab;

}
