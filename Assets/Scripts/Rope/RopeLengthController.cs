using UnityEngine;
using Obi;

public class RopeLengthController : MonoBehaviour
{
	[SerializeField] private DistanceChecker _distanceChecker;
	[SerializeField] private YMovement _yMovement;
	[SerializeField] private ObiRopeCursor _cursor;
	[SerializeField] private ObiRope _rope;

	private float _speed = 0f;
	private float _length;

    private void Start()
    {
		_length = _rope.restLength;
    }

    private void OnEnable()
    {
		_distanceChecker.DistanceChanged += OnDistanceChanged;
    }

    private void OnDisable()
    {
		_distanceChecker.DistanceChanged -= OnDistanceChanged;
	}

	private void OnDistanceChanged(float distance, float maxDistance)
    {
		if (_rope.restLength <= distance)
		{
			_cursor.ChangeLength(distance);
		}
	}
}