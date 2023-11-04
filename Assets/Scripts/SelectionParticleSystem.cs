using UnityEngine;
using UnityEngine.Events;

public class SelectionParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _selectedParticleSystem;

    public UnityAction BaseSelected;
    public UnityAction BaseDeselected;

    private void Awake()
    {
        BaseSelected += OnBaseSelected;
        BaseDeselected += OnBaseDeselected;
    }

    private void OnBaseSelected()
    {
        _selectedParticleSystem.Play();
    }

    private void OnBaseDeselected()
    {
        _selectedParticleSystem.Stop();
    }
}
