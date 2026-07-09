using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public TMP_Text gameovercountT;
    public TMP_Text gameoverstarT;

    private void OnEnable()
    {
        STartTed();
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

    void STartTed()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1165.1f);

        StartCoroutine(MovingAnimation(gameObject.GetComponent<RectTransform>(), new Vector2(0,0), 3f));

        gameovercountT.text = "문제 개수: " + GameManager.gm.Qcount.ToString();
        gameoverstarT.text = "모은 별: " + GameManager.gm.mystar.ToString();
    }

    public void Exited()
    {
        StartCoroutine(SceneMovedObject.instance.MovedAndLoadScene(false, "Main"));
    }

    public void Retry()
    {
        StartCoroutine(SceneMovedObject.instance.MovedAndLoadScene(false, "Loading"));
    }
}
