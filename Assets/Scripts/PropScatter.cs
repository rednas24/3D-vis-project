using UnityEngine;

public class RandomScatter : MonoBehaviour
{
    public GameObject[] prefabs;

    public int amount = 100;

    public Vector2 areaSize = new Vector2(20, 20);

    public float yPosition = 0f;

    public Vector2 randomScale = new Vector2(0.8f, 1.2f);

    [ContextMenu("Scatter Objects")]
    void ScatterObjects()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject prefab =
                prefabs[Random.Range(0, prefabs.Length)];

            Vector3 position = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                yPosition,
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            );

            position += transform.position;

            GameObject obj =
                Instantiate(prefab, position,
                Quaternion.Euler(
                    0,
                    Random.Range(0, 360),
                    0));

            float scale =
                Random.Range(randomScale.x,
                             randomScale.y);

            obj.transform.localScale *= scale;
        }
    }
}