using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadShopData
{
    public struct Item
    {
        public string name;
        public Sprite image;
        public string flavorText;
        public string effects;
        public int price;
        public event Action OnBuy;

        public Item(string name, Sprite image, string flavorText, string effects, int price, Action buyEffect)
        {
            this.name = name;
            this.image = image;
            this.flavorText = flavorText;
            this.effects = effects;
            this.price = price;
            OnBuy = buyEffect;
        }

        public void Buy()
        {
            OnBuy?.Invoke();
        }
    }

    public struct Contract
    {
        public string title;

        public string wantedNum;
        public Sprite wantedImg;
        public string wantedType;

        public string payNum;
        public Sprite payImg;
        public string payType;

        public string rewardNum;
        public Sprite rewardImg;
        public string rewardType;

        public Contract(string title, string wantedNum, Sprite wantedImg, string wantedType, string payNum, Sprite payImg, string payType, string rewardNum, Sprite rewardImg, string rewardType)
        {
            this.title = title;
            this.wantedNum = wantedNum;
            this.wantedImg = wantedImg;
            this.wantedType = wantedType;
            this.payNum = payNum;
            this.payImg = payImg;
            this.payType = payType;
            this.rewardNum = rewardNum;
            this.rewardImg = rewardImg;
            this.rewardType = rewardType;
        }
    }

    public List<Item> items;
    public List<Contract> contracts;

    public BadShopData()
    {
        items = new List<Item>();
        contracts = new List<Contract>();
    }

    public void AddItem(int itemID, string flavorText, string effects, int price, Action OnBuy)
    {
        //AddItem(GlobalShopData.instance.itemNames[itemID], GlobalShopData.instance.itemSprites[itemID], flavorText, effects, price, OnBuy);
    }

    public void AddItem(string name, Sprite image, string flavorText, string effects, int price, Action OnBuy)
    {
        items.Add(new Item(name, image, flavorText, effects, price, OnBuy));
    }

    public void AddContract(string title, string wantedNum, string payNum, string rewardNum)
    {
        AddContract(title, wantedNum, 0, payNum, 1, rewardNum, 0);
    }

    public void AddContract(string title, string wantedNum, int wantedID, string payNum, int payID, string rewardNum, int rewardID)
    {
        //AddContract(title, wantedNum, GlobalShopData.instance.enemySprites[wantedID], GlobalShopData.instance.enemyNames[wantedID], payNum, GlobalShopData.instance.itemSprites[payID], GlobalShopData.instance.itemNames[payID], rewardNum, GlobalShopData.instance.itemSprites[rewardID], GlobalShopData.instance.itemNames[rewardID]);
    }

    public void AddContract(string title, string wantedNum, Sprite wantedImg, string wantedType, string payNum, Sprite payImg, string payType, string rewardNum, Sprite rewardImg, string rewardType)
    {
        contracts.Add(new Contract(title, wantedNum, wantedImg, wantedType, payNum, payImg, payType, rewardNum, rewardImg, rewardType));
    }
}
