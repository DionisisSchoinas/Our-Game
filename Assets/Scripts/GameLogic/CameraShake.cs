using UnityEngine;
using System.Collections;
using System;

public class CameraShake : MonoBehaviour
{
	public static CameraShake current;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Transform camTransform;
	Vector3 originalPos;

	void Awake()
	{
		camTransform = Camera.main.transform;

		current = this;
		current.onCameraShake += Shake;
	}

    private void OnDestroy()
	{
		current.onCameraShake -= Shake;
	}


    //------------- Events -----------------
    public Action<float, float> onCameraShake;
	public void ShakeCamera(float duration, float amount)
	{
		if (onCameraShake != null)
		{
			onCameraShake(duration, amount);
		}
	}

	private void Shake(float duration, float amount)
	{
		originalPos = camTransform.localPosition;
		shakeDuration = duration;
		shakeAmount = amount;
	}

	void Update()
	{
		if (Time.timeScale != 0)
		{
			if (shakeDuration > 0)
			{
				camTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;

			}
		}
	}
}