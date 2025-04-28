using System.Collections;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    public static Anvil anvil;

    [SerializeField]
    Sword swordPrefab;

    [SerializeField]
    Transform heldSwordParent;

    [SerializeField]
    int rawMaterialCost;

    [SerializeField]
    KeyCode interactKey;

    [SerializeField]
    float interactionDistance;
    [SerializeField]
    AudioSource craft;

    Sword currentSword;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anvil = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < interactionDistance )
        {
            if (currentSword != null)
                SwordDesc.desc.ShowSword(currentSword);
            if (Player.Instance.Holding != null && Player.Instance.Holding.TryGetComponent<Hammer>(out Hammer hammer))
            {
                if (currentSword != null)
                {
                    IconController.Show(Icon.Hammer, "Q", transform.position + Vector3.up * 4f, Icon.Pickup, "E");
                }
                else
                {
                    IconController.Show(Icon.Hammer, "Q", transform.position + Vector3.up * 4f, Icon.Gold, rawMaterialCost.ToString());
                }

                if (Input.GetKeyDown(KeyCode.Q) && Player.Instance.reasonsWeCantMove.Count == 0)
                {
                    
                    Interact(hammer);
                }
            }

            if (Input.GetKeyUp(KeyCode.E) && currentSword != null)
            {
                Player.Instance.PickUp(currentSword.gameObject);
                currentSword = null;
            }

        }
    }

    public void Interact(Hammer hammer)
    {
        StartCoroutine(InteractCoroutine(hammer));
    }
    public void Holding(GameObject go)
    {
        if (currentSword != null && go == currentSword.gameObject)
            currentSword = null;
    }

    IEnumerator InteractCoroutine(Hammer hammer)
    {
        if (currentSword == null)
        {
            craft.Play();
            //Show oops no money
            if (Chest.Money < rawMaterialCost)
                yield break;
        }
        //hammer animation
        Player.Instance.Hammer(transform.position);
        yield return new WaitForSeconds(.25f);

        if (currentSword == null) { 
            Chest.LoseGold(rawMaterialCost);
            currentSword = Instantiate(swordPrefab, heldSwordParent.position, heldSwordParent.rotation);
        }


        HammerRoll roll = hammer.Roll();
        if (roll.bust)
        {
            //Sword breaks
            currentSword.DestroySword();

            yield break;
        }

        foreach (var boost in roll.boosts)
        {
            currentSword.Boost(boost);
        }
    }
}
