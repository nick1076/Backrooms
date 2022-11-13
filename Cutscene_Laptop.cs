using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Laptop : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject clickUI;
    public GameObject realPlayer;
    public GameObject fakePlayer;

    public void Clicked()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        mainUI.SetActive(false);
        clickUI.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        fakePlayer.SetActive(false);
        realPlayer.SetActive(true);
    }
}
