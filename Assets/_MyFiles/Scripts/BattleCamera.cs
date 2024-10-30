using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    Vector3 _followPosition;
    List<GameObject> _followObjects = new List<GameObject>();
    [SerializeField] float yOffset = 2f;
    [SerializeField] float followRate = 2f;
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
        if (_followObjects.Count > 0)
        { 
            center /= _followObjects.Count;
        }

        center = new Vector3(center.x, center.y + yOffset, transform.position.z);

        return center;
    }
}
