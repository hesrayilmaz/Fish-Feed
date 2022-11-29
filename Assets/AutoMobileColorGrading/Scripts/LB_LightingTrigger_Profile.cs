
// Blend two profiles based on camera entered into  trigger

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LB_LightingTrigger_Profile : MonoBehaviour {

	AutoMobileColorGrading current;
	AutoMobileColorGrading temp;

	[Tooltip("target profile for blending from current to it - Target")]
	public AutoMobileColorGrading targetProfile;
	[Tooltip("Target camera tag for trigger enter and exit")]
	public string cameraTag = "MainCamera";
	[Tooltip("Blend lerp speed * Time.deltaTime")]
	public float blendSpeed = 10f;
	[Tooltip("Update time duration. used for optimization")]
	public float blendDuration = 3f;

	void Start () {
		current = GameObject.FindGameObjectWithTag(cameraTag).GetComponent<AutoMobileColorGrading> ();
		temp = new AutoMobileColorGrading();
		temp.useAutoExposureMode = current.useAutoExposureMode;
		temp.middleGrey = current.middleGrey;
		temp.white = current.white;
		temp.adaptionSpeed = current.adaptionSpeed;
		temp.OveralExposure = current.OveralExposure;
		temp.Contrast = current.Contrast;
		temp.Saturation = current.Saturation;
		temp.Gamma = current.Gamma;
		temp.vignetteIntensity = current.vignetteIntensity;
		temp.R = current.R;
		temp.G = current.G;
		temp.B = current.B;
	}
	
	bool isChanging;
	bool isUpdating;

	void Update () {
		if (!isUpdating)
			return;
		
		if (isChanging) {
			current.useAutoExposureMode = targetProfile.useAutoExposureMode;
			current.middleGrey = Mathf.Lerp(current.middleGrey, targetProfile.middleGrey, Time.deltaTime * blendSpeed);
			current.white = Mathf.Lerp(current.white, targetProfile.white, Time.deltaTime * blendSpeed);
			current.adaptionSpeed = Mathf.Lerp(current.adaptionSpeed, targetProfile.adaptionSpeed, Time.deltaTime * blendSpeed);
			current.OveralExposure = Mathf.Lerp (current.OveralExposure, targetProfile.OveralExposure, Time.deltaTime * blendSpeed);
			current.Contrast = Mathf.Lerp (current.Contrast, targetProfile.Contrast, Time.deltaTime * blendSpeed);
			current.Gamma = Mathf.Lerp (current.Gamma, targetProfile.Gamma, Time.deltaTime * blendSpeed);
			current.Saturation = Mathf.Lerp (current.Saturation, targetProfile.Saturation, Time.deltaTime * blendSpeed);
			current.vignetteIntensity = Mathf.Lerp (current.vignetteIntensity, targetProfile.vignetteIntensity, Time.deltaTime * blendSpeed);
			current.R = Mathf.Lerp (current.R, targetProfile.R, Time.deltaTime * blendSpeed);
			current.G = Mathf.Lerp (current.G, targetProfile.G, Time.deltaTime * blendSpeed);
			current.B = Mathf.Lerp (current.B, targetProfile.B, Time.deltaTime * blendSpeed);
		} else {
			current.useAutoExposureMode = temp.useAutoExposureMode;
			current.middleGrey = Mathf.Lerp(current.middleGrey, temp.middleGrey, Time.deltaTime * blendSpeed);
			current.white = Mathf.Lerp(current.white, temp.white, Time.deltaTime * blendSpeed);
			current.adaptionSpeed = Mathf.Lerp(current.adaptionSpeed, temp.adaptionSpeed, Time.deltaTime * blendSpeed);
			current.OveralExposure = Mathf.Lerp (current.OveralExposure, temp.OveralExposure, Time.deltaTime * blendSpeed);
			current.Contrast = Mathf.Lerp (current.Contrast, temp.Contrast, Time.deltaTime * blendSpeed);
			current.Gamma = Mathf.Lerp (current.Gamma, temp.Gamma, Time.deltaTime * blendSpeed);
			current.Saturation = Mathf.Lerp (current.Saturation, temp.Saturation, Time.deltaTime * blendSpeed);
			current.vignetteIntensity = Mathf.Lerp (current.vignetteIntensity, temp.vignetteIntensity, Time.deltaTime * blendSpeed);
			current.R = Mathf.Lerp (current.R, temp.R, Time.deltaTime * blendSpeed);
			current.G = Mathf.Lerp (current.G, temp.G, Time.deltaTime * blendSpeed);
			current.B = Mathf.Lerp (current.B, temp.B, Time.deltaTime * blendSpeed);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == cameraTag){
			StopCoroutine ("StopUpdating");
			StartCoroutine ("StopUpdating");
			isChanging = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == cameraTag)
		{
			StopCoroutine ("StopUpdating");
			StartCoroutine ("StopUpdating");
			isChanging = false;
		}
	}

	// Stop update function after passing blennd duration in seconds    
	IEnumerator StopUpdating()
	{
		isUpdating = true;
		yield return new WaitForSeconds (blendDuration);
		isUpdating = false;
	}
}