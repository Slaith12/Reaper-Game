using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Enemy;

[CreateAssetMenu(fileName = "New Contract", menuName = "Data Container/Contract")]
public class Contract : ScriptableObject
{
    public string title;
    public string description;
    [Header("Soul type to get")]
    public EnemyInfo targetEntity;
    public int targetQuota;
    [Header("Initial payment for taking contract")]
    public ItemData payItem;
    public int payAmount;
    [Header("Reward for completing contract")]
    public ItemData rewardItem;
    public int rewardAmount;
}
