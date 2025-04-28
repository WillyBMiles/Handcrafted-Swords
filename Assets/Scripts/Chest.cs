using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{

    [SerializeField] int startingMoney;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField]
    AudioSource coinSource;

    public static int Money { get; private set; }

    public static Vector3 position { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Money = startingMoney;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Money.ToString();
        position = transform.position;
    }

    public static void EarnGold(int amount)
    {
        Money += amount;
    }

    public static void LoseGold(int amount)
    {
        Money -= amount;
        if (Money <= 0)
            Money = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Slime>(out _))
        {
            if (Random.value < .1f)
                LoseGold(1);
            coinSource.DOKill();
            coinSource.volume = .2f;
            coinSource.DOFade(0f, 1f);
        }
    }

}
