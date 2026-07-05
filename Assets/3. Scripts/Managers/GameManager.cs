using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameObject qner;
    public TMP_Text qnerName;
    public Image qnerImage;
    Qner nowQner;
    bool qnerIsTekbeul;
    public int Qcount;
    public TMP_Text QcountT;
    public Question[] questions;
    public Question nowQ;
    public TMP_Text qT;
    public Action qset;
    public Qner[] dayqner;
    public string[] randomName;
    public Transform stars;

    public int mystar;
    public TMP_Text starT;
    public TMP_Text plusstarT;

    public TMP_Text timerT;
    [SerializeField] float timer;
    float time;

    public RectTransform anser;
    public RectTransform answers;
    public Transform answerTransform;
    public GameObject pre_AnswerB;

    public Transform hpTransform;
    int hp;
    public GameObject gameoverP;

    bool isquestionSet;
    bool isgameover;

    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneMovedObject.instance.Moved(true);
        ResetGame();

        SoundManager.playsound.Looped(SoundManager.playsound.bgm, true);
        SoundManager.playsound.BGMPlay(SoundManager.playsound.soundClip);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isquestionSet && !isgameover)
        {
            time = UnityEngine.Random.Range(30, 51);

            NewQuestion();
            isquestionSet = true;
        }
    }

    public void ResetGame()
    {
        Qcount = 0;
        hp = 3;
        mystar = 0;

        starT.text = $"x {mystar.ToString()}";
        plusstarT.gameObject.SetActive(false);

        qner.SetActive(false);
        isquestionSet = false;
        gameoverP.SetActive(false);
        isgameover = false;
    }

    IEnumerator TimerOn()
    {
        bool isok = false;
        timer = time;
        yield return null;

        timerT.gameObject.SetActive(false);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerT.text = $"{timer.ToString("F0")}...";

            if (timer < time / 1.5f && !isok)
            {
                StartCoroutine(FadeIn(timerT.GetComponent<CanvasGroup>(), 1.5f));

                isok = true;
            }

            yield return null;
        }

        if (timer <= 0)
        {
            StartCoroutine(Answerd(false));
        }
    }

    public void NewQuestion()
    {
        StopAllCoroutines();

        qner.SetActive(true);
        qner.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -850);
        StartCoroutine(MovingAnimation(qner.GetComponent<RectTransform>(), new Vector2(0, 0), 3));

        Qcount++;

        string a = null;
        switch (Qcount)
        {
            case 1:
                a = "st";
                break;
            case 2:
                a = "nd";
                break;
            case 3:
                a = "rd";
                break;
            default:
                a = "th";
                break;
        }

        QcountT.text = Qcount.ToString() + $"<size=40>{a}</size>";

        qset = null;
        SetQner();
        qset?.Invoke();
        SoundManager.playsound.SoundEffectPlay(SoundManager.playsound.nextqner);
    }

    void SetQner()
    {
        nowQner = null;
        nowQ = null;
        int r = UnityEngine.Random.Range(0, dayqner.Length);

        nowQner = dayqner[r];
        qnerImage.sprite = nowQner.qnerImage;

        if (nowQner.myQuestion.Length == 0) // 이 자식은 평범한 자식입니다
        {
            qnerIsTekbeul = false;
            int ran = UnityEngine.Random.Range(0, randomName.Length);
            qnerName.text = randomName[ran];
        }
        else // 임마는 좀 특별캄 ㅇㅇ
        {
            qnerIsTekbeul = true;
            qnerName.text = nowQner.qnerName;
            int ran = UnityEngine.Random.Range(0, nowQner.myQuestion.Length);

            nowQ = nowQner.myQuestion[ran];
        }

        qset += SetQ;
    }

    void SetQ()
    {
        bool iscorrectset = false;
        List<string> list = new List<string>();
        list.Clear();
        for (int i = 0; i < stars.childCount; i++)
        {
            Color32 col = new Color32(104, 91, 71, 255);
            stars.GetChild(i).GetComponent<Image>().color = col;
        }

        // Question 세팅
        if (!qnerIsTekbeul)
        {
            int r = UnityEngine.Random.Range(0, questions.Length);
            nowQ = questions[r];
        }

        int star = nowQ.star;
        for (int i = 0; i < star; i++)
        {
            Color32 col = new Color32(159, 147, 130, 255);
            stars.GetChild(i).GetComponent<Image>().color = col;
        }
        StartCoroutine(TypeText(qT, nowQ.Qtext));
        StartCoroutine(TimerOn());

        // Answer 버튼 세팅
        StartCoroutine(AnswerSet());

        for (int i = 0; i < 3; i++)
        {
            string ans = null;
            GameObject b = Instantiate(pre_AnswerB, answerTransform);

            bool isimi = true;
            int selectqnum = 0;
            int isthatcorrect = 0;
            while (isimi)
            {
                selectqnum = UnityEngine.Random.Range(0, nowQ.wrongAnswer.Length);
                isthatcorrect = UnityEngine.Random.Range(1, 101);

                if (list.Count != 0 && list.Contains(nowQ.wrongAnswer[selectqnum]))
                {
                    continue;
                }
                else
                {
                    isimi = false;
                    break;
                }
            }

            if (!isimi)
            {
                ans = nowQ.wrongAnswer[selectqnum];
                Action act = null;

                act = () => StartCoroutine(Answerd(false));
                if (!iscorrectset)
                {
                    if (isthatcorrect <= 34 || i == 2)
                    {
                        iscorrectset = true;
                        ans = nowQ.answer;
                        act = () => StartCoroutine(Answerd(false));
                    }
                }

                b.GetComponent<AnswerButton>().text.text = ans;
                b.GetComponent<AnswerButton>().onclick = act;
                list.Add(ans);
            }
        }
    }

    IEnumerator AnswerSet()
    {
        anser.localPosition = new Vector2(2250, 171);
        answers.localPosition = new Vector2(1325f, 393);
        yield return new WaitForSeconds(2f);

        int r = UnityEngine.Random.Range(-54, -115);
        anser.localRotation = Quaternion.Euler(0, 0, r);

        Vector2 target = new Vector2(722, 171);
        StartCoroutine(MovingAnimation(anser, target, 3));

        yield return new WaitForSeconds(1f);

        StartCoroutine(MovingAnimation(answers, new Vector2(626f, answers.localPosition.y), 3));
    }

    IEnumerator MovingAnimation(RectTransform what, Vector2 target, float speed)
    {
        while (what.anchoredPosition != target)
        {
            float x = Mathf.Lerp(what.anchoredPosition.x, target.x, Time.deltaTime * speed);
            float y = Mathf.Lerp(what.anchoredPosition.y, target.y, Time.deltaTime * speed);

            what.anchoredPosition = new Vector2(x, y);
            yield return null;
        }
    }

    IEnumerator FadeIn(CanvasGroup what, float fadeTime)
    {
        float time = 0f;
        what.gameObject.SetActive(true);
        what.alpha = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            what.alpha = Mathf.Lerp(0f, 1f, time / fadeTime);
            yield return null;
        }

        what.alpha = 1f;

    }

    IEnumerator TypeText(TMP_Text targetT, string text)
    {
        targetT.text = "";

        foreach (char letter in text.ToCharArray())
        {
            targetT.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator Answerd(bool iscorrected)
    {
        StopAllCoroutines();
        StartCoroutine(MovingAnimation(anser, new Vector2(2250, 171), 3));
        StartCoroutine(MovingAnimation(answers, new Vector2(1325f, 393), 3));
        int r = 0;

        if (iscorrected)
        {
            r = UnityEngine.Random.Range(0, nowQner.correctReact.Length);
            StartCoroutine(TypeText(qT, nowQner.correctReact[r]));

            StarChanged(nowQ.star);
            SoundManager.playsound.SoundEffectPlay(SoundManager.playsound.correct);
        }
        else
        {
            r = UnityEngine.Random.Range(0, nowQner.wrondReact.Length);
            StartCoroutine(TypeText(qT, nowQner.wrondReact[r]));

            StartCoroutine(HpDown());
            SoundManager.playsound.SoundEffectPlay(SoundManager.playsound.wrong);
        }

        for (int i = 0; i < answerTransform.childCount; i++)
        {
            Destroy(answerTransform.GetChild(i).gameObject);
        }

        yield return StartCoroutine(QEnd());

        if(hp <= 0)
        {
            GameOver();
        }
    }

    IEnumerator HpDown()
    {
        hp--;

        GameObject targetobj = hpTransform.GetChild(2 - hp).Find("Image").gameObject;
        targetobj.SetActive(true);
        RectTransform target = targetobj.GetComponent<RectTransform>();
        target.sizeDelta = new Vector2(10, 0);

        while (target.sizeDelta.y < 100)
        {
            target.sizeDelta = new Vector2(10, target.sizeDelta.y + 120 * Time.deltaTime);
            yield return null;
        }

        target.sizeDelta = new Vector2(10, 100);
    }

    IEnumerator QEnd()
    {
        timerT.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);

        StartCoroutine(MovingAnimation(qner.GetComponent<RectTransform>(), new Vector2(0, -850), 3));

        float r = UnityEngine.Random.Range(3f, 5f);
        yield return new WaitForSeconds(r);

        qner.SetActive(false);
        isquestionSet = false;
    }

    void StarChanged(int changed)
    {
        mystar += changed;
        StartCoroutine(StarChangeAnimation(changed));

        starT.text = $"<size=30>x</size> {mystar.ToString()}";
    }

    IEnumerator StarChangeAnimation(int changed)
    {
        plusstarT.gameObject.SetActive(true);

        if(changed >= 0)
        {
            plusstarT.text = $"+{changed}";
        }
        else
        {
            plusstarT.text = $"{changed}";
        }

        CanvasGroup can = plusstarT.gameObject.GetComponent<CanvasGroup>();
        RectTransform rec = plusstarT.gameObject.GetComponent<RectTransform>();

        float time = 0f;
        float fadeout = 1.5f;

        can.alpha = 1f;

        rec.anchoredPosition = new Vector2(114.8f, rec.anchoredPosition.y);
        StartCoroutine(MovingAnimation(rec, new Vector2(170.88f, 0f), 1.5f));

        while (time < fadeout)
        {
            time += Time.deltaTime;
            can.alpha = Mathf.Lerp(1f, 0f, time / fadeout);

            yield return null;
        }

        can.alpha = 0f;
        plusstarT.gameObject.SetActive(false);
    }

    void GameOver()
    {
        isgameover= true;

        SoundManager.playsound.BGMPlay(null);
        SoundManager.playsound.SoundEffectPlay(null);

        gameoverP.SetActive(true);
    }
}
