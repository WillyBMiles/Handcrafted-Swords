using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    float speed;

    [SerializeField]
    Transform holdPostion;
    Animator animator;


    [SerializeField]
    AudioSource slash;

    public GameObject Holding { get; private set; }
    Sword sword;
    Rigidbody holdingRb;

    public List<string> reasonsWeCantMove = new();

    CharacterController characterController;

    private void Awake()
    {
        Instance = this;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (reasonsWeCantMove.Count > 0)
            return;
        if (sword != null)
        {
            if (Input.GetMouseButton(0))
            {
                Sequence s2 = DOTween.Sequence();
                s2.AppendInterval(.2f);
                s2.AppendCallback(() => slash.Play());
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, LayerMask.GetMask("Clickable"));
                LookTowards(hit.point);

                reasonsWeCantMove.Add("Attacking");

                animator.SetTrigger("Attack");
                Sequence s = DOTween.Sequence();
                s.AppendInterval(.75f);
                s.AppendCallback(() =>
                {
                    reasonsWeCantMove.Remove("Attacking");
                    animator.ResetTrigger("Attack");
                });
            }
        }


        MoveUpdate();

    }

    void MoveUpdate()
    {

        if (reasonsWeCantMove.Count > 0)
            return;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, 0f, y);
        if (move.sqrMagnitude > 1f)
            move = move.normalized;
        if (move.sqrMagnitude > .2f)
            LookTowards(transform.position + move);
        move *= speed * Time.deltaTime;


        characterController.Move(move + Vector3.down);
    }

    private void LateUpdate()
    {
        if (Holding != null)
        {
            Holding.transform.SetPositionAndRotation(holdPostion.position, holdPostion.rotation);
            if (holdingRb != null && holdingRb.gameObject == Holding)
            {
                //pass
            }
            else
            {
                holdingRb = Holding.GetComponent<Rigidbody>();
            }
            holdingRb.linearVelocity = new Vector3();

            Anvil.anvil.Holding(Holding);
        }
        
    }

    public void PickUp(GameObject gameObject)
    {
        Holding = gameObject;
        sword = Holding.GetComponent<Sword>();
    }

    public void Drop()
    {
        if (Holding != null)
        {
            Rigidbody rb = Holding.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 5f + Random.insideUnitSphere * .1f, ForceMode.Impulse);
        }
        Holding = null;
    }
    
    public void LookTowards(Vector3 position)
    {
        Vector3 target = position - transform.position;
        target = new Vector3(target.x, 0f, target.z);
        transform.rotation = Quaternion.LookRotation(target);
    }

    public void Hammer(Vector3 anvilLocation)
    {
        LookTowards(anvilLocation);
        animator.SetTrigger("Hammer");
        reasonsWeCantMove.Add("Hammering");
        Sequence s = DOTween.Sequence();
        s.AppendInterval(.5f);
        s.AppendCallback(() =>
        {
            reasonsWeCantMove.Remove("Hammering");
            animator.ResetTrigger("Hammer");
        }
            );
    }

    public void GetHit()
    {
        //Knocked back?
    }
}
