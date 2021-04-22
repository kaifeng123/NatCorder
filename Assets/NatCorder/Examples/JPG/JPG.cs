using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPG : MonoBehaviour
{
    [Header("GIF Settings")]
    public int imageWidth = 640;
    public int imageHeight = 480;

    private JPGRecorder recorder;
    private CameraInput cameraInput;
    public Camera cam;

    private bool isPhoto = false;

    public void StartRecording()
    {
        if (isPhoto == false) {
            isPhoto = true;
            // Start recording
            recorder = new JPGRecorder(imageWidth, imageHeight);
            cameraInput = new CameraInput(recorder, new RealtimeClock(), cam);
            cameraInput.frameSkip = 40;
            Debug.Log(" StartRecording ");
        }
    }

    public async void StopRecording()
    {
        isPhoto = false;

        // Stop the recording
        cameraInput.Dispose();
        var path = await recorder.FinishWriting();
        Debug.Log($"Saved animated jpg image to: {path}");
        var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
        Application.OpenURL($"{prefix}{path}");
    }
}
