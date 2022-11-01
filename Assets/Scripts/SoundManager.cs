using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static void PlaySound(AudioClip clip)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
