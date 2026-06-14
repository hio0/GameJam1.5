using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Book : MonoBehaviour
{
    public Page[] pages;
    int nowpage;

    public RectTransform pageA;
    public Transform A_content;
    public TMP_Text A_Text;
    public TMP_Text A_PT;

    public RectTransform pageB;
    public Transform B_content;
    public TMP_Text B_Text;
    public TMP_Text B_PT;

    public GameObject pageC;
    public TMP_Text C_Text;
    public TMP_Text C_PT;

    public float moveAmount = 300f;
    public float duration = 0.5f;

    bool isFlipping = false;

    public GameObject pre_mokchaA;
    public GameObject pre_mokchaB;

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

    void SetP(TMP_Text text, string much, string part)
    {
        text.text = $"P.{much}|{part}";
    }

    void SetMokcha()
    {
        A_Text.text = null;
        B_Text.text = null;
        C_Text.text = null;

        //A_PT.text = 

        //Instantiate(pre_mokchaA, A_content);
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

        ApplyFlip(end, forward);
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
