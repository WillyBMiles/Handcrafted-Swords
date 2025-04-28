using UnityEngine;

public class Holdable : MonoBehaviour
{
    [SerializeField]
    float pickupDistance;

    [SerializeField]
    KeyCode pickupKey = KeyCode.E;

    static Holdable holdable;
    static float closestDistance;
    Collider coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holdable == this)
            holdable = null;

        if (gameObject == Player.Instance.Holding)
        {
            coll.enabled = false;
            return;
        }
        coll.enabled = true;

        float myDist = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (myDist > pickupDistance)
        {
            return;
        }
        if (myDist < closestDistance)
        {
            holdable = this;
        }
    }

    private void LateUpdate()
    {
        if (holdable == this)
        {
            if (!IconController.ShowingAnIcon())
            {
                IconController.Show(Icon.Pickup, "E", transform.position);
            }
        }

        if (Input.GetKeyDown(pickupKey))
        {

            if (holdable == this)
            {
                Player.Instance.PickUp(gameObject);
            }
            else if (Player.Instance.Holding == gameObject)
            {
                
                Player.Instance.Drop();
            }
        }

        closestDistance = float.MaxValue;

    }
}
