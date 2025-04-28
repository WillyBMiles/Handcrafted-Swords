using System.Collections.Generic;
using UnityEngine;

public class HammerVendingMachine : MonoBehaviour
{
    [SerializeField]
    float interactionRange;
    [SerializeField]
    int cost;
    [SerializeField]
    AudioSource playThis;
    [SerializeField]
    Transform position;

    [SerializeField]
    List<Hammer> hammers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (hammers.Count == 0)
            Destroy(gameObject);
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < interactionRange)
        {
            IconController.Show(Icon.Hammer, "Q", transform.position + Vector3.right * 2f, Icon.Gold, cost.ToString());
            if (Chest.Money >= cost && Input.GetKeyDown(KeyCode.Q))
            {
                Buy();
            }
        }
    }

    public void Buy()
    {
        playThis.Play();
        Chest.LoseGold(cost);
        cost *= 2;
        int index = Random.Range(0, hammers.Count);
        Instantiate(hammers[index], position.position, position.rotation);
        hammers.RemoveAt(index);
    }
}
