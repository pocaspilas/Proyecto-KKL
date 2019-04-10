using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public GameObject treePrefab;
    public LayerMask terrainLayer;
    public float plantRadius;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            Plant();
    }

    public void Plant()
    {
        if (plantRoutine != null) return;
        plantRoutine = StartCoroutine(PlantRoutine());
    }

    Coroutine plantRoutine;

    IEnumerator PlantRoutine()
    {
        for (int i = 0; i < PlayerController.instance.itemsCollected; i++)
        {
            Vector2 rand = Random.insideUnitCircle * plantRadius;
            Vector3 plantPos = new Vector3(rand.x, 0,rand.y);
            plantPos += transform.position;
            RaycastHit hit;
            Ray r = new Ray(plantPos, Vector3.down);
            Debug.DrawRay(r.origin, r.direction * 10, Color.red, 3);
            Physics.Raycast(r, out hit, 10, terrainLayer);
            if (hit.collider != null)
            {
                Instantiate(treePrefab, hit.point, Quaternion.identity, transform);
                yield return new WaitForSeconds(1);
            }
            else
                Debug.Log("HIT NULL");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, plantRadius);
    }
}
