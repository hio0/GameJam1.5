using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Book : MonoBehaviour
{
    public static Book book;

    public GameObject[] pages;
    int nowpage;
    int page;

    public RectTransform pageA;
    public Transform A_content;
    public TMP_Text A_PT;

    public RectTransform pageB;
    public Transform B_content;
    public TMP_Text B_PT;

    public RectTransform pageC;
    public Transform C_content;
    public TMP_Text C_PT;

    public float moveAmount = 300f;
    public float duration = 0.5f;

    bool isFlipping = false;

    public TMP_Text pre_pageT;
    public GameObject pre_mokchaA;
    public GameObject pre_mokchaB;

    private void Awake()
    {
        if (book == null)
        {
            book = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetMokcha();
    }

    void Update()
    {
        if (!isFlipping)
        {
            if (Input.GetKeyDown(KeyCode.D) && (nowpage + 1) * 2 + 1 < pages.Length)
            {
                StartCoroutine(FlipPage(true));
            }
            else if (Input.GetKeyDown(KeyCode.A) && nowpage > 0)
            {
                StartCoroutine(FlipPage(false));
            }
        }
    }

    void SetP(TMP_Text text, int num)
    {
        string part = null;
        if (nowpage < 2)
        {
            part = "목차";
        }
        else if (nowpage < 6)
        {
            part = "이끼";
        }
        else if (nowpage < 12)
        {
            part = "림버스";
        }

        text.text = $"P.{num + 1} | {part}";
    }

    void ClearPage()
    {
        if (A_content.childCount > 0)
        {
            Destroy(A_content.GetChild(0).gameObject);
        }

        if (B_content.childCount > 0)
        {
            Destroy(B_content.GetChild(0).gameObject);
        }

        if (C_content.childCount > 0)
        {
            Destroy(C_content.GetChild(0).gameObject);
        }
    }

    void SetPage()
    {
        TMP_Text pt = null;
        Transform con = null;

        int pageindex = 0;

        pageindex = nowpage * 2;
        pt = B_PT;
        con = B_content;
        Instantiate(pages[pageindex], con);
        SetP(pt, pageindex);

        pageindex = nowpage * 2 + 1;
        pt = C_PT;
        con = C_content;
        Instantiate(pages[pageindex], con);
        SetP(pt, pageindex);
    }

    public void SetMokcha()
    {
        ClearPage();

        nowpage = 0;

        SetPage();
    }

    public void WarpToPageNum(int page)
    {
        bool ifsf = false;
        if (page >= nowpage)
        {
            ifsf = true;
        }

        nowpage = page;
        FlipPage(ifsf);
    }

    IEnumerator FlipPage(bool forward)
    {
        isFlipping = true;

        float time = 0f;

        float start = 0f;
        float end = 1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Lerp(start, end, time / duration);

            ApplyFlip(t, forward); // ⭐ 방향 전달

            yield return null;
        }

        Vector2 vec = Vector2.zero;

        ClearPage();
        if (forward)
        {
            nowpage++;
        }
        else
        {
            nowpage--;
        }
        SetPage();
        pageA.anchoredPosition = new Vector2(320f, -72.5f);

        ApplyFlip(end, true);
        isFlipping = false;
    }

    void ApplyFlip(float t, bool forward)
    {

        t = Mathf.SmoothStep(0, 1, t);

        // 1️⃣ 회전
        float angle = forward
            ? Mathf.Lerp(0, -180f, t)
            : Mathf.Lerp(-180f, 0, t);

        pageA.localRotation = Quaternion.Euler(0, angle, 0);

        // 2️⃣ 위치 이동 (핵심)
        float x = forward
            ? Mathf.Lerp(320f, -320f, t)   // D
            : Mathf.Lerp(-320f, 320f, t);  // A

        pageA.anchoredPosition = new Vector2(x, -72.5f);

        // 3️⃣ 뒤집기
        if (forward) // 앞으로 넘김.
        {
            if (t > 0.5f)
                pageA.localScale = new Vector3(-1, 1, 1);
            else
                pageA.localScale = new Vector3(1, 1, 1);
            /*
            A_Text.text = B_Text.text;
            B_Text.text = C_Text.text;
        */
        }
        else
        {
            if (t > 0.5f)
                pageA.localScale = new Vector3(1, 1, 1);
            else
                pageA.localScale = new Vector3(-1, 1, 1);
        }
    }
}
