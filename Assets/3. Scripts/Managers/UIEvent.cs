using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvent : MonoBehaviour
{
    public static UIEvent Event;

    public event Action OnAnswerClicked;

    private void Awake()
    {
        if(Event == null)
        {
            Event = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnswerClick()
    {
        OnAnswerClicked?.Invoke();
    }
}
