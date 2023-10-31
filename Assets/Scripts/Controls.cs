using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _pointingOnLayer;
    [SerializeField] private LayerMask _selectableLayer;

    private Base _selectedBase;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out RaycastHit selectHit, Mathf.Infinity, _selectableLayer))
            {
                _selectedBase?.BaseDeselected.Invoke();
                _selectedBase = selectHit.collider.gameObject.GetComponent<Base>();
                _selectedBase.BaseSelected.Invoke();
            }
            else if (_selectedBase != null && Physics.Raycast(ray, out RaycastHit pointHit, Mathf.Infinity, _pointingOnLayer))
            {
                if (_selectedBase.IsFlagActivated)
                    _selectedBase.MoveFlag(pointHit.point);
                else
                    _selectedBase.ActivateFlag(pointHit.point);
            }
            else
            {
                if (_selectedBase != null)
                {
                    _selectedBase.BaseDeselected.Invoke();
                    _selectedBase = null;
                }
            }
        }
    }
}