using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnModelOnFloor : MonoBehaviour
{
    [SerializeField] ARPlaneManager _planeManager;

    void Start()
    {
        _planeManager.trackablesChanged.AddListener(FloorDetectionEvent);
    }

    public void FloorDetectionEvent(ARTrackablesChangedEventArgs<ARPlane> trackablesChanged)
    {
      //  Debug.Log(trackablesChanged.updated.Count + " planes updated, " +
				  //trackablesChanged.added.Count + " planes added, " +
				  //trackablesChanged.removed.Count + " planes removed.");
    }

	public bool FloorFoundAtCoords(Vector2 worldCoordsPoint, out Vector3 worldOutputPoint)
	{
		foreach (var plane in _planeManager.trackables)
		{
			var ray = new Ray(new Vector3(worldCoordsPoint.x, 100, worldCoordsPoint.y), Vector3.down);

			if(plane.GetComponent<MeshCollider>().Raycast(ray, out var hit, Mathf.Infinity))
			{
				worldOutputPoint = hit.point;
				Debug.Log($"Point found on plane: {plane.trackableId} at {worldOutputPoint} using raycast.");
				return true;
			}
		}

		Debug.Log("Point not found on any plane.");
		worldOutputPoint = Vector3.zero;
		return false;
	}
}
