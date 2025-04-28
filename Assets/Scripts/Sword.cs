using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Dictionary<Stat, float> stats = new();
    [SerializeField]
    Transform bladeTransform;
    [SerializeField] Transform crossbarTransform;
    [SerializeField] Transform pommelTransform;

    [SerializeField] AudioSource breakSound;

    [SerializeField] GameObject spawnOnDestroy;

    private void Awake()
    {
        stats[Stat.Length] = 100;
        stats[Stat.Damage] = 100;
        stats[Stat.Knockback] = 100;
        stats[Stat.Quality] = 100;
    }

    public void Strike()
    {
        stats[Stat.Quality] *= .8f;
        if (stats[Stat.Quality] < .1f){
            DestroySword();
        }
    }

    public int Price()
    {
        float sumOfAllStats = stats.Except(stats.Where(kvp => kvp.Key == Stat.Quality)).Select(kvp => {
            float value = kvp.Value;
            float above = 20 + Mathf.Pow((value - 100) / 2f, 1.2f);
            float below = value / 5f;
            return kvp.Value > 100 ? above : below;
            }).Sum();
        return (int) ( sumOfAllStats * stats[Stat.Quality] / 100f); 
    }

    public void Boost(StatBoost statBoost)
    {
        switch (statBoost.Operation)
        {
            case Operation.Add:
                stats[statBoost.Stat] += statBoost.Boost;
                break;
            case Operation.Multiply:
                stats[statBoost.Stat] *= statBoost.Boost;
                break;
        }

        bladeTransform.localScale = new Vector3(1f, stats[Stat.Length] / 100, 1f);
        pommelTransform.localScale = new Vector3(stats[Stat.Damage], 100f, stats[Stat.Damage]) / 100;
        crossbarTransform.localScale = new Vector3(1f, 1f, stats[Stat.Knockback] / 100f); 
    }

    private void Update()
    {
        if (Player.Instance.Holding == gameObject)
        {
            SwordDesc.desc.ShowSword(this);
        }
    }

    public float GetStat(Stat stat)
    {
        return stats[stat];
    }


    public void DestroySword()
    {

        //...
        breakSound.Play();
        Destroy(gameObject);
        Instantiate(spawnOnDestroy, transform.position, transform.rotation);
    }

}
public enum Stat
{
    Length,
    Damage,
    Knockback,
    Quality
}