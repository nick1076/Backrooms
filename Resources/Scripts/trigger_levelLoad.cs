using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class trigger_levelLoad : MonoBehaviour
{

    public int levelIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entity.Player")
        {
            SceneManager.LoadScene(levelIndex);
        }
    }

}
