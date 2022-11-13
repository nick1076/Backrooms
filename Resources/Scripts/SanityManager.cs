using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

public class SanityManager : MonoBehaviour
{
    public bool sanityDeclineOverTime = true;

    public Image sanityFrame;
    public Image[] sanityBars;
    public UnityEngine.Rendering.VolumeProfile[] sanityTiers;
    public UnityEngine.Rendering.Volume volume;

    public AudioSource strangeSounds;

    public float fadeTime = 2.0f;

    private int cyclesWaited;

    private float currentSanity = 50.0f;

    Coroutine FadeEvent;
    Coroutine FadeEventOut;

    private void Start()
    {
        if (sanityDeclineOverTime)
        {
            StartCoroutine(sanityCountdown());
        }
        AlterSanity(50);
    }

    IEnumerator sanityCountdown()
    {
        yield return new WaitForSeconds(120);
        AlterSanity(-10);
        StartCoroutine(sanityCountdown());
    }

    public void AlterSanity(float amount)
    {
        currentSanity += amount;

        if (currentSanity > 50)
        {
            currentSanity = 50;
        }
        else if (currentSanity < 0)
        {
            currentSanity = 0;
        }

        sanityBars[0].gameObject.SetActive(false);
        sanityBars[1].gameObject.SetActive(false);
        sanityBars[2].gameObject.SetActive(false);
        sanityBars[3].gameObject.SetActive(false);
        sanityBars[4].gameObject.SetActive(false);

        if (currentSanity >= 10)
        {
            sanityBars[0].gameObject.SetActive(true);
        }

        if (currentSanity >= 20)
        {
            sanityBars[1].gameObject.SetActive(true);
        }

        if (currentSanity >= 30)
        {
            sanityBars[2].gameObject.SetActive(true);
        }

        if (currentSanity >= 40)
        {
            sanityBars[3].gameObject.SetActive(true);
        }

        if (currentSanity >= 50)
        {
            sanityBars[4].gameObject.SetActive(true);
        }

        if (currentSanity <= 50 && currentSanity > 40)
        {
            volume.profile = sanityTiers[5];
            strangeSounds.volume = 0;
        }
        else if (currentSanity <= 40 && currentSanity > 30)
        {
            volume.profile = sanityTiers[4];
            strangeSounds.volume = 0;
        }
        else if (currentSanity <= 30 && currentSanity > 20)
        {
            volume.profile = sanityTiers[3];
            strangeSounds.volume = .1f;
        }
        else if (currentSanity <= 20 && currentSanity > 10)
        {
            volume.profile = sanityTiers[2];
            strangeSounds.volume = .25f;
        }
        else if (currentSanity <= 10 && currentSanity > 0)
        {
            volume.profile = sanityTiers[1];
            strangeSounds.volume = .5f;
        }
        else if (currentSanity == 0)
        {
            volume.profile = sanityTiers[0];
            strangeSounds.volume = 1;
        }
    }
}
