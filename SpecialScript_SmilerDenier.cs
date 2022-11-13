using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialScript_SmilerDenier : MonoBehaviour
{
    public Transform resetLocat;
    public Entity_Smiler smiler;
    public DoorObject door;

    public void Execute()
    {
        Destroy(smiler);
    }
}
