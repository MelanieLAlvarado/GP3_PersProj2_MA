using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    Vector3 _followPosition;
    List<GameObject> _followObjects = new List<GameObject>();
    public void AddToFollowObjects(GameObject posToAdd) { _followObjects.Add(posToAdd); }
    public void RemoveFromFollowObjects(GameObject posToRemove) { _followObjects.Remove(posToRemove); }
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


        foreach (GameObject followObject in _followObjects)
        {
            center += followObject.transform.position;
        }
        if (_followObjects.Count > 0)
        { 
            center /= _followObjects.Count;
        }
        return center;
    }
}
