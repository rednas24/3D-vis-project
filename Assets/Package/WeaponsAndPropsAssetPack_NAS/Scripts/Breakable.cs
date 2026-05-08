using System.Collections;
using UnityEngine;

namespace WeaponsAndPropsAssetPack_NAS.Scripts
{
    public class Breakable : MonoBehaviour
    {
        [SerializeField] private Transform wholeObject;
        [SerializeField] private Transform fracturedObject;

        // NEW
        [SerializeField] private GameObject keyPrefab;
        [SerializeField] private Transform keySpawnPoint;

        private bool isBroken;
        private Transform fracturedObjectInstance;

        private const float timeToCleanUp = 5f;

        public void TriggerBreak()
        {
            if (isBroken) return;

            BreakObject();
        }

        private void BreakObject()
        {
            wholeObject.gameObject.SetActive(false);

            Collider col = GetComponent<Collider>();

            if (col != null)
            {
                col.enabled = false;
            }

            fracturedObjectInstance = Instantiate(
                fracturedObject,
                wholeObject.position,
                wholeObject.rotation
            );

            fracturedObjectInstance.gameObject.SetActive(true);

            // Spawn key
            if (keyPrefab != null)
            {
                Vector3 spawnPosition = transform.position;

                if (keySpawnPoint != null)
                {
                    spawnPosition = keySpawnPoint.position;
                }

                Instantiate(
                    keyPrefab,
                    spawnPosition,
                    Quaternion.identity
                );
            }

            isBroken = true;

            StartCoroutine(CleanUpCoroutine());
        }

        private IEnumerator CleanUpCoroutine()
        {
            yield return new WaitForSeconds(timeToCleanUp);

            if (fracturedObjectInstance != null)
            {
                Destroy(fracturedObjectInstance.gameObject);
            }
        }
    }
}