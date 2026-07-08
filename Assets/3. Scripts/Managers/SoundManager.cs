using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager playsound;

    public AudioSource bgm;
    public AudioSource sound;
    public AudioClip soundClip;
    public AudioClip nextqner;
    public AudioClip correct;
    public AudioClip wrong;

    private void Awake()
    {
        if(playsound == null)
        {
            playsound = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        bgm = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BGMPlay(AudioClip audio)
    {
        bgm.clip = audio;
        bgm.Play();
    }

    public void Volumed(AudioSource audioSource, float volume)
    {
        audioSource.volume = volume;
    }

    public void Looped(AudioSource audioSource, bool looped)
    {
        audioSource.loop = looped;
    }

    public void SoundEffectPlay(AudioClip audio)
    {
        sound.PlayOneShot(audio);
    }

    public void ShutUp()
    {
        sound.Stop();
        bgm.Stop();
    }
}
