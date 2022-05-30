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

    public void ShowItem(string name, Sprite image, string flavorText, string effects, string costNum)
    {
        gameObject.SetActive(true);
        this.name.text = name;
        this.image.sprite = image;
        this.flavorText.text = flavorText;
        this.effects.text = effects;
        cost.text = "Cost:";
        this.costNum.text = costNum;
        costType.sprite = defaultCurrency;
        reward.gameObject.SetActive(false);
    }

    public void ShowContract(string name, Sprite image, string flavorText, string info, string payNum, Sprite payType, string rewardNum, Sprite rewardType)
    {
        gameObject.SetActive(true);
        this.name.text = name;
        this.image.sprite = image;
        this.flavorText.text = flavorText;
        effects.text = info;
        cost.text = "Payment:";
        costNum.text = payNum;
        costType.sprite = payType;
        reward.gameObject.SetActive(true);
        this.rewardNum.text = rewardNum;
        this.rewardType.sprite = rewardType;
    }
}
