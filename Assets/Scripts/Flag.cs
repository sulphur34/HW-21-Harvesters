using DG.Tweening;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class Flag : MonoBehaviour
{
    private Vector3 _rotationVector;
    private float _duration;
    private float _flagOffsetPosition = 3f;

    private void Start()
    {
        _rotationVector = new Vector3(0, -40, 0);
        _duration = 1;
        transform.DORotate(_rotationVector, _duration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Flash);
    }

    public void ActivateFlag(Vector3 position)
    {
        this.enabled = true;
        gameObject.SetActive(true);
        MoveFlag(position);
    }

    public void MoveFlag(Vector3 position)
    {
        Vector3 flagPosition = new Vector3(position.x, position.y + _flagOffsetPosition, position.z);
        transform.position = flagPosition;
    }
}
