using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Environment
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> spawnPool;
        [SerializeField] float top, bottom, left, right;
        [SerializeField] float spawnInterval = 2;
        public int clusterMin = 3;
        public int clusterMax = 5;
        public float clusterInterval = 15;
        public float clusterIntervalVariance = 3;
        public float clusterSpacing = 8;
        public float clusterSpacingVariance = 2;

        private float cooldown;

        private void Update()
        {
            if(cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                return;
            }
            StartCoroutine(SpawnCluster(getNewQuantity(), getNewLocation()));
            cooldown = getNewCooldown();
        }

        private IEnumerator SpawnCluster(int quantity, Vector2 location)
        {
            Debug.Log($"Spawning cluster of size {quantity} at location {location}.");
            if (quantity < 1)
                quantity = 1;
            while(quantity > 0)
            {
                quantity--;
                location += getNewSpacing();
                //constrain the location to the boundaries
                location.x = Mathf.Max(Mathf.Min(location.x, right), left);
                location.y = Mathf.Max(Mathf.Min(location.y, top), bottom);
                int enemyType = Random.Range(0, spawnPool.Count);
                Debug.Log($"Creating enemy type {enemyType} at location {location}.");
                Instantiate(spawnPool[enemyType], location, Quaternion.Euler(0, 0, 0));
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        #region RNG Functions

        private int getNewQuantity()
        {
            return Random.Range(clusterMin, clusterMax);
        }

        private Vector2 getNewLocation()
        {
            return new Vector2(Random.Range(left, right), Random.Range(bottom, top));
        }

        private float getNewCooldown()
        {
            return clusterInterval + Random.value * clusterIntervalVariance - Random.value * clusterIntervalVariance;
        }

        private Vector2 getNewSpacing()
        {
            return (Random.value * 360).ToDirection() * (clusterSpacing + Random.value * clusterSpacingVariance - Random.value * clusterSpacingVariance);
        }

        #endregion

    }
}
