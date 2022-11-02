using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reaper.Shops
{
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
            name.text = shopItem.item.name;
            image.sprite = shopItem.item.sprite;
            flavorText.text = shopItem.flavorText;
            effects.text = shopItem.item.description;
            cost.text = "Cost:";
            costNum.text = shopItem.price.ToString();
            costType.sprite = defaultCurrency;
            reward.gameObject.SetActive(false);
        }

        public void ShowContract(Contract contract)
        {
            gameObject.SetActive(true);
            name.text = contract.title;
            image.sprite = contract.targetEntity.sprite;
            flavorText.text = contract.description;
            effects.text = "";
            cost.text = "Payment:";
            costNum.text = contract.payAmount.ToString();
            costType.sprite = contract.payItem.sprite;
            reward.gameObject.SetActive(true);
            rewardNum.text = contract.rewardAmount.ToString();
            rewardType.sprite = contract.rewardItem.sprite;
        }
    }
}