using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Hammer : Sirenix.OdinInspector.SerializedMonoBehaviour
{
    [SerializeField] List<HammerRoll> rolls;

    [SerializeField, TextArea] string description;
    public string Description => description;

    [SerializeField]
    AudioSource strike;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.Holding == gameObject)
        {
            HammerDesc.desc.ShowHammer(this);
        }
    }

    public HammerRoll Roll()
    {
        HammerRoll r = rolls[UnityEngine.Random.Range(0, rolls.Count)];
        strike.Play();
        return r;
    }
}

[Serializable]
public struct HammerRoll
{
    [Tooltip("Mark if this is a 'bust'")]
    public bool bust;

    [HideIf(nameof(bust))]
    public List<StatBoost> boosts;
}

[Serializable]
public struct StatBoost
{
    public float Boost;
    public Stat Stat;
    public Operation Operation;
}

[Serializable]
public enum Operation
{
    Add,
    Multiply,
}
