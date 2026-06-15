using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public GameObject pageC;
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

    private void OnEnable()
    {
        SetMokcha();
        nowpage = 0;
    }

    void Update()
    {
        if (!isFlipping)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(FlipPage(true));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(FlipPage(false));
            }
        }
    }

    void SetP(TMP_Text text, string much)
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

        text.text = $"P.{much} | {part}";
    }

    void ClearPage()
    {
        if (A_content.childCount > 0 || B_content.childCount > 0 || C_content.childCount > 0)
        {
            Destroy(A_content.GetChild(0).gameObject);
            Destroy(B_content.GetChild(0).gameObject);
            Destroy(C_content.GetChild(0).gameObject);
        }

        /*
        for(int i  = 0; i < A_content.childCount; i++)
        {
            Destroy(A_content.GetChild(i));
        }

        for (int i = 0; i < B_content.childCount; i++)
        {
            Destroy(B_content.GetChild(i));
        }

        for (int i = 0; i < C_content.childCount; i++)
        {
            Destroy(C_content.GetChild(i));
        }
        */
    }

    public void SetMokcha()
    {
        ClearPage();

        nowpage = 0;
        Instantiate(pages[nowpage], A_content);
        Instantiate(pages[nowpage + 1], A_content);

        SetP(A_PT, 1.ToString());
        SetP(B_PT, 2.ToString());
    }

    public void WarpToPageNum(int page)
    {
        nowpage = page - 1;
        FlipPage(true);
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
        if (forward)
        {
            vec = new Vector2(320f, -72.5f);

            if (nowpage >= pages.Length)
            {
                StopAllCoroutines();
            }
            else
            {
                ClearPage();

                Instantiate(pages[nowpage], B_content);
                int b = nowpage + 1;
                SetP(B_PT, b.ToString());

                Instantiate(pages[nowpage + 1], C_content);
                int c = nowpage + 2;
                SetP(C_PT, c.ToString());
                nowpage++;

                /*
                string b_part = null;
                string c_part = null;

                int b = 0;
                int c = 0;


                
                A_Text.text = B_Text.text;

                if (nowpage == 0)
                {
                    Destroy(A_content.transform.GetChild(0).gameObject);

                    B_Text.text = pages[nowpage].text;
                    C_Text.text = pages[nowpage + 1].text;

                    b_part = pages[nowpage].part;
                    c_part = pages[nowpage + 1].part;

                    b = nowpage + 3;
                    c = nowpage + 4;
                }
                else
                {
                    if (nowpage + 1 >= pages.Length)
                    {
                        
                    }
                    else
                    {
                        B_Text.text = pages[nowpage + 1].text;
                        b_part = pages[nowpage + 1].part;
                        b = nowpage + 4;
                    }
                    if (nowpage + 2 >= pages.Length)
                    {
                        pageC.SetActive(false);
                    }
                    else
                    {
                        pageC.SetActive(true);
                        C_Text.text = pages[nowpage + 2].text;
                        c_part = pages[nowpage + 2].part;
                        c = nowpage + 5;
                    }
                }

                SetP(B_PT, b.ToString(), b_part);
                SetP(C_PT, c.ToString(), c_part);

                nowpage++;
            }
        }
        else
        {
            vec = new Vector2(-320f, -72.5f);

            pageC.SetActive(true);
            nowpage--;

            C_Text.text = B_Text.text;

            B_Text.text = pages[nowpage].text;
            if (nowpage == 0)
            {
                SetMokcha();
            }
        }
                */
            }
            pageA.anchoredPosition = vec;

            ApplyFlip(end, forward);
            isFlipping = false;
        }
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
