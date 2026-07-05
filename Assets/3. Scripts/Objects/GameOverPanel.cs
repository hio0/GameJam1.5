using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public TMP_Text gameovercountT;
    public TMP_Text gameoverstarT;

    public GameObject exitB;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(STartTed());
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

    IEnumerator STartTed()
    {
        exitB.SetActive(false);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1165.1f, 0);

        StartCoroutine(MovingAnimation(gameObject.GetComponent<RectTransform>(), Vector2.zero, 3f));

        gameovercountT.text = "문제 개수: " + GameManager.gm.Qcount.ToString();
        gameoverstarT.text = "모은 별: " + GameManager.gm.mystar.ToString();

        yield return new WaitForSeconds(1.5f);
        exitB.SetActive(true);
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
