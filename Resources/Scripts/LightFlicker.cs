using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public GameObject lights;
    public Material unLit;
    public Material lit;
    public GameObject toMaterial;
    public int flickerInterval;

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        for (int i = 0; i < flickerInterval; i++)
        {
            int rand = Random.Range(0, flickerInterval + 1);
            
            if (rand == flickerInterval || i == flickerInterval - 1)
            {
                lights.SetActive(false);
                toMaterial.GetComponent<Renderer>().material = unLit;
                yield return new WaitForSeconds(Random.Range(0.25f, 2f));
                lights.SetActive(true);
                toMaterial.GetComponent<Renderer>().material = lit;
                i = 0;
            }

            yield return new WaitForSeconds(1);
        }
    }


}
