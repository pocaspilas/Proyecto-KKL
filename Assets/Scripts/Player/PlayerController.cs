using Mati36.Sound;
using Mati36.Utility;
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
    [SerializeField, ReadOnly]
    private int currentWaypoint;

    static public bool isMoving;

    public float maxSpeed;
    public float accel;

    public int initialWaypoint = 0;

    private float currentSpeed;

    [ReadOnly(true)]
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
    public SoundAsset bgMusic2, pushkeAppear, pushkeDisappear;
    private PoolableAudioSource currentMusic1, currentMusic2;

    [Header("Panel")]
    public Text panelText;
    public Animator panelAni;

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
        UnityEngine.XR.InputTracking.Recenter();
        pushke.Play("Pushke_Disappear", 0, 1f);
        transform.position = GetWaypoint(initialWaypoint);
        currentWaypoint = initialWaypoint;
        lastForward = (GetWaypoint(initialWaypoint + 1) - GetWaypoint(initialWaypoint)).normalized;
        nextForward = (GetWaypoint(initialWaypoint + 2) - GetWaypoint(initialWaypoint + 1)).normalized;
        transform.forward = lastForward;

        isMoving = false;
        timeGazing = 0;

        HidePanel(true);

        StartMusic();

        VRGaze.currentVRCamera.EnableReticle();

        FadeUtility.FadeFromBlack(2);

        //ShowPanel("Bienvenidos al Bosque KKL");
        //Utility.ExecuteAfterSeconds(4, delegate { HidePanel(); });
    }

    public void StartMusic()
    {
        if (bgMusic != null)
        {
            currentMusic1 = SoundManager.PlaySound(bgMusic);
            currentMusic1.FadeIn(1);
        }
        if (bgMusic2 != null)
        {
            currentMusic2 = SoundManager.PlaySound(bgMusic2);
            currentMusic2.FadeIn(1);
        }
    }

    public void StopMusic()
    {
        currentMusic1?.FadeOut(1);
        currentMusic1 = null;
        currentMusic2?.FadeOut(1);
        currentMusic2 = null;
    }

    private void OnDestroy()
    {
        StopMusic();
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
        //if (currentSpeed <= 0) { return; }
        if (currentWaypoint >= WaypointsRoute.splinePoints.Count - 1) return;
        MoveThroughPoints();
    }

    public void ToggleMove()
    {
        isMoving = !isMoving;
    }

    private void MoveThroughPoints()
    {
        //Debug.DrawRay(transform.position, transform.forward * 5f, Color.green);

        Vector3 current = GetWaypoint(currentWaypoint);
        Vector3 next = GetWaypoint(currentWaypoint + 1);

        transform.position = Vector3.Lerp(current, next, t);
        transform.forward = Vector3.Slerp(lastForward, nextForward, t);

        if (isMoving)
            currentSpeed += Time.deltaTime * accel;
        else
            currentSpeed -= Time.deltaTime * accel;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        if (currentSpeed <= 0) return;

        t += Mathf.Max(0f, (Time.deltaTime * (currentSpeed - gazeSlow * timeGazing)) / Vector3.Distance(current, next));

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
        if (currentPushkeRoutine != null)
            StopCoroutine(currentPushkeRoutine);
        else
            ShowPushke();

        StartCoroutine(PushkeRoutine(1.5f));

    }

    Coroutine currentPushkeRoutine;

    private IEnumerator PushkeRoutine(float secToShow)
    {
        yield return new WaitForSeconds(secToShow);
        HidePushke();
    }

    //PUSHKE
    public void ShowPushke()
    {
        if (pushkeAppear)
            SoundManager.PlayOneShotSound(pushkeAppear);
        pushke.Play("Pushke_Appear", 0, 0f);
        SetPushkeSpeed(1);
    }

    public void SetPushkeSpeed(float speed)
    {
        pushke.speed = speed;
    }

    public void HidePushke()
    {
        if (pushkeDisappear)
            SoundManager.PlayOneShotSound(pushkeDisappear);
        pushke.CrossFadeInFixedTime("Pushke_Disappear", 0.5f);
        //pushke.Play("Pushke_Disappear", 0, 0f);
    }

    //PANEL
    public void ShowPanel(string text)
    {
        panelAni.Play("Panel_Appear", 0, 0f);
        panelText.text = text;
    }

    public void HidePanel(bool immediatly = false)
    {
        panelAni.Play("Panel_Disappear", 0, immediatly ? 1f : 0f);
    }

    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.yellow;

    //    Vector2 current = GetWaypoint(currentWaypoint).XZ();
    //    Vector2 next = GetWaypoint(currentWaypoint + 1).XZ();

    //    Vector3 closestPoint;

    //    Vector2 dir = next - current;

    //    Gizmos.DrawRay(UseYAsZ(current), UseYAsZ(dir));

    //    Vector2 pDir = transform.position.XZ() - current;
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawRay(UseYAsZ(current), UseYAsZ(pDir));


    //    float dot = Vector2.Dot(dir, pDir);
    //    dot = dot / dir.magnitude;

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(UseYAsZ(current), dot);


    //    closestPoint = current + dir.normalized * dot;
    //    Gizmos.DrawWireSphere(UseYAsZ(closestPoint), 1f);
    //}

    //private Vector3 UseYAsZ(Vector2 vector)
    //{
    //    return new Vector3(vector.x, 3f, vector.y);
    //}
}
