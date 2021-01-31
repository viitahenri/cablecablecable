using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private CanvasGroup _text;

    void Awake()
    {
        _background.alpha = 0f;
        _text.alpha = 0f;
    }

    public void StartEnd()
    {
        StartCoroutine(StartEndRoutine());
    }

    public IEnumerator StartEndRoutine()
    {
        yield return new WaitForEndOfFrame();
        float timer = 0f;
        float fadeTime = 5f;
        float endShowTime = 3f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            _background.alpha = Mathf.Lerp(0f, 1f, timer / fadeTime);
            yield return new WaitForEndOfFrame();
        }

        // cable model sound

        timer = 0f;
        while (timer <= endShowTime)
        {
            timer += Time.deltaTime;
            _text.alpha = Mathf.Lerp(0f, 1f, timer / endShowTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
