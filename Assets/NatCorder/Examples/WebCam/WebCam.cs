/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;

    public class WebCam : MonoBehaviour {

        public RawImage rawImage;
        public AspectRatioFitter aspectFitter;

        private WebCamTexture webCamTexture;
        private MP4Recorder recorder;
        private WebCamTextureInput webCamTextureInput;

        private IEnumerator Start () {
            // Request camera permission
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
                yield break;
            // Start the WebCamTexture
            webCamTexture = new WebCamTexture(1280, 720, 30);
            webCamTexture.Play();
            // Display webcam
            yield return new WaitUntil(() => webCamTexture.width != 16 && webCamTexture.height != 16); // Workaround for weird bug on macOS
            rawImage.texture = webCamTexture;
            aspectFitter.aspectRatio = (float)webCamTexture.width / webCamTexture.height;
        }

        public void StartRecording () {
            // Start recording
            var clock = new RealtimeClock();
            recorder = new MP4Recorder(webCamTexture.width, webCamTexture.height, 30);
            webCamTextureInput = new WebCamTextureInput(recorder, clock, webCamTexture);
        }

        public async void StopRecording () {
            // Stop recording
            webCamTextureInput.Dispose();
            var path = await recorder.FinishWriting();
            // Playback recording
            Debug.Log($"Saved recording to: {path}");
            var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
            //Handheld.PlayFullScreenMovie($"{prefix}{path}");
        }
    }
}