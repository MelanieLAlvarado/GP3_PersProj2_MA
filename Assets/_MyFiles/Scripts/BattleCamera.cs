using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    Vector3 _followPosition;
    List<GameObject> _followObjects = new List<GameObject>();
    [Header("Move Offset Settings")]
    [SerializeField] float yOffset = 2f;
    [SerializeField] float minDistance = 4f;
    [SerializeField] float maxDistance = 15f;

    [Header("Zoom Offset Settings")]
    [SerializeField] float xZoomOffset = 2f;
    [SerializeField] float yZoomThreshold = 2f;
    [SerializeField] float yZoomOffset = 1.5f;

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
            return this.transform.position;//doesn't move
        }

        center /= _followObjects.Count;
        //Debug.Log($"center: {center.x},{center.y},{center.z}");

        float zDist = CalculateZoomDistance(center);

        center = new Vector3(center.x, center.y + yOffset, zDist);
        //center = new Vector3(center.x, center.y + yOffset, transform.position.z);
        return center;
    }
    private float CalculateZoomDistance(Vector3 center) 
    {
        float dist = Vector3.Distance(_followObjects[0].transform.position, center);

        //Debug.Log("distance from center:" + dist);
        dist = Mathf.Abs(dist) + xZoomOffset;

        if (Mathf.Abs(center.y) > yZoomThreshold)//in case player jumped or fell too close to edge of screen.
        {
            dist += yZoomOffset;
        }

        dist = Mathf.Clamp(dist, minDistance, maxDistance);

        //center.x = Mathf.Clamp(center.x, -distFromOriginThreshold, distFromOriginThreshold);
        //clamping so screen doesn't go farther than needs to
        //   for when player goes offscreen to fall off(if killbox)

        if (cameraYDirection == 0)
        {
            cameraYDirection = -1;
        }
        return dist * cameraYDirection;
    }

}
