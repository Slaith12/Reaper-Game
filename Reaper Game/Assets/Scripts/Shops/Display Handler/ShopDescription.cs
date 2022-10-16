using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDescription : MonoBehaviour
{
    [SerializeField] new Text name;
    [SerializeField] Image image;
    [SerializeField] Text flavorText;
    [SerializeField] Text effects;
    [SerializeField] Text cost;
    [SerializeField] Text costNum;
    [SerializeField] Image costType;
    [SerializeField] Text reward;
    [SerializeField] Text rewardNum;
    [SerializeField] Image rewardType;
    [SerializeField] Sprite defaultCurrency;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowItem(ShopItem shopItem)
    {
        gameObject.SetActive(true);
        this.name.text = shopItem.item.name;
        this.image.sprite = shopItem.item.sprite;
        this.flavorText.text = shopItem.flavorText;
        this.effects.text = shopItem.item.description;
        cost.text = "Cost:";
        this.costNum.text = shopItem.price.ToString();
        costType.sprite = defaultCurrency;
        reward.gameObject.SetActive(false);
    }

    public void ShowContract(Contract contract)
    {
        gameObject.SetActive(true);
        this.name.text = contract.title;
        this.image.sprite = contract.targetEntity.sprite;
        this.flavorText.text = contract.description;
        effects.text = "";
        cost.text = "Payment:";
        costNum.text = contract.payAmount.ToString();
        costType.sprite = contract.payItem.sprite;
        reward.gameObject.SetActive(true);
        this.rewardNum.text = contract.rewardAmount.ToString();
        this.rewardType.sprite = contract.rewardItem.sprite;
    }
}
