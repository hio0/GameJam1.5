using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameObject qner;
    int Qcount;
    public TMP_Text QcountT;
    public Question[] questions;
    public Question nowQ;
    public TMP_Text qT;

    public GameObject anser;
    public GameObject pre_AnswerB;

    bool isquestionSet;
    int days;

    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEvent.Event.OnAnswerClicked += Answerd;

        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isquestionSet)
        {
            SetQuestion();
            isquestionSet = true;
        }
    }

    public void ResetGame()
    {
        days = 0;
        Qcount = 0;

        qner.SetActive(false);
        isquestionSet = false;
    }

    public void NextDay()
    {
        days++;
    }

    public void SetQuestion()
    {
        qner.SetActive(true);
        Qcount++;
        QcountT.text = Qcount.ToString();

        int r = Random.Range(0, questions.Length);
        nowQ = questions[r];

        qT.text = nowQ.Qtext;
    }

    public void Answerd()
    {

    }
}
