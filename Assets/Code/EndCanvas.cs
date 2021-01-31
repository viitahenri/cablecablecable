using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private CanvasGroup _text;
    [SerializeField] private List<AudioClip> _endDialUps = new List<AudioClip>();

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        float fadeTime = 6f;
        float endShowTime = 3f;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            _background.alpha = Mathf.Lerp(0f, 1f, timer / fadeTime);
            yield return new WaitForEndOfFrame();
        }

        timer = 0f;

        _audioSource.PlayOneShot(_endDialUps[0]);
        yield return new WaitForSeconds(.5f);
        _audioSource.PlayOneShot(_endDialUps[1]);
        yield return new WaitForSeconds(_endDialUps[1].length);
        _audioSource.pitch = 1.1f;
        _audioSource.PlayOneShot(_endDialUps[0]);

        timer = 0f;
        while (timer <= endShowTime)
        {
            timer += Time.deltaTime;
            _text.alpha = Mathf.Lerp(0f, 1f, timer / endShowTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("cable_intro");
    }
}
