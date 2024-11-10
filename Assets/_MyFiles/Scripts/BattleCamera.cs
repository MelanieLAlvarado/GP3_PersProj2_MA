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

    [Header("Follow Settings")]
    [SerializeField] float followRate = 2f;
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
            return this.transform.position; //Camera doesn't move
        }

        center /= _followObjects.Count;  //getting average position of GameObjects

        float yDist = Mathf.Abs(_followObjects[0].transform.position.y - center.y);
        float zDist = CalculateZoomDistance(center, yDist);

        float yOffset = yBaseOffset;
        yOffset = yOffset - (0.8f * yDist); //moves camera up so UI doesn't cover characters

        zDist *= cameraYDirection;//+z direction or -z direction (since this became abs() in code)
        center = new Vector3(center.x, center.y + yOffset, zDist);
        return center;
    }
    private float CalculateZoomDistance(Vector3 center, float yDist) 
    {
        float zoomOffset = xZoomOffset;  //characters won't off-screen indirection x
        if (yDist > 0.5f)
        {
            zoomOffset = 1.75f * yDist * xZoomOffset; //characters won't off-screen indirection y
        }


        float dist = Vector3.Distance(_followObjects[0].transform.position, center);
        dist = Mathf.Abs(dist) + zoomOffset; //applying any zoom to distance

        dist = Mathf.Clamp(dist, minDistance, maxDistance); //clamping zoom

        if (cameraYDirection == 0)
        {
            cameraYDirection = -1;//+z direction or -z direction
        }
        return dist;
    }

}
