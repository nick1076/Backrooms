using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public GameObject debugUI;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        debugUI.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (debugUI.activeInHierarchy)
            {
                debugUI.SetActive(false);
            }
            else
            {
                debugUI.SetActive(true);
            }
        }

        if (debugUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                LoadScene(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                LoadScene(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                LoadScene(2);
            }
        }
    }

    public void LoadScene(int scene)
    {
        debugUI.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);

        if (scene == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            //Used to disable debug for testing on certain levels
            //Destroy(this.gameObject);
        }
    }
}
