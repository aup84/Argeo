/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using UnityEngine.UI;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class TrackableEventHandlerArena : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
	bool bas = false;
    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus){
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED ||
		          newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " found");

			OnTrackingFound ();
			if (mTrackableBehaviour.TrackableName.Equals ("Base")) {
				if (!bas) {
					Debug.Log ("Mostrando el Contenedor de Arena");
					bas = true;
					GameObject.Find ("Arenero").GetComponentInChildren<MeshRenderer> ().enabled = true;
					GameObject.Find ("Arenero").GetComponentInChildren<Transform> ().localScale = new Vector3 (2f,2f,3f);
					GameObject.Find("txtInstrucciones").GetComponent<Text>().text = "Selecciona la medida del Recipiente Auxiliar (Cubo) ";
					GameObject.Find ("pnlSize").GetComponent<RectTransform> ().localScale = new Vector3 (0.25f, 0.30f, 0.1f);
					GameObject.Find ("Dropdown").GetComponent<RectTransform> ().localScale = new Vector3 (8f, 10f, 2f);
					GameObject.Find ("lblDrop").GetComponent<RectTransform> ().localScale = new Vector3 (2f, 10f, 10f);
				} 
				else if (mTrackableBehaviour.TrackableName.Equals ("Cubo")) {
					Debug.Log ("Tag:  " + GameObject.Find ("CuboArena").tag);
				}
			} 
			else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NO_POSE) {
				Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " lost");
				OnTrackingLost ();
			}
		}
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PRIVATE_METHODS
}
