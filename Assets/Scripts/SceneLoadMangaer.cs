using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class SceneLoadManager : MonoBehaviour
{
    [Header("Loading Bar")]
    [SerializeField] private Image loadBar;
    [SerializeField] private float halfTime = 1.5f;
    [SerializeField] private float fullTime = 1.5f;
    [SerializeField] private CanvasGroup objectsFeel;

    [Header("Radial Transition")]
    [SerializeField] private Image radialMask;
    [SerializeField] private float radialTime = 1f;

    [Header("Loading Dots")]
    [SerializeField] private List<TextMeshProUGUI> dots = new List<TextMeshProUGUI>();
    [SerializeField] private float dotInterval = 0.4f;

    private void Start()
    {
        AnimateDots();
        PlayLoadingSequence(1);
    }

    private void PlayLoadingSequence(int sceneIndex)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(loadBar.DOFillAmount(0.5f, halfTime).SetEase(Ease.OutBounce));
        seq.Append(loadBar.DOFillAmount(1f, fullTime).SetEase(Ease.InOutQuad));

        seq.OnComplete(() =>
        {
            objectsFeel.DOFade(0f, 1f).OnComplete(() =>
            {
                objectsFeel.gameObject.SetActive(false);

                DOVirtual.DelayedCall(0.5f, () =>
                {
                    radialMask.fillAmount = 1f;
                    radialMask.gameObject.SetActive(true);

                    radialMask.DOFillAmount(0f, radialTime)
                              .SetEase(Ease.OutBounce)
                              .OnComplete(() =>{SceneManager.LoadScene(sceneIndex);});
                });
            });
        });
    }

    private void AnimateDots()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].alpha = 0;
        }

        Sequence dotSeq = DOTween.Sequence();

        for (int i = 0; i < dots.Count; i++)
        {
            dotSeq.Append(dots[i].DOFade(1, 0.2f));
            dotSeq.AppendInterval(dotInterval);
        }

        for (int i = 0; i < dots.Count; i++)
        {
            dotSeq.Append(dots[i].DOFade(0, 0.2f));
        }

        dotSeq.SetLoops(-1);
    }
}
