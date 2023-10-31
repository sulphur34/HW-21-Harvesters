using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Harvester _harvesterPrefab;
    [SerializeField] private Flag _buildFlag;
    [SerializeField] private ParticleSystem _selectedParticleSystem;
    [SerializeField] private float _resourceValue;

    private Queue<Harvester> _freeHarvesters;
    private float _buildHarvesterCost = 3;
    private float _buildBaseCost = 5;
    private Transform _transform;
    private float _flagOffsetPosition = 3f;
    private Coroutine _operateCoroutine;
    private Coroutine _collectResourceCoroutine;
    private Coroutine _buildBaseCoroutine;

    public bool IsFlagActivated => _buildFlag.gameObject.activeSelf;

    public UnityAction BaseSelected;
    public UnityAction BaseDeselected;

    private void Awake()
    {
        _transform = transform;
        _freeHarvesters = new Queue<Harvester>();
        StartCoroutine(Operate());
        BaseSelected += OnBaseSelected;
        BaseDeselected += OnBaseDeselected;
    }

    private void OnDisable()
    {
        StopCoroutine(_buildBaseCoroutine);
        StopCoroutine(_operateCoroutine);
        StopCoroutine(_collectResourceCoroutine);
    }

    public void AddResource(Resource resource)
    {
        Harvester harvester = resource.GetComponentInParent<Harvester>();
        _resourceValue += resource.ResourceValue;
        resource.Deactivate();        
        _freeHarvesters.Enqueue(harvester);        
    }

    public void AttachHarvester(Harvester harvester)
    {
        harvester.SetOwner(this);
        _freeHarvesters.Enqueue(harvester);
    }

    public void ActivateFlag(Vector3 position)
    {
        _buildFlag.enabled = true;
        _buildFlag.gameObject.SetActive(true);
        MoveFlag(position);
    }

    public void MoveFlag(Vector3 position)
    {
        Vector3 flagPosition = new Vector3(position.x, position.y + _flagOffsetPosition, position.z);
        _buildFlag.transform.position = flagPosition;
    }

    private void OnBaseSelected()
    {
        _selectedParticleSystem.Play();
    }

    private void OnBaseDeselected()
    {
        _selectedParticleSystem.Stop();
    }

    private IEnumerator Operate()
    {
        bool isContinue = true;

        while (isContinue)
        {
            if (_buildFlag.enabled == false)
            {
                if (_resourceValue >= _buildHarvesterCost)
                    BuildNewHarvester();                
            }
            else if (_resourceValue >= _buildBaseCost)
            {
                InitiateNewBaseBuilding();
            }

            if (TryGetResources(out Resource[] resources))
                CollectResources(resources);

            yield return null;
        }
    }

    private void CollectResources(Resource[] resources)
    {
        int resourceIndex = 0;

        while (_freeHarvesters.Count > 0 && resourceIndex < resources.Length)
        {
            Harvester harvester = _freeHarvesters.Dequeue();
            resources[resourceIndex].SetOwner(this);
            StartCoroutine(harvester.CollectResource(resources[resourceIndex]));
            resourceIndex++;
        }
    }

    private void BuildNewHarvester()
    {
        _resourceValue -= _buildHarvesterCost;
        Harvester harvester = Instantiate(_harvesterPrefab);
        harvester.transform.position = _transform.position;
        harvester.SetOwner(this);
        _freeHarvesters.Enqueue(harvester);
    }

    private void InitiateNewBaseBuilding()
    {
        _resourceValue -= _buildBaseCost;

        if (_freeHarvesters.Count > 0)
        {
            Harvester harvester = _freeHarvesters.Dequeue();
            StartCoroutine(harvester.BuildBase(_buildFlag));
            _buildFlag.enabled = false;
        }
    }

    private bool TryGetResources(out Resource[] resources)
    {
        IEnumerable<Resource> resourcesFound = FindObjectsOfType<Resource>()
            .Where(resource => resource.Owner == null);

        if (resourcesFound.Count() == 0)
        {
            resources = new Resource[0];
            return false;
        }
        else
        {
            resourcesFound.OrderBy(resource => Vector3.Distance(_transform.position, resource.transform.position));
            resources = resourcesFound.ToArray();
            return true;
        }
    }
}
