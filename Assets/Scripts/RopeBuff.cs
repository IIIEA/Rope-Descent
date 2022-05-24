using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class RopeBuff : MonoBehaviour, ICollectable
{
    private void OnDestroy()
    {
        DOTween.Clear();
    }

    public void Collect(Vector3 point)
    {
        Destroy(gameObject, 0.3f);
        transform.DOScale(0, 0.5f);
        transform.DOMove(point, 0.1f);
    }
}
