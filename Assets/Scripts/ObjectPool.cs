using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int _capacity = 5;
        
    private List<GameObject> _pool;
    private Transform _transform;

    protected void Reset()
    {
        foreach (var item in _pool)
        {
            item.SetActive(false);
        }
    }

    protected void Initialize(GameObject prefab)
    {
        _pool = new List<GameObject>();
        _transform = transform;

        for (int i = 0; i < _capacity; i++)
        {
            GameObject spawned = Instantiate(prefab, _transform);
            spawned.SetActive(false);
            _pool.Add(spawned);
        }
    }    

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(gameObject => gameObject.activeSelf == false);
        return result != null;
    }    
}
