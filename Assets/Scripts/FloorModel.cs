using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorModel : MonoBehaviour
{
    SpawnModelOnFloor _floorSpawner;
	List<MeshRenderer> _renderers;

	[SerializeField] Vector2 _modelOffset;
	[SerializeField] Dialog _dialog;

	bool _spawned = false;
	Vector3 _fixedPosition;
	Quaternion _fixedRotation;

	private void Awake()
	{
		_floorSpawner = FindAnyObjectByType<SpawnModelOnFloor>();
		_renderers = GetComponentsInChildren<MeshRenderer>(true).ToList();
		ToggleRenderers(false);
	}

	public void Init(StatueTextSO textSO, float yRot)
	{
		_dialog.Init(textSO);
		_fixedRotation = Quaternion.Euler(0, yRot, 0);
	}

	private void Start()
	{
		Debug.Log($"FloorModel started, waiting for updates. Pos: {transform.position}, rot: {transform.eulerAngles}");
	}

	void ToggleRenderers(bool enable)
	{
		foreach (var _renderer in _renderers)
		{
			_renderer.enabled = enable;
		}
		_dialog.gameObject.SetActive(enable);
	}

	void UpdatePositionAndRotation()
	{
		transform.SetPositionAndRotation(_fixedPosition, _fixedRotation);
		_dialog.LookAt(Camera.main.transform.position);
	}

	private void Update()
	{
		if (_spawned)
		{
			UpdatePositionAndRotation();
			return;
		}

		var localOffsetVector = _fixedRotation * new Vector3(_modelOffset.x, 0, _modelOffset.y);
		var finalWorldPosition = new Vector3(transform.position.x, 0, transform.position.z) + localOffsetVector;

		//looks for the point on the floor at the desired position
		var pointFound = _floorSpawner.FloorFoundAtCoords(new Vector2(finalWorldPosition.x, finalWorldPosition.z), out var res);
		if (pointFound)
		{
			_spawned = true;
			ToggleRenderers(true);
			_fixedPosition = res;
			UpdatePositionAndRotation();
			Debug.Log($"FloorModel position updated to: {transform.position}");
		}
		else
		{
			ToggleRenderers(false);
			Debug.Log("FloorModel position not updated, point not found on any plane.");
		}
	}
}
