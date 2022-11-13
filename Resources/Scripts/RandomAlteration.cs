using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAlteration : MonoBehaviour
{

    public GameObject[] possibleAlterations;

    void Start()
    {
        int selection = UnityEngine.Random.Range(0, possibleAlterations.Length);
        GameObject selectionObj = possibleAlterations[selection];
        possibleAlterations[selection].SetActive(true);
        possibleAlterations[selection] = null;
        for (int i = 0; i < possibleAlterations.Length; i++)
        {
            if (possibleAlterations[i] != null && possibleAlterations[i].gameObject != selectionObj)
            {
                Destroy(possibleAlterations[i].gameObject);
            }
        }

    }
}
