using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public TMP_Text text;
    public Action onclick;
    public AudioClip huoguaum;
   
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        onclick?.Invoke();
    }

    public void AudioPlay()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.clip = huoguaum;
        audioSource.Play();
    }
}
