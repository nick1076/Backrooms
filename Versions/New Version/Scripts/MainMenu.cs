using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator anim;
    public string confrenceLevelName;
    public float timeForAnimation = 1.25f;
    public void MenuOption(string type)
    {
        StartCoroutine(Proceed(type));
    }

    IEnumerator Proceed(string type)
    {
        anim.SetTrigger("Go");
        yield return new WaitForSeconds(timeForAnimation);
        if (type == "play")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(confrenceLevelName);
        }
        else if (type == "quit")
        {
            Application.Quit();
        }
    }
}
