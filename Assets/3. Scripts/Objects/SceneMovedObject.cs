using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMovedObject : MonoBehaviour
{
    public static SceneMovedObject instance;
    CanvasGroup can;
    public AudioClip huoguaum;

    private void Awake()
    {
        instance = this;
        can = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        can.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public IEnumerator MovedAndLoadScene(bool isopen, string sceneName)
    {
        Moved(isopen);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    public void Moved(bool isopen)
    {
        RectTransform rec = GetComponent<RectTransform>();
        float closedpos = 673;
        float opendpos = -3200f;
        float target = 0f;
        float speed = 7f;

        if(isopen)
        {
            target = opendpos;
            rec.anchoredPosition = new Vector2(closedpos, rec.anchoredPosition.y);
        }
        else
        {
            target = closedpos;
            rec.anchoredPosition = new Vector2(4400, rec.anchoredPosition.y);
        }
        can.alpha = 1f;
        StartCoroutine(MovingAnimation(rec, new Vector2(target, rec.anchoredPosition.y), speed));
        SoundManager.playsound.SoundEffectPlay(huoguaum);
    }
}
