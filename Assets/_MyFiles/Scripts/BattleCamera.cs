using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    Vector3 _followPosition; [SerializeField]
    List<GameObject> _followObjects = new List<GameObject>();
    [Header("Move Offset Settings")]
    [SerializeField] float yBaseOffset = 1.5f;
    [SerializeField] float minDistance = 4f;
    [SerializeField] float maxDistance = 20f;

    [Header("Zoom Offset Settings")]
    [SerializeField] float xZoomOffset = 2f;
    [SerializeField] float yCenterDistanceThreshold = 2f;

    [Header("Follow Settings")]
    [SerializeField] float followRate = 2f;
    [SerializeField] float distFromOriginThreshold = 10f;//may remove or work on further
    [SerializeField] int cameraYDirection = -1;
    public void AddToFollowObjects(GameObject posToAdd) { _followObjects.Add(posToAdd); }
    public void RemoveFromFollowObjects(GameObject posToRemove) { _followObjects.Remove(posToRemove); }
    private void Awake()
    {
        _followPosition = transform.position;
    }

    private void Update()
    {
        _followPosition = CalculateCenter();

        this.transform.position = Vector3.Lerp(this.transform.position, _followPosition, Time.deltaTime * followRate);
    }

    private Vector3 CalculateCenter()
    {
        Vector3 center = Vector3.zero;

        foreach (GameObject followObject in _followObjects)
        {
            center += followObject.transform.position;
        }
        if (_followObjects.Count <= 0)
        {
            return this.transform.position;//Camera doesn't move
        }

        center /= _followObjects.Count;
        //Debug.Log($"center: {center.x},{center.y},{center.z}");

        float yDist = Mathf.Abs(_followObjects[0].transform.position.y - center.y);
        float zDist = CalculateZoomDistance(center, yDist);

        float yOffset = yBaseOffset;
        yOffset = yOffset - (0.8f * yDist);

        zDist *= cameraYDirection;
        center = new Vector3(center.x, center.y + yOffset, zDist);
        return center;
    }
    private float CalculateZoomDistance(Vector3 center, float yDist) 
    {
        float zoomOffset = xZoomOffset;
        if (yDist > 0.5f)
        {
            zoomOffset = 1.75f * yDist * xZoomOffset;
        }


        float dist = Vector3.Distance(_followObjects[0].transform.position, center);
        dist = Mathf.Abs(dist) + zoomOffset;

        dist = Mathf.Clamp(dist, minDistance, maxDistance);

        if (cameraYDirection == 0)
        {
            cameraYDirection = -1;
        }
        return dist;
    }

}
