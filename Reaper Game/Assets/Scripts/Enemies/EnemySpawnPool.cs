using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Enemy;

namespace Reaper.Environment
{
    [CreateAssetMenu(fileName = "New Enemy Pool", menuName = "Data Container/Enemy Pool")]
    public class EnemySpawnPool : ScriptableObject
    {
        [SerializeField] private List<EnemyInfo> enemyPool;
        [SerializeField] private List<int> weights;
        [SerializeField] private int totalWeight;

        public EnemyInfo GetRandomEnemy()
        {
            if (enemyPool == null || totalWeight == 0)
                return null;
            int num = Random.Range(0, totalWeight);
            int savedNum = num;
            int index = 0;
            for (; index < enemyPool.Count && num >= weights[index]; num -= weights[index], index++) ;
            Debug.Log($"num: {savedNum} index: {index} enemy: {enemyPool[index].name}");
            return enemyPool[index];
        }

        #region List Stuff

        public int Count { get { return enemyPool.Count; } }

        public void Init()
        {
            enemyPool = new List<EnemyInfo>();
            weights = new List<int>();
            totalWeight = 0;
        }

        public void Add(EnemyInfo enemy, int weight)
        {
            enemyPool.Add(enemy);
            weights.Add(weight);
            totalWeight += weight;
        }

        public void Insert(int index, EnemyInfo enemy, int weight)
        {
            enemyPool.Insert(index, enemy);
            weights.Insert(index, weight);
            totalWeight += weight;
        }

        public void Remove(int index)
        {
            totalWeight -= weights[index];
            enemyPool.RemoveAt(index);
            weights.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
                totalWeight -= weights[index + i];
            enemyPool.RemoveRange(index, count);
            weights.RemoveRange(index, count);
        }

        public (EnemyInfo, int) this[int i]
        {
            get { return (enemyPool[i], weights[i]); }
            set
            {
                enemyPool[i] = value.Item1;
                totalWeight -= weights[i];
                weights[i] = value.Item2;
                totalWeight += value.Item2;
            }
        }

        #endregion

        private void Awake()
        {
            if (enemyPool == null)
            {
                enemyPool = new List<EnemyInfo>();
            }
            if(enemyPool.Count != weights.Count)
            {
                while (enemyPool.Count < weights.Count)
                    enemyPool.Add(null);
                while (enemyPool.Count > weights.Count)
                    weights.Add(0);
            }
        }

        private void OnValidate()
        {
            if (enemyPool == null || weights == null)
                Init();
            if(enemyPool.Count != weights.Count)
            {
                Debug.LogError($"{name} has invalid state. Adjusting weights to correct it.");
                while (weights.Count < enemyPool.Count)
                    weights.Add(0);
                if (weights.Count > enemyPool.Count)
                    weights.RemoveRange(enemyPool.Count, weights.Count - enemyPool.Count);
            }
        }
    }
}
