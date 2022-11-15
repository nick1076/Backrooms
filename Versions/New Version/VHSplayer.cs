using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSplayer : MonoBehaviour
{

    public Animator anim;

    public void UsePlayer()
    {
        if (GameObject.Find("player") != null)
        {
            if (GameObject.Find("player").GetComponent<Inventory>() != null)
            {
                Item cItem = GameObject.Find("player").GetComponent<Inventory>().RetrieveCurrentlySelected();
                print("h");
                if (cItem != null)
                {
                    string toLoad = "";
                    if (cItem.data == "vhs_1")
                    {
                        toLoad = "level";
                    }
                    if (toLoad != "")
                    {
                        anim.SetTrigger("Go");
                        StartCoroutine(waitLoad(toLoad));
                    }
                }
            }
        }
    }

    IEnumerator waitLoad(string lvl)
    {
        yield return new WaitForSeconds(.45f);
        Destroy(GameObject.Find("player"));
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene(lvl);
    }
}
