using Reaper.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Environment
{
    public class EnemySpawner : MonoBehaviour
    {
        public EnemySpawnPool spawnPool;
        [SerializeField] float top, bottom, left, right;
        [Tooltip("When a cluster is spawned, how long will it take for all enemies to be spawned")]
        [SerializeField] float spawnVariance = 2;

        [Tooltip("Minimum Cluster Size")]
        public int clusterMin = 3;
        [Tooltip("Maximum Cluster Size")]
        public int clusterMax = 5;

        [Tooltip("Standard time between clusters")]
        public float clusterInterval = 15;
        [Tooltip("Time between clusters")]
        public float clusterIntervalVariance = 3;

        [Tooltip("Space within clusters")]
        public float enemySpacing = 8;

        private float cooldown;

        private void Start()
        {
            cooldown = 3;
        }

        private void Update()
        {
            if(cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }
            SpawnCluster(GetNewQuantity(), GetNewLocation());
            cooldown = GetNewCooldown();
        }

        private void SpawnCluster(int quantity, Vector2 location)
        {
            if (quantity < 1)
                quantity = 1;
            while(quantity > 0)
            {
                quantity--;
                Vector2 currentLocation = location + GetNewSpacing();
                //constrain the location to the boundaries
                currentLocation.x = Mathf.Max(Mathf.Min(currentLocation.x, right), left);
                currentLocation.y = Mathf.Max(Mathf.Min(currentLocation.y, top), bottom);
                EnemyInfo enemyType = spawnPool.GetRandomEnemy();
                StartCoroutine(SpawnDelayed(enemyType, currentLocation, GetRandomDelay()));
            }
        }

        private IEnumerator SpawnDelayed(EnemyInfo enemy, Vector2 location, float delay)
        {
            yield return new WaitForSeconds(delay);
            enemy.Create(location);
        }

        #region RNG Functions

        private int GetNewQuantity()
        {
            return Random.Range(clusterMin, clusterMax);
        }

        private Vector2 GetNewLocation()
        {
            return new Vector2(Random.Range(left, right), Random.Range(bottom, top));
        }

        private float GetNewCooldown()
        {
            return clusterInterval + Random.value * clusterIntervalVariance - Random.value * clusterIntervalVariance;
        }

        private Vector2 GetNewSpacing()
        {
            return (Random.value * 360).ToDirection() * (enemySpacing * Random.value);
        }

        private float GetRandomDelay()
        {
            return spawnVariance * Random.value;
        }

        #endregion

    }
}
