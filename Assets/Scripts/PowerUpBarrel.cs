using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBarrel : MonoBehaviour
{
    public GameObject powerUpItem;
    public bool shouldSpawnAnother = true;
    public bool hasBeenDestroyedAtLeastOnce = false;

    private float startDelay = 2.5f;
    private float spawnPositionY = 0.75f;
    private float delayTimeSpawn = 10.0f;
    private bool isFirstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnPowerUp()
    {
        if (isFirstTime)
        {
            isFirstTime = false;
            yield return new WaitForSeconds(startDelay);
            StartCoroutine(SpawnPowerUp());
        }
        else if (shouldSpawnAnother)
        {
            if (hasBeenDestroyedAtLeastOnce)
            {
                yield return new WaitForSeconds(delayTimeSpawn);
            }

            Quaternion rotation = powerUpItem.transform.rotation;
            float positionY = gameObject.transform.position.y + spawnPositionY;
            float positionX = gameObject.transform.position.x;
            float positionZ = gameObject.transform.position.z;
            Vector3 position;
            position = new Vector3(positionX, positionY, positionZ);

            var powerUpObject = Instantiate(powerUpItem, position, rotation);

            var powerUp = powerUpObject.GetComponent<PowerUp>();
            powerUp.associatedBarrel = gameObject.name;

            shouldSpawnAnother = false;

            if (!hasBeenDestroyedAtLeastOnce)
            {
                yield return new WaitForSeconds(delayTimeSpawn);
            }

            StartCoroutine(SpawnPowerUp());
        }
        else
        {
            yield return new WaitForSeconds(delayTimeSpawn);
            StartCoroutine(SpawnPowerUp());
        }
    }
}
