using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform itemTemplate;
    public GameObject[] Items;

    public UI_Shop ui;

    private void Awake()
    {
        container = transform.Find("container");
        itemTemplate = container.Find("shopItemTemplate");
        itemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(Items[0], ItemCosts.GetCost(ItemCosts.ItemType.MoveSpeed), 0);
        CreateItemButton(Items[1], ItemCosts.GetCost(ItemCosts.ItemType.HealPoition), 1);
        CreateItemButton(Items[2], ItemCosts.GetCost(ItemCosts.ItemType.BulletSize), 2);
        CreateItemButton(Items[3], ItemCosts.GetCost(ItemCosts.ItemType.ATKspeed), 3);
    }

    private void Update()
    {
        if (GameController.instance.playerIsInShop) ShowShop();
        else HideShop();
    }

    private void CreateItemButton(GameObject item, int itemCost, int position)
    {
        Sprite itemSprite = item.GetComponent<SpriteRenderer>().sprite;

        Transform shopItemTransform=Instantiate(itemTemplate, container);
        RectTransform shopItemRectTransform= shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 50f;
        shopItemRectTransform.anchoredPosition= new Vector2(0, -shopItemHeight * position);

        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(item.name);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("Image").GetComponent<Image>().sprite = itemSprite;
        shopItemTransform.name= item.name;
    }

    public void BuyButtonClicked(TextMeshProUGUI text)
    {
        if (text.text == "ATK+") TryBuyItem(ItemCosts.ItemType.ATKspeed);
        else if (text.text == "MVSPD+") TryBuyItem(ItemCosts.ItemType.MoveSpeed);
        else if (text.text == "BLT+") TryBuyItem(ItemCosts.ItemType.BulletSize);
        else if (text.text == "HP+") TryBuyItem(ItemCosts.ItemType.HealPoition);
    }

    private void TryBuyItem(ItemCosts.ItemType item)
    {
        int money= GameController.Money;
        int cost= ItemCosts.GetCost(item);
        if (money - cost >= 0)
        {
            BuyItem(item);
            GameController.ChangeMoney(-cost);
        }
    }



    private void BuyItem(ItemCosts.ItemType item)
    {
        switch (item)
        {
            case ItemCosts.ItemType.MoveSpeed: 
                GameController.SpeedChange(0.2f);
                break;
            case ItemCosts.ItemType.BulletSize:
                GameController.bulletsizeChange(0.1f);
                break;
            case ItemCosts.ItemType.HealPoition:
                GameController.HealPlayer(1);
                break;
            case ItemCosts.ItemType.ATKspeed:
                GameController.atkSpeedchange(0.1f);
                break;
            default:
                break;
        }
    }

    public void ShowShop()
    {
        Transform itemTemplate2; 
        itemTemplate2 = container.Find(Items[0].name);
        itemTemplate2.gameObject.SetActive(true);
        itemTemplate2 = container.Find(Items[1].name);
        itemTemplate2.gameObject.SetActive(true);
        itemTemplate2 = container.Find(Items[2].name);
        itemTemplate2.gameObject.SetActive(true);
        itemTemplate2 = container.Find(Items[3].name);
        itemTemplate2.gameObject.SetActive(true);

    }

    public void HideShop()
    {
        Transform itemTemplate2;
        itemTemplate2 = container.Find(Items[0].name);
        itemTemplate2.gameObject.SetActive(false);
        itemTemplate2 = container.Find(Items[1].name);
        itemTemplate2.gameObject.SetActive(false);
        itemTemplate2 = container.Find(Items[2].name);
        itemTemplate2.gameObject.SetActive(false);
        itemTemplate2 = container.Find(Items[3].name);
        itemTemplate2.gameObject.SetActive(false);
    }

}
