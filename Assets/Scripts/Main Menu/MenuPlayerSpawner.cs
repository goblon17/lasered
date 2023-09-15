using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuPlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform playerPosition;
    [SerializeField]
    private SerializedDictionary<GameObject, float> playerPrefabs;

    private void Start()
    {
        List<float> weights = new List<float>(playerPrefabs.Values);
        List<GameObject> gameObjects = new List<GameObject>(playerPrefabs.Keys.Select(x => x.Item));
        float weightSum = weights.Sum();
        float randFloat = Random.Range(0, weightSum);
        GameObject gameObjectToSpawn = null;
        for (int i = 0; i < weights.Count; i++)
        {
            if (weights[i] > randFloat)
            {
                gameObjectToSpawn = gameObjects[i];
                break;
            }

            randFloat -= weights[i];
        }
        gameObjectToSpawn = gameObjectToSpawn != null ? gameObjectToSpawn : gameObjects[0];
        Instantiate(gameObjectToSpawn, playerPosition.position, playerPosition.rotation);
    }
}
