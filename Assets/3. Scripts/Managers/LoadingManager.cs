using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public string nextSceneName;

    void Start()
    {
        SoundManager.playsound.ShutUp();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float progress = 0f;

        while (!op.isDone)
        {
            // 실제 로딩 진행도 (0~0.9)
            float target = Mathf.Clamp01(op.progress / 0.9f);

            // 부드럽게 보간 (슬라이딩 느낌 추가)
            progress = Mathf.Lerp(progress, target, Time.deltaTime * 5f);

            // 로딩 완료
            if (progress >= 0.99f)
            {
                yield return new WaitForSeconds(0.3f); // 연출용 딜레이
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
