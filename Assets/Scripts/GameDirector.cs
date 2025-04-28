using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameDirector : MonoBehaviour
{

    [SerializeField]
    List<Transform> spawnLocations = new();
    [SerializeField]
    List<GameObject> enemyprefabs = new();

    [SerializeField]
    Color nightColor;

    [SerializeField]
    Transform lightParent;

    [SerializeField]
    Customer customer;

    [SerializeField]
    AudioSource dayMusic;
    [SerializeField]
    AudioSource nightMusic;
    [SerializeField]
    AudioSource nightTrigger;
    [SerializeField] float musicVolume;

    ColorAdjustments colorAdjustments;

    public bool isDay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Volume volume = FindFirstObjectByType<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        StartCoroutine(FullGame());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator FullGame()
    {
        for (int day =0; ; day++)
        {
            yield return Day(day);

            yield return Night(day);
        }

    }

   

    IEnumerator Day(int day)
    {
        lightParent.DORotate(new Vector3(19.9f, 0f, 0f), 2f);
        DOTween.To(() => colorAdjustments.contrast.value,
    (val) => colorAdjustments.contrast.value = val,
    0f, 2f);
        DOTween.To(() => colorAdjustments.colorFilter.value,
            (val) => colorAdjustments.colorFilter.value = val,
            Color.white, 2f);

        dayMusic.DOFade(musicVolume, 2f);
        nightMusic.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);
        for (int i =0; i < CustomersByDay(day); i++)
        {
            yield return ShowCustomer();
            yield return new WaitForSeconds(1f);
        }
        lightParent.DORotate(new Vector3(-21.73f, 0f, 0f), 2f);

        DOTween.To(() => colorAdjustments.contrast.value,
    (val) => colorAdjustments.contrast.value = val,
    19.5f, 2f);
        DOTween.To(() => colorAdjustments.colorFilter.value,
            (val) => colorAdjustments.colorFilter.value = val,
            nightColor, 2f);
        nightMusic.DOFade(musicVolume, 2f);
        dayMusic.DOFade(0f, 2f);

        yield return new WaitForSeconds(2f);
    }

    IEnumerator ShowCustomer()
    {
        customer.Send();
        yield return new WaitUntil(() => customer.ReadyToSend);
    }

    int CustomersByDay(int day)
    {
        if (day < 2)
            return 1;
        if (day < 6)
            return 2;
        return (int) ( 2 + Mathf.Log(day) / 10);
    }


    List<GameObject> allEnemies = new();
    IEnumerator Night(int day)
    {
        nightTrigger.Play();
        int enemies = EnemiesByNight(day);
        for (int i = 0; i < enemies; )
        {
            int toSpawn = Mathf.Min(Random.Range(1, 3), enemies - i);
            i += toSpawn;
            for (int j =0; j < toSpawn; j++)
            {
                Transform loc = spawnLocations[Random.Range(0, spawnLocations.Count)];
                GameObject e = enemyprefabs[Random.Range(0, enemyprefabs.Count)];
                allEnemies.Add(Instantiate(e,loc.transform.position, loc.transform.rotation));
            }

            yield return new WaitUntil(() =>
            {
                allEnemies.RemoveAll(e => e == null);
                return !allEnemies.Any();
            });
        }
        yield break;
    }

    int EnemiesByNight(int day)
    {
        if (day < 1)
            return 1;
        if (day < 3)
            return 2;
        return 2 + day / 5;
    }
}
