using UnityEngine;

public abstract class Widget : MonoBehaviour
{
    protected GameObject _owner;

    public GameObject GetOwner() { return _owner; }
    public virtual void SetOwner(GameObject owner) { _owner = owner; }
}
