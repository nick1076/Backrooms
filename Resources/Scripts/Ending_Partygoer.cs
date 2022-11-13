using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending_Partygoer : MonoBehaviour
{
    public GameObject musicObj;
    public GameObject noClippedWall;
    public GameObject trigger;

    public void Initiate()
    {
        PlayerPrefs.SetInt("Data.Achievement.LetsParty", 1);
        musicObj.SetActive(true);
        noClippedWall.GetComponent<BoxCollider>().isTrigger = false;
        Destroy(trigger);
        StartCoroutine(sanityDecay());
    }

    IEnumerator sanityDecay()
    {
        SanityManager playerSanity = GameObject.FindWithTag("Entity.Player").GetComponent<SanityManager>();
        playerSanity.AlterSanity(50);

        for (int i = 0; i < 36; i++)
        {
            yield return new WaitForSeconds(1);
            playerSanity.AlterSanity(-1.38f);
        }

        playerSanity.AlterSanity(-100);
    }
}
