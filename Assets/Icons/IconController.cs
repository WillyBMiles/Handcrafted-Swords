using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class IconController : SerializedMonoBehaviour
{
    static IconController iconController;

    [SerializeField] Image image1;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] Image image2;
    [SerializeField] TextMeshProUGUI text2;
    [SerializeField] Dictionary<Icon, Sprite> sprites = new();
    public static IReadOnlyDictionary<Stat, Icon> statToIcon = new Dictionary<Stat, Icon>()
    {
        {Stat.Knockback, Icon.Knockback },
        {Stat.Quality, Icon.Quality },
        {Stat.Damage, Icon.Damage },
        {Stat.Length, Icon.Range},
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        iconController = this;
    }

    int time = 0;
    // Update is called once per frame
    void Update()
    {
        if (time <= 0)
        {
            image1.enabled = false;
            image2.enabled = false;
            text1.text = "";
            text2.text = "";
        }
        time--;
    }

    public static void Show(Icon icon, string iconText1, Vector3 location, Icon? icon2 = null, string iconText2 = "")
    {
        iconController.ShowLocal(icon, iconText1, location, icon2, iconText2);
    }

    void ShowLocal(Icon icon, string iconText1, Vector3 location, Icon? icon2 = null, string iconText2 = "")
    {
        image1.enabled = true;
        transform.position = location;
        image1.sprite = sprites[icon];
        text1.text = iconText1;
        time = 2;

        if (icon2.HasValue)
        {
            image2.enabled = true;
            image2.sprite = sprites[icon2.Value];

        }
        else
        {
            image2.enabled = false;
        }

        text2.text = iconText2;
    }

    public static bool ShowingAnIcon()
    {
        return iconController.time > 0;
    }
}

public enum Icon
{
    Gold,
    Leave,
    Hammer,
    Sell,
    WantSword,
    Quality,
    Range,
    Damage,
    Knockback,
    Pickup
}
