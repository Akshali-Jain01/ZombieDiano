using System.Collections;
using UnityEngine;

public class InstantiateRandomPrefabsAndMove : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public int numberOfPrefabsToClone = 5;
    public float cloneInterval = 1.0f; // Time interval between each clone in seconds
    public float moveSpeed = 5.0f; // Speed at which the clones move

    private void Start()
    {
        StartCoroutine(ClonePrefabsAndMove());
    }

    private IEnumerator ClonePrefabsAndMove()
    {
        for (int i = 0; i < numberOfPrefabsToClone; i++)
        {
            // Randomize the y-coordinate between 1 and 10, while keeping x-coordinate at -2
            Vector3 randomPosition = new Vector3(-12f, Random.Range(5f, -5f), transform.position.z);

            // Instantiate the prefab at the randomized position
            GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, randomPosition, Quaternion.identity);

            // Calculate the target position for mfoving
            Vector3 targetPosition = new Vector3(2f, instantiatedPrefab.transform.position.y, instantiatedPrefab.transform.position.z);

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

            // Wait for the specified interval before cloning the next prefab
            yield return new WaitForSeconds(cloneInterval);
        }
    }
}
