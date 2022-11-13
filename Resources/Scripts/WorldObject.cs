using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{

    [Header("Object Tags")]
    [SerializeField] private List<string> objectTags = new List<string>();


    public void AddTag(string tag)
    {
        if (objectTags.Contains(tag))
        {
            return;
        }

        objectTags.Add(tag);
    }

    public bool ContainsTag(string id)
    {
        foreach (string tag in objectTags)
        {
            if (tag == id)
            {
                return true;
            }
        }

        return false;
    }

}
