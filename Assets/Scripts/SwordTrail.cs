using UnityEngine;

public class SwordTrail : MonoBehaviour
{
    TrailRenderer trailRenderer;
    Holdable holdable;
    Sword sword;
    Player player;

    Collider c;
    HoldPosition holdPosition;

    [SerializeField]
    AudioSource hitSound;
    private void Awake()
    {
        holdPosition = FindFirstObjectByType<HoldPosition>();
        holdable = GetComponentInParent<Holdable>();
        sword = GetComponentInParent<Sword>();
        player = FindFirstObjectByType<Player>();
        trailRenderer = GetComponent<TrailRenderer>();
        c = GetComponent<Collider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Holding == holdable.gameObject)
        {
            trailRenderer.emitting = holdPosition.isTrailing;
            trailRenderer.startWidth = .6f * transform.lossyScale.y;
            c.enabled = holdPosition.isTrailing;
        }else
        {
            trailRenderer.emitting = false;
            c.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //deal damage
        if (other.TryGetComponent<Slime>(out Slime slime))
        {
            hitSound.Play();
            sword.Strike();
            slime.GetHit(sword.GetStat(Stat.Damage), sword.GetStat(Stat.Knockback));
        }
    }
}
