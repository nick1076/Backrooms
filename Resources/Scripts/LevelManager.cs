using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int levelID;
    public GameObject player;
    public GameObject smilerScare;
    public List<GameObject> levelCheckpoints = new List<GameObject>();

    public void Start()
    {
        player.transform.position = levelCheckpoints[PlayerPrefs.GetInt("LastCheckpoint")].transform.position;
        player.transform.rotation = levelCheckpoints[PlayerPrefs.GetInt("LastCheckpoint")].transform.rotation;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LastCheckpoint", 0);
    }

    public void SetCheckpoint(GameObject point)
    {
        for (int i = 0; i < levelCheckpoints.Count; i++)
        {
            if (point == levelCheckpoints[i])
            {
                PlayerPrefs.SetInt("LastCheckpoint", i);
                return;
            }
        }
    }

    public void CallJumpscare(string entity)
    {
        if (entity == "Entity.Smiler")
        {
            player.SetActive(false);
            Instantiate(smilerScare, new Vector3(0, 1000, 0), Quaternion.identity);
        }

        Invoke("ReloadLevel", 5);
    }

    public void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelID);
    }
}
