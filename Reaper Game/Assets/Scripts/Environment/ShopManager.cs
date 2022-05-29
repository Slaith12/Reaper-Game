using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance { get; private set; }

    [SerializeField] List<ShopItem> items;
    [SerializeField] List<ShopContract> contracts;
    [SerializeField] ShopDescription description;

    private void Awake()
    {
        instance = this;
        GameObject baseItem = items[0].gameObject;
        GameObject baseContract = contracts[0].gameObject;
        for(int i = 1; i < 4; i++)
        {
            GameObject newItem = Instantiate(baseItem, baseItem.transform.parent);
            Vector2 newPos = new Vector2(-50, 50);
            if (i % 2 == 1)
                newPos.x = 50;
            if (i / 2 == 1)
                newPos.y = -50;
            ((RectTransform)newItem.transform).anchoredPosition = newPos;
            items.Add(newItem.GetComponent<ShopItem>());
            items[i].SetID(i);

            GameObject newContract = Instantiate(baseContract, baseContract.transform.parent);
            newPos = new Vector2(0, 60 - 50*i);
            ((RectTransform)newContract.transform).anchoredPosition = newPos;
            contracts.Add(newContract.GetComponent<ShopContract>());
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
            description.ShowItem(id.ToString(), null, "lol", "", "");
        }
        else
        {
            description.ShowContract(id.ToString(), null, "", "", "", null, "", null);
        }
    }

    public void OpenShop(int shopID)
    {
        description.Hide();
        gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
