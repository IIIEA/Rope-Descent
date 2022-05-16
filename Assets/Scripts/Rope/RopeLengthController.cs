using UnityEngine;
using Obi;

public class RopeLengthController : MonoBehaviour
{
	[SerializeField] private YMovement _yMovement;
	[SerializeField] private ObiRopeCursor _cursor;
	[SerializeField] private ObiRope _rope;

	private float _speed = 1f;

	void Update()
	{
		_speed = Remap.DoRemap(0, 25, 0, 10, _yMovement.VelocityY);
		_cursor.ChangeLength(_rope.restLength + _speed * Time.deltaTime);
	}
}