using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] new Text name;
    [SerializeField] Image image;
    [SerializeField] Text price;

    CustomButton button;

    private void Awake()
    {
        button = GetComponent<CustomButton>();
    }

    public void Enable()
    {
        button.interactable = true;
    }

    public void Disable()
    {
        button.interactable = false;
    }

    public void SetID(int num)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ShopManager.instance.OrderItem(num));
        button.OnSelection += () => ShopManager.instance.ShowDescription(num, true);
    }

    public void SetItem(string name, Sprite image, int price)
    {
        this.name.text = name;
        this.image.sprite = image;
        this.price.text = price.ToString();
    }
}
