using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float _resourceValue;

    private ResourceSpawner _spawner;
    private Transform _transform;
    private Base _owner;

    public float ResourceValue => _resourceValue;
    public Base Owner => _owner;

    private void Awake()
    {
        _spawner = GetComponentInParent<ResourceSpawner>();
        _transform = transform;
    }

    public void Deactivate()
    {
        _owner = null;
        _transform.SetParent(_spawner.transform, false);
        gameObject.SetActive(false);
    }

    public void SetOwner(Base owner)
    {
        _owner = owner;
    }
}
