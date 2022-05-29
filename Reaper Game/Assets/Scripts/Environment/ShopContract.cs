using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContract : MonoBehaviour
{
    [SerializeField] Image wantedType;
    [SerializeField] Text wantedNum;
    [SerializeField] Image rewardType;
    [SerializeField] Text rewardNum;

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
        button.onClick.AddListener(() => ShopManager.instance.TakeContract(num));
        button.OnSelection += () => ShopManager.instance.ShowDescription(num, false);
    }

    public void SetContract(Sprite wantedImage, string wantedText, Sprite rewardImage, string rewardText)
    {
        wantedType.sprite = wantedImage;
        wantedNum.text = wantedText;
        rewardType.sprite = rewardImage;
        rewardNum.text = rewardText;
    }
}
