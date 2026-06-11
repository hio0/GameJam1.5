using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(UIEvent.Event.OnAnswerClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
