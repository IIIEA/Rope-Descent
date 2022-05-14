using UnityEngine;
using Obi;

public class RopeLengthController : MonoBehaviour
{
	[SerializeField] private ObiRopeCursor _cursor;
	[SerializeField] private ObiRope _rope;

	private float _speed = 1f;

	void Update()
	{
		if (Input.GetKey(KeyCode.W))
			_cursor.ChangeLength(_rope.restLength - _speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.S))
			_cursor.ChangeLength(_rope.restLength + _speed * Time.deltaTime);
	}
}