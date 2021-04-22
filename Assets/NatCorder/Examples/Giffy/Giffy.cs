/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;

    public class Giffy : MonoBehaviour {
        
        [Header("GIF Settings")]
        public int imageWidth = 640;
        public int imageHeight = 480;
        public float frameDuration = 0.1f; // seconds

        private GIFRecorder recorder;
        private CameraInput cameraInput;

        public void StartRecording () {
            // Start recording
            recorder = new GIFRecorder(imageWidth, imageHeight, frameDuration);
            cameraInput = new CameraInput(recorder, new RealtimeClock(), Camera.main);
            // Get a real GIF look by skipping frames
            cameraInput.frameSkip = 4;
        }

        public async void StopRecording () {
            // Stop the recording
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            Debug.Log($"Saved animated GIF image to: {path}");
            var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
            Application.OpenURL($"{prefix}{path}");
        }
    }
}