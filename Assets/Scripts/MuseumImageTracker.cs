using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MuseumImageTracker : MonoBehaviour
{
	[Serializable]
	public class NameModelAssoc
	{
		public string Name;
		public FloorModel ModelPrefab;
		public StatueTextSO TextSO;
	}

	[SerializeField] ARTrackedImageManager _imageManager;
	[SerializeField] List<NameModelAssoc> _imageModelAssoc = new();

    void Start()
    {
        _imageManager.trackablesChanged.AddListener(TrackablesChanged);
	}

	private void TrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
	{
		foreach (var trackedImage in args.added)
		{
			Debug.Log($"Image added: {trackedImage.referenceImage.name} at position {trackedImage.transform.position}");
			var model = _imageModelAssoc.FirstOrDefault(a => a.Name == trackedImage.referenceImage.name);
			if (model != null && model.ModelPrefab != null)
			{
				var modelInstance = Instantiate(model.ModelPrefab, trackedImage.transform.position, Quaternion.identity);
				modelInstance.transform.SetParent(trackedImage.transform, true);
				modelInstance.Init(model.TextSO);
				Debug.Log($"Spawned model: {model.ModelPrefab.name} for image: {trackedImage.referenceImage.name}");
			}
			else
			{
				Debug.LogWarning($"No model associated with image: {trackedImage.referenceImage.name}");
			}
		}
		foreach (var trackedImage in args.updated)
		{
			Debug.Log($"Image updated: {trackedImage.referenceImage.name} at position {trackedImage.transform.position}");
		}
		foreach (var (trackedId, trackedImage) in args.removed)
		{
			Debug.Log($"Image removed: {trackedImage.referenceImage.name}");
		}
	}
}
