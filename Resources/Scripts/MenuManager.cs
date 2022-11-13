using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public TMP_InputField seedInput;
    public GameObject seedWarning;
    public TextMeshProUGUI countdownText;
    public GameObject seedwarningCover;
    public int countdown = 5;
    private bool open;

    public List<string> secretsList = new List<string>();
    public int totalSecrets;
    public TextMeshProUGUI totalSecretsText;

    public GameObject mainMenu;
    public GameObject secretMenu;

    public void OpenSecretMenu()
    {
        secretMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        secretMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void Start()
    {
        int gotten = 0;
        foreach (string sec in secretsList)
        {
            if (PlayerPrefs.GetInt(sec) == 1)
            {
                gotten += 1;
            }
        }

        totalSecretsText.text = (totalSecrets - gotten).ToString();

        StartCoroutine(Countdown());
    }

    public void Play()
    {
        PlayerPrefs.SetInt("seed", 16);
        SceneManager.LoadScene(1);
    }

    public void EnableSeedWarning()
    {
        seedWarning.SetActive(true);
        open = true;
    }

    public void DisableSeedWarning()
    {
        seedWarning.SetActive(false);
        if (countdown > 0)
        {
            countdown = 5;
        }
        open = false;
    }

    IEnumerator Countdown()
    {
        if (open)
        {
            yield return new WaitForSeconds(1);
            if (open)
            {
                countdown -= 1;
            }

            countdownText.text = "Please Read Above... (" + countdown.ToString() + ")";

            if (countdown < 0)
            {
                seedwarningCover.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Countdown());
    }

    public void Seed()
    {
        if (seedInput.text == "")
        {
            PlayerPrefs.SetInt("seed", UnityEngine.Random.Range(0, 100000));
        }
        else
        {
            PlayerPrefs.SetInt("seed", (int)Int64.Parse(seedInput.text));
        }

        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
