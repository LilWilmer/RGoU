using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public AudioSource[] AudioClips;
    public bool FadingIn { get; set; }
    public bool FadingOut { get; set; }
    public float[] StartVolume { get; set; }

    private void Start()
    {
        StartVolume = new float[AudioClips.Length];
        int ii = 0;
        foreach (AudioSource a in AudioClips)
        {
            StartVolume[ii] = a.volume;
            ii++;
        }
    }
    public void PlayAll()
    {
        foreach(AudioSource audioClip in AudioClips)
        {
            audioClip.Play();
        }
    }
    
    public void PlayClip(int index)
    {
        AudioClips[index].Play();
    }

    public void StopClip(int index)
    {
        AudioClips[index].Stop();
    }

    //COROUTINES==============================================================
    public IEnumerator FadeInCoroutine(int track, float speed, float targetVolume)
    {
        FadingOut = false;
        FadingIn = true;
        PlayClip(track);
        while (AudioClips[track].volume <= targetVolume && !FadingOut)
        {
            AudioClips[track].volume += speed;
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Fading In");

        }
        Debug.Log("FADE IN DONE");
        FadingIn = false;
    }

    public IEnumerator FadeOutCoroutine(int track, float speed, float targetVolume)
    {
        FadingIn = false;
        FadingOut = true;
        while (AudioClips[track].volume >= targetVolume+0.001 && !FadingIn)
        {
            AudioClips[track].volume -= speed;
            yield return new WaitForSeconds(0.1f);
        }
        StopClip(track);
        FadingOut = false;

    }

    public void FadeOut(int track, float speed, float targetVolume)
    {
        StartCoroutine(FadeOutCoroutine(track, speed, targetVolume));
    }

    public void FadeIn(int track, float speed, float targetVolume)
    {
        StartCoroutine(FadeInCoroutine(track, speed, targetVolume));
    }

    public void FadeOut1(int track)
    {
        FadeOut(track, 0.01f, 0);
    }

    public void FadeIn1(int track)
    {
        FadeIn(track, 0.01f, StartVolume[track]);
    }
}
