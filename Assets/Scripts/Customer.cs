using DG.Tweening;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class Customer : MonoBehaviour
{
    [SerializeField] float interactionDistance = 4f;

    [SerializeField] Transform holdPosition;
    Stat? stat;

    [SerializeField]
    AudioSource sell;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    bool ready;

    GameObject holding;
    public bool ReadyToSend { get; private set; } = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) < interactionDistance)
            {

                if (Player.Instance.Holding != null && Player.Instance.Holding.TryGetComponent<Sword>(out Sword sword))
                {
                    if (stat != null)
                    {
                        IconController.Show(Icon.Sell, "Sell: Q", transform.position + Vector3.forward * 2f, IconController.statToIcon[stat.Value]);
                    }
                    else
                        IconController.Show(Icon.Sell, "Sell: Q", transform.position + Vector3.forward * 2f);
                    if (Input.GetKey(KeyCode.Q))
                    {
                        Player.Instance.Drop();
                        holding = sword.gameObject;

                        sell.Play();
                        if (stat == null)
                        {
                            Chest.EarnGold(sword.Price());
                        }
                        else
                        {
                            int money = (int) Mathf.Max(1, sword.Price() * .7f + (sword.GetStat(stat.Value) - 100f) * 2f);
                            Chest.EarnGold(money);
                        }

                    }

                }
                else
                {
                    IconController.Show(Icon.Leave, "<size=60%>Leave:</size> Q", transform.position + Vector3.forward * 2f);
                    if (Input.GetKey(KeyCode.Q))
                    {
                        Leave();
                    }

                }
            }
        }


    }

    private void LateUpdate()
    {
        if (holding != null)
        {
            holding.transform.SetPositionAndRotation(holdPosition.transform.position, holdPosition.transform.rotation);
        }
        if (!IconController.ShowingAnIcon())
        {
            if (stat != null)
            {
                IconController.Show(Icon.WantSword, "", transform.position + Vector3.forward * 2f, IconController.statToIcon[stat.Value]);
            }
            else
            {
                IconController.Show(Icon.WantSword, "", transform.position + Vector3.forward * 2f);
            }
            
        }
    }

    public void Send()
    {
        ReadyToSend = false;
        Sequence s = DOTween.Sequence();
        s.AppendInterval(2.5f);
        s.AppendCallback(() => ready = true);
        animator.SetTrigger("Enter");
        if (Random.value < .7f)
        {
            var statValues = System.Enum.GetValues(typeof(Stat));
            stat = (Stat) statValues.GetValue(Random.Range(0, statValues.Length));
        }
        else
        {
            stat = null;
        }
    }

    public void Leave()
    {
        animator.SetTrigger("Leave");
        ready = false;

        Sequence s = DOTween.Sequence();
        s.AppendInterval(2.5f);
        s.AppendCallback(() =>
        {
            if (holding != null)
            {
                Destroy(holding);
            }
            ReadyToSend = true;
        });
    }
}
