using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class RopeBuff : MonoBehaviour
{
    private void OnDestroy()
    {
        DOTween.Clear();
    }

    public void Collect(Vector3 point)
    {
        Destroy(gameObject, 0.3f);
        transform.DOScale(0, 1f);
        transform.DOMove(point, 0.3f);
    }
}
