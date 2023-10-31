using DG.Tweening;
using UnityEngine;

public class Flag : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(new Vector3(0, -40, 0), 1).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Flash);
    }    
}
