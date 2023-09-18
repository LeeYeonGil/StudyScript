using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressbar;
    [SerializeField] TMP_Text tip;
    [SerializeField] TMP_Text per;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Load");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // 비동기 방식으로 일시 중지가 발생X  LoadSceneAsync() 함수가 AsyncOperation 클래스 형식으로 반환
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            per.text = op.progress.ToString("00.0%");
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, op.progress, timer);
                if (progressbar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, 1f, timer);
                if (progressbar.fillAmount == 1.0f)
                {
                per.text = "100.0%";
                yield return new WaitForSeconds(1.0f);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
