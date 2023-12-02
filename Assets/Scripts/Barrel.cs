using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject foodItem;
    public float startDelay = 2.5f;
    public bool shouldSpawnAnother = true;
    public bool hasBeenDestroyedAtLeastOnce = false;

    private float spawnPositionY = 0.75f;
    private float delayTimeSpawn = 5.0f;
    private bool isFirstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFood());
    }

    IEnumerator SpawnFood()
    {
        if (isFirstTime)
        {
            isFirstTime = false;
            yield return new WaitForSeconds(startDelay);
            StartCoroutine(SpawnFood());
        }
        else if (shouldSpawnAnother)
        {
            if (hasBeenDestroyedAtLeastOnce)
            {
                yield return new WaitForSeconds(delayTimeSpawn);
            }

            Quaternion rotation = foodItem.transform.rotation;
            float positionY = gameObject.transform.position.y + spawnPositionY;
            float positionX = gameObject.transform.position.x;
            float positionZ = gameObject.transform.position.z;
            Vector3 position;
            position = new Vector3(positionX, positionY, positionZ);

            var foodObject = Instantiate(foodItem, position, rotation);

            if (foodItem.name == "Watermelon")
            {
                var watermelon = foodObject.GetComponent<Watermelon>();
                watermelon.associatedBarrel = gameObject.name;
            }
            else
            {
                var food = foodObject.GetComponent<FoodItem>();
                food.associatedBarrel = gameObject.name;
            }

            shouldSpawnAnother = false;

            if (!hasBeenDestroyedAtLeastOnce)
            {
                yield return new WaitForSeconds(delayTimeSpawn);
            }

            StartCoroutine(SpawnFood());
        }
        else
        {
            yield return new WaitForSeconds(delayTimeSpawn);
            StartCoroutine(SpawnFood());
        }
    }
}
