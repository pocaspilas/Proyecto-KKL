using Mati36.Sound;
using Mati36.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;

    public GameObject eyeCamera;
    private int currentWaypoint;

    static public bool isMoving;
    public float speed;

    public int itemsCollected = 0;
    public Animator pushke;
    static public Vector3 PushkePosition { get { return instance.pushke.transform.position; } }


    [Header("Gaze Slow")]
    public float gazeSlow = 1;
    public float gazeSlowTime;
    public float gazeSpeedTime;
    private float timeGazing = 0;

    [Header("Sounds")]
    public SoundAsset bgMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        pushke.Play("Pushke_Disappear", 0, 1f);
        transform.position = GetWaypoint(0);
        currentWaypoint = 0;
        lastForward = (GetWaypoint(1) - GetWaypoint(0)).normalized;
        nextForward = (GetWaypoint(2) - GetWaypoint(1)).normalized;
        transform.forward = lastForward;

        isMoving = false;
        timeGazing = 0;

        SoundManager.PlaySound(bgMusic, true);
    }

    Vector3 GetWaypoint(int index)
    {
        return WaypointsRoute.splinePoints[index];
    }

    private float t;
    private Vector3 lastForward, nextForward;

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(5);
        isMoving = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ToggleMove();

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

        if (VRGaze.currentVRCamera.IsGazingSomething)
            timeGazing += Time.deltaTime / gazeSlowTime;
        else
            timeGazing -= Time.deltaTime / gazeSpeedTime;
        timeGazing = Mathf.Clamp01(timeGazing);
    }

    private void LateUpdate()
    {
        if (!isMoving) return;
        if (currentWaypoint >= WaypointsRoute.splinePoints.Count - 1) return;
        MoveThroughPoints();
    }

    public void ToggleMove()
    {
        isMoving = !isMoving;
    }

    private void MoveThroughPoints()
    {
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.green);

        Vector3 current = GetWaypoint(currentWaypoint);
        Vector3 next = GetWaypoint(currentWaypoint + 1);

        transform.position = Vector3.Lerp(current, next, t);
        transform.forward = Vector3.Slerp(lastForward, nextForward, t);
        t += (Time.deltaTime * (speed - gazeSlow * timeGazing)) / Vector3.Distance(current, next);

        if (t > 1)
        {
            currentWaypoint++;
            transform.position = GetWaypoint(currentWaypoint);
            lastForward = nextForward;
            t = 0;
            if (currentWaypoint < WaypointsRoute.splinePoints.Count - 1)
                nextForward = (GetWaypoint(currentWaypoint + 1) - GetWaypoint(currentWaypoint)).normalized;
        }
    }

    public void OnItemCollect()
    {
        itemsCollected++;
        pushke.Play("Pushke_Appear", 0, 0f);
    }
}
