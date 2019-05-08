using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedDeer : MonoBehaviour
{
    public Transform initialPos, eatPos, finalPos;

    public float appearRange, runRange;
    public float walkSpeed, runSpeed, trotSpeed;
    private Animator ani;

    private Camera currentCam;

    public LayerMask terrainLayer;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        currentCam = Camera.main;
    }

    void Start()
    {
        transform.position = initialPos.position;
        ani.Play("idle", 0, 0f);
        StartCoroutine(AppearRoutine());
    }

        RaycastHit info;
    void UpdateHeight()
    {
        Physics.Raycast(transform.position + Vector3.up * 3f, Vector3.down, out info, 10f, terrainLayer);
        if(info.collider != null)
        {
            transform.position = new Vector3(transform.position.x, info.point.y, transform.position.z);
        }
    }

    IEnumerator AppearRoutine()
    {
        while (Vector3.Distance(currentCam.transform.position, initialPos.position) > appearRange)
            yield return null;

        float distToTarget = Vector3.Distance(initialPos.position, eatPos.position);
        ani.Play("walking", 0, 0f);

        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(initialPos.position, eatPos.position, t);
            transform.forward = (eatPos.position - transform.position).normalized;
            t += walkSpeed * Time.deltaTime / distToTarget;

            UpdateHeight();
            yield return null;
        }
        transform.position = eatPos.position;

        StartCoroutine(EatRoutine());
    }

    IEnumerator EatRoutine()
    {
        ani.CrossFade("eating", 0.8f, 0, 0f);

        UpdateHeight();

        while (Vector3.Distance(currentCam.transform.position, eatPos.position) > runRange)
            yield return null;

        UpdateHeight();
        ani.CrossFade("idle", 0.3f, 0, 0.4f);
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(FleeRoutine());
    }

    IEnumerator FleeRoutine()
    {
        float distToTarget = Vector3.Distance(eatPos.position, finalPos.position);

        float t = 0;
        //ani.CrossFade("walking", 0.1f, 0, 0f);

        //while (t < 0.05f)
        //{
        //    transform.position = Vector3.Lerp(eatPos.position, finalPos.position, t);
        //    transform.forward = (finalPos.position - transform.position).normalized;


        //    t += walkSpeed * Time.deltaTime / distToTarget;
        //    yield return null;
        //}

        //ani.Play("trotting", 0, 0f);
        ani.CrossFade("trotting", 0.01f, 0, 0f);

        float currentSpeed = 0f;

        while (t < 0.2f)
        {
            transform.position = Vector3.Lerp(eatPos.position, finalPos.position, t);
            transform.forward = (finalPos.position - transform.position).normalized.SetY(0);
            
            currentSpeed = Mathf.Lerp(walkSpeed + 5, trotSpeed, t / 0.2f);
            ani.speed = Mathf.Clamp01((currentSpeed / trotSpeed) + 0.2f);
            t += (currentSpeed) * Time.deltaTime / distToTarget;
            UpdateHeight();
            yield return null;
        }
        ani.speed = 1f;

        ani.CrossFade("galloping", 0.3f, 0, 0f);

        while (t < 1)
        {
            transform.position = Vector3.Lerp(eatPos.position, finalPos.position, t);
            transform.forward = (finalPos.position - transform.position).normalized.SetY(0);
            t += runSpeed * Time.deltaTime / distToTarget;
            UpdateHeight();
            yield return null;
        }
        transform.position = finalPos.position;
        transform.forward = finalPos.position - eatPos.position;
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        if (initialPos == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(initialPos.position, 2f);
        Gizmos.DrawWireSphere(initialPos.position, appearRange);

        if (eatPos == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eatPos.position, 2f);
        Gizmos.DrawWireSphere(eatPos.position, runRange);

        if (finalPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(finalPos.position, 2f);
    }
}
