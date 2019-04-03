using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class WaypointsRoute : MonoBehaviour
{
    public List<Transform> waypointsList;
    public float splineResolution;
    static public List<Vector3> splinePoints;
    static public List<Vector3> wayPoints;



    private void Awake()
    {
        CalculateSpline();
    }

    public void CalculateSpline()
    {
        wayPoints = waypointsList.Select(x => x.position).ToList();
        splinePoints = CatmullRomSpline.GetCatmullRomPoints(ref waypointsList, splineResolution);
    }

#if UNITY_EDITOR
    public Color nodeColor = Color.white, pointColor;
    public float bigNodeSize, smallNodeSize;
    public bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (splinePoints == null)
            CalculateSpline();
        if (!drawGizmos) return;
        Gizmos.color = nodeColor;
        foreach (var wayPoint in wayPoints)
            Gizmos.DrawWireSphere(wayPoint, bigNodeSize);
        if (wayPoints.Count < 4) return;
        foreach (var splinePoint in splinePoints)
        {
            Gizmos.color = pointColor;
            Gizmos.DrawWireSphere(splinePoint, smallNodeSize);
        }
    }
#endif
}
