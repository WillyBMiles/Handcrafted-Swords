using DG.Tweening;
using TMPro;
using UnityEngine;

public class SwordDesc : MonoBehaviour
{
    public static SwordDesc desc;

    [SerializeField]
    TextMeshProUGUI rangeText;
    [SerializeField]
    TextMeshProUGUI damageText;
    [SerializeField]
    TextMeshProUGUI knockbackText;
    [SerializeField]
    TextMeshProUGUI qualityText;
    [SerializeField]
    TextMeshProUGUI priceText;

    [SerializeField]
    CanvasGroup canvasGroup;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desc = this;
    }

    public void ShowSword(Sword sword)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, .75f);
        rangeText.text = ((int)sword.GetStat(Stat.Length)).ToString();
        damageText.text = ((int)sword.GetStat(Stat.Damage)).ToString();
        knockbackText.text = ((int)sword.GetStat(Stat.Knockback)).ToString();
        qualityText.text = ((int)sword.GetStat(Stat.Quality)).ToString();
        priceText.text = sword.Price().ToString();
    }
}
