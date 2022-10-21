using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Environment
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<GameObject> spawnPool;
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
            SpawnCluster(getNewQuantity(), getNewLocation());
            cooldown = getNewCooldown();
        }

        private void SpawnCluster(int quantity, Vector2 location)
        {
            Debug.Log($"Spawning cluster of size {quantity} at location {location}.");
            if (quantity < 1)
                quantity = 1;
            while(quantity > 0)
            {
                quantity--;
                Vector2 currentLocation = location + getNewSpacing();
                //constrain the location to the boundaries
                currentLocation.x = Mathf.Max(Mathf.Min(currentLocation.x, right), left);
                currentLocation.y = Mathf.Max(Mathf.Min(currentLocation.y, top), bottom);
                int enemyType = Random.Range(0, spawnPool.Count);
                Debug.Log($"Creating enemy type {enemyType} at location {currentLocation}.");
                StartCoroutine(SpawnDelayed(spawnPool[enemyType], currentLocation, getRandomDelay()));
            }
        }

        private IEnumerator SpawnDelayed(GameObject gameObject, Vector2 location, float delay)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(gameObject, location, Quaternion.Euler(0, 0, 0));
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
            return (Random.value * 360).ToDirection() * (enemySpacing * Random.value);
        }

        private float getRandomDelay()
        {
            return spawnVariance * Random.value;
        }

        #endregion

    }
}
