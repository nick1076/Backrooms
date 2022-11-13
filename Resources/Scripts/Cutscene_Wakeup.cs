using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Wakeup : MonoBehaviour
{

    public GameObject player;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(Wake());
    }

    IEnumerator Wake()
    {
        yield return new WaitForSeconds(15);
        player.SetActive(true);
        Destroy(this.GetComponent<Camera>());
        Destroy(this.GetComponent<AudioListener>());
        this.GetComponent<AudioListener>().enabled = false;
    }

}
