using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractDisplay : MonoBehaviour
{
    [SerializeField] Image wantedType;
    [SerializeField] Text wantedNum;
    [SerializeField] Image payType;
    [SerializeField] Text payNum;
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

    public void SetContract(Contract contract)
    {
        wantedNum.text = contract.targetQuota.ToString();
        wantedType.sprite = contract.targetEntity.sprite;
        payNum.text = contract.payAmount.ToString();
        payType.sprite = contract.payItem.sprite;
        rewardNum.text = contract.rewardAmount.ToString();
        rewardType.sprite = contract.rewardItem.sprite;
    }
}
