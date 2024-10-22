using UnityEngine;

public class Widget : MonoBehaviour
{
    private GameObject _owner;

    public GameObject GetOwner() { return _owner; }
    public void SetOwner(GameObject owner) { _owner = owner; }
}
