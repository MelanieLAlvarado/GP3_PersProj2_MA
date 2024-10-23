using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    Vector3 _followPosition;
    List<GameObject> followObjects = new List<GameObject>();
    public void AddToFollowObjects(GameObject posToAdd) { followObjects.Add(posToAdd); }
    public void RemoveFromFollowObjects(GameObject posToRemove) { followObjects.Remove(posToRemove); }
    private void Awake()
    {
        _followPosition = transform.position;
    }

    private void Update()
    {
        _followPosition = CalculateCenter();
        this.transform.position = new Vector3(_followPosition.x, _followPosition.y + 2f, transform.position.z);
    }

    private Vector3 CalculateCenter() 
    {
        Vector3 center = Vector3.zero;


        foreach (GameObject followObject in followObjects)
        {
            center += followObject.transform.position;
        }
        if (followObjects.Count > 0)
        { 
            center /= followObjects.Count;
        }
        return center;
    }
}
