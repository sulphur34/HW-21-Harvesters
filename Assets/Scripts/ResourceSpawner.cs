using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ResourceSpawner : ObjectPool
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Collider _spawnZoneCollider;
    [SerializeField] private float _spawnDelay;

    protected Vector3 _minSpawnZoneBound;
    protected Vector3 _maxSpawnZoneBound;
    private float _offsetY = 2f;
    private WaitForSeconds _spawnSuspender;

    public UnityEvent ResourceCollected;

    private void Awake()
    {
        SetSpawnZoneBounds();
        _spawnSuspender = new WaitForSeconds(_spawnDelay);
        StartCoroutine(Begin());
    }

    private IEnumerator Begin()
    {
        Initialize(_resourcePrefab.gameObject);

        bool isContinue = true;

        while (isContinue)
        {
            if(TryGetObject(out GameObject resource))
            {
                Vector3 spawnPosition = GetRandomPositionOnSpawnZone();
                resource.transform.position = spawnPosition;
                resource.SetActive(true);
            }

            yield return _spawnSuspender;
        }
    }     

    private void SetSpawnZoneBounds()
    {
        _minSpawnZoneBound = _spawnZoneCollider.bounds.min;
        _maxSpawnZoneBound = _spawnZoneCollider.bounds.max;
    }

    private Vector3 GetRandomPositionOnSpawnZone()
    {
        float positionX = Random.Range(_minSpawnZoneBound.x, _maxSpawnZoneBound.x);
        float positionZ = Random.Range(_minSpawnZoneBound.z, _maxSpawnZoneBound.z);
        float positionY = _maxSpawnZoneBound.y + _offsetY;

        return new Vector3(positionX, positionY, positionZ);
    }
}
