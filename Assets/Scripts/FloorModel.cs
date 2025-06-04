using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorModel : MonoBehaviour
{
    SpawnModelOnFloor _floorSpawner;
	List<MeshRenderer> _renderers;

	[SerializeField] Vector2 _modelOffset;

	bool _spawned = false;
	Vector3 _fixedPosition;

	private void Awake()
	{
		_floorSpawner = FindAnyObjectByType<SpawnModelOnFloor>();
		_renderers = GetComponentsInChildren<MeshRenderer>(true).ToList();
		foreach (var _renderer in _renderers)
		{
			_renderer.enabled = false; // Initially disable all renderers
		}
	}

	private void Start()
	{
		Debug.Log($"FloorModel started, waiting for updates. Pos: {transform.position}");
	}

	private void Update()
	{
		if (_spawned)
		{
			transform.SetPositionAndRotation(_fixedPosition, Quaternion.Euler(0, 0, 0));
			return;
		}

		var pointFound = _floorSpawner.FloorFoundAtCoords(new Vector2(transform.position.x + _modelOffset.x, transform.position.z + _modelOffset.y), out var res);
		if (pointFound)
		{
			_spawned = true;
			foreach (var _renderer in _renderers)
			{
				_renderer.enabled = true;
			}
			_fixedPosition = res;
			Debug.Log($"FloorModel position updated to: {transform.position}");
		}
		else
		{
			foreach (var _renderer in _renderers)
			{
				_renderer.enabled = false;
			}
			Debug.Log("FloorModel position not updated, point not found on any plane.");
		}
	}
}
