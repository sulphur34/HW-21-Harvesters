using System.Collections;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Base _basePrefab;

    private Transform _transform;
    private Base _owner;
    private float _baseBuildOffset = 3f;

    private void Awake()
    {
        _transform = transform;
    }
        
    public IEnumerator CollectResource(Resource resource)
    {
        Transform target = resource.transform;

        yield return MoveToTarget(target);

        target.SetParent(_transform);
        target.localPosition = new Vector3(0,1,0);

        yield return MoveToTarget(_owner.transform);

        _owner.AddResource(resource);
         
    }

    public IEnumerator BuildBase(Flag flag)
    {
        yield return MoveToTarget(flag.transform);

        Vector3 buildPlace = new Vector3(_transform.position.x, _baseBuildOffset, _transform.position.z);
        Base newBase = Instantiate(_basePrefab, buildPlace, _basePrefab.transform.rotation);

        newBase.AttachHarvester(this);
    }

    public void SetOwner(Base owner)
    {
        _owner = owner;
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        float minDistanceTolerance = 1f;        

        while (Vector3.Distance(target.position, _transform.position) > minDistanceTolerance)
        {
            _transform.rotation = Quaternion.LookRotation(target.position - _transform.position);
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
