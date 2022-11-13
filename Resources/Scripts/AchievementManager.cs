using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public string achievementName;
    public string achievementDescription;
    public string achievementSerializedName = "Data.Achievement.LetsParty";
    public Sprite achievementIcon;

    [Space]

    public TMPro.TextMeshProUGUI achievementInfoPanel_nameText;
    public TMPro.TextMeshProUGUI achievementInfoPanel_DescriptionText;
    public UnityEngine.UI.Image achievementInfoPanel_icon;

    public GameObject[] lockedObj;

    bool locked;

    public void Start()
    {
        if (PlayerPrefs.GetInt(achievementSerializedName) == 1)
        {
            locked = false;
        }
        else
        {
            locked = true;
        }

        if (locked)
        {
            foreach (GameObject lockedObject in lockedObj)
            {
                lockedObject.SetActive(true);
            }
        }
    }

    public void OnExpanded()
    {
        achievementInfoPanel_nameText.text = achievementName;
        achievementInfoPanel_DescriptionText.text = achievementDescription;
        achievementInfoPanel_icon.sprite = achievementIcon;

        if (locked)
        {
            foreach(GameObject lockedObject in lockedObj)
            {
                lockedObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject lockedObject in lockedObj)
            {
                lockedObject.SetActive(false);
            }
        }
    }

}
