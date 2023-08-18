using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRandomPrefabsAndMove : MonoBehaviour
{
    [System.Serializable]
    public class PrefabSpeedMapping
    {
        public GameObject prefab;
        public float moveSpeed;
    }

    public PrefabSpeedMapping[] prefabSpeedMappings; // Mapping of each prefab to its move speed
    public int numberOfPrefabsToClone = 3; // Number of prefabs to clone in each set
    public float cloneInterval = 3.0f; // Time interval between each set of clones in seconds (3 seconds)
    public string playerTag = "Player"; // Tag to identify the player GameObject
    public GameObject projectilePrefab; // Prefab of the projectile to shoot
    public float shootingInterval = 1.0f; // Time interval between each shooting in seconds

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CloneAndMovePrefabs());
    }

    private IEnumerator CloneAndMovePrefabs()
    {
        while (true) // Infinite loop to continuously clone and move prefabs
        {
            for (int i = 0; i < numberOfPrefabsToClone; i++)
            {
                // Randomly select a prefab-speed mapping from the array
                PrefabSpeedMapping randomMapping = prefabSpeedMappings[Random.Range(0, prefabSpeedMappings.Length)];

                // Randomize the y-coordinate between -5 and 5, while keeping x-coordinate at -12
                Vector3 randomPosition = new Vector3(-12f, Random.Range(-5f, 5f), transform.position.z);

                // Check for collisions with existing clones and adjust the position if needed
                randomPosition = AdjustPositionToAvoidCollisions(randomPosition);

                // Instantiate the prefab at the adjusted position
                GameObject instantiatedPrefab = Instantiate(randomMapping.prefab, randomPosition, Quaternion.identity);
                instantiatedPrefabs.Add(instantiatedPrefab);

                // Calculate the target position for moving
                Vector3 targetPosition = new Vector3(0f, instantiatedPrefab.transform.position.y, instantiatedPrefab.transform.position.z);

                float moveSpeed = randomMapping.moveSpeed;

                // Move the instantiated prefab to the target position
                while (Vector3.Distance(instantiatedPrefab.transform.position, targetPosition) > 0.05f)
                {
                    instantiatedPrefab.transform.position = Vector3.MoveTowards(
                        instantiatedPrefab.transform.position,
                        targetPosition,
                        moveSpeed * Time.deltaTime
                    );

                    yield return null;
                }

                // Start continuous shooting from the newly instantiated prefab
                StartCoroutine(ContinuousShooting(instantiatedPrefab));
            }

            // Wait for the specified interval before cloning the next set of prefabs
            yield return new WaitForSeconds(cloneInterval);
        }
    }

    private bool CheckForCollisions(Vector3 position)
    {
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (Vector3.Distance(prefab.transform.position, position) < 1.0f)
            {
                return true; // Collision detected
            }
        }
        return false; // No collision detected
    }

    private Vector3 AdjustPositionToAvoidCollisions(Vector3 originalPosition)
    {
        Vector3 adjustedPosition = originalPosition;
        float stepSize = 1.0f; // Step size for adjusting position

        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            // Check for collisions at the adjusted position
            if (!CheckForCollisions(adjustedPosition))
            {
                return adjustedPosition; // No collision, return the adjusted position
            }

            // Adjust the position by the step size
            adjustedPosition += new Vector3(stepSize, stepSize, 0f);

            // Limit the position adjustments to avoid going beyond boundaries
            adjustedPosition.x = Mathf.Clamp(adjustedPosition.x, -12f, 12f);
            adjustedPosition.y = Mathf.Clamp(adjustedPosition.y, -5f, 5f);
        }

        return originalPosition; // If no empty position is found nearby, return the original position
    }

    private IEnumerator ContinuousShooting(GameObject prefab)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag); // Find the player GameObject by tag
        while (true)
        {
            if (player != null && projectilePrefab != null)
            {
                Vector3 direction = (player.transform.position - prefab.transform.position).normalized;
                Vector2 shootingDirection = new Vector2(direction.x, direction.y);
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootingDirection);
                GameObject projectile = Instantiate(projectilePrefab, prefab.transform.position, rotation);
                Destroy(projectile, 5.0f); // Destroy the projectile after 5 seconds
            }

            yield return new WaitForSeconds(shootingInterval);
        }
    }
}
