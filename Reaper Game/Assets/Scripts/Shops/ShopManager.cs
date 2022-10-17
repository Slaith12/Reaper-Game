using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance { get; private set; }

    [SerializeField] List<ItemDisplay> items;
    [SerializeField] List<ContractDisplay> contracts;
    [SerializeField] ShopDescription description;
    ShopData currentShop;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameObject baseItem = items[0].gameObject;
        GameObject baseContract = contracts[0].gameObject;
        for (int i = 1; i < 4; i++)
        {
            GameObject newItem = Instantiate(baseItem, baseItem.transform.parent);
            Vector2 newPos = new Vector2(-50, 50);
            if (i % 2 == 1)
                newPos.x = 50;
            if (i / 2 == 1)
                newPos.y = -50;
            ((RectTransform)newItem.transform).anchoredPosition = newPos;
            items.Add(newItem.GetComponent<ItemDisplay>());
            items[i].SetID(i);

            GameObject newContract = Instantiate(baseContract, baseContract.transform.parent);
            newPos = new Vector2(0, 60 - 50 * i);
            ((RectTransform)newContract.transform).anchoredPosition = newPos;
            contracts.Add(newContract.GetComponent<ContractDisplay>());
            contracts[i].SetID(i);
        }
        items[0].SetID(0);
        contracts[0].SetID(0);

        gameObject.SetActive(false);
        description.Hide();
    }

    public void OrderItem(int id)
    {
        CloseShop();
    }

    public void TakeContract(int id)
    {
        CloseShop();
    }

    public void ShowDescription(int id, bool item)
    {
        if(item)
        {
            ShopItem currItem = currentShop.items[id];
            description.ShowItem(currItem);
        }
        else
        {
            Contract currCont = currentShop.contracts[id];
            description.ShowContract(currCont);
        }
    }

    public void OpenShop(ShopData shopData)
    {
        if (shopData == null)
            return;
        description.Hide();
        gameObject.SetActive(true);
        currentShop = shopData;
        for(int i = 0; i < 4; i++)
        {
            if (currentShop.items.Count <= i)
            {
                items[i].gameObject.SetActive(false);
            }
            else
            {
                items[i].gameObject.SetActive(true);
                ShopItem currentItem = currentShop.items[i];
                items[i].SetItem(currentItem);
            }
            if(currentShop.contracts.Count <= i)
            {
                contracts[i].gameObject.SetActive(false);
            }
            else
            {
                contracts[i].gameObject.SetActive(true);
                Contract currentContract = currentShop.contracts[i];
                contracts[i].SetContract(currentContract);
            }
        }
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
