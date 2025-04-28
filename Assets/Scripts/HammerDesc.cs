using TMPro;
using UnityEngine;
using DG.Tweening;

public class HammerDesc : MonoBehaviour
{
    public static HammerDesc desc;

    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desc = this;
    }

    public void ShowHammer(Hammer hammer)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, .75f);
        text.text = hammer.Description;
    }
}
