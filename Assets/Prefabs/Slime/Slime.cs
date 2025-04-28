using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Slime : MonoBehaviour
{
    [SerializeField]
    int goldLost;
    [SerializeField] float health;
    [SerializeField] AudioSource die;

    CharacterController characterController;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 target = Chest.position - transform.position;
        target = new Vector3(target.x, 0f, target.z);

        transform.SetPositionAndRotation(new Vector3(transform.position.x, 10f, transform.position.z), Quaternion.LookRotation(target, Vector3.up));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = Chest.position - transform.position;
        target = new Vector3(target.x, 0f, target.z);
        transform.rotation = Quaternion.LookRotation(target, Vector3.up);
    }

    public void GetHit(float damage, float knockback)
    {
        animator.SetTrigger("Damage");
        health -= damage;
        if (health < 0)
        {
            Die();
        }
        Knockback(knockback);
    }

    void Knockback(float amount)
    {
        float knock = amount / 40f;
        

        float knockback = knock;
        DOTween.To(() =>
        knockback,
        (val) =>
        {
            knockback = val;
            if (characterController != null && characterController.enabled)
                characterController.Move(knockback * Time.deltaTime * (transform.position - Player.Instance.transform.position));
        },
        0f, .25f);
        
    }

    void Die()
    {
        animator.SetTrigger("Dead");
        Sequence s = DOTween.Sequence();
        s.AppendInterval(5f);
        s.AppendCallback(() => Destroy(gameObject));
        die.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider c = collision.collider;
        Debug.Log(c);
        
        if (c.TryGetComponent<Player>(out Player p))
        {
            p.GetHit();
        }
    }
}
