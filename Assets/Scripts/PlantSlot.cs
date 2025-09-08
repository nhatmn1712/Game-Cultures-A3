using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantSlot : MonoBehaviour
{
    public Sprite plantSprite;
    public GameObject plantObject;
    public int price;

    public Image icon;
    public TextMeshProUGUI priceText;

    // NOTE: match the exact class name of your manager script
    private Gamemanager gms;

    private void Start()
    {
        // find the Gamemanager component on the object named "GameManager"
        gms = GameObject.Find("GameManager").GetComponent<Gamemanager>();

        // hook up the button
        GetComponent<Button>().onClick.AddListener(BuyPlant);

        // optional: update the UI at start
        OnValidate();
    }

    private void BuyPlant()
    {
        if (gms == null)
        {
            Debug.LogError("[PlantSlot] Gamemanager not found.");
            return;
        }

        gms.BuyPlant(plantObject, plantSprite, price);
    }

    private void OnValidate()
    {
        if (icon != null)
        {
            icon.enabled = plantSprite != null;
            icon.sprite = plantSprite;
        }

        if (priceText != null)
            priceText.text = price.ToString();
    }
}
