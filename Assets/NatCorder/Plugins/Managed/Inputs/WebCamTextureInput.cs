/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Recorders.Inputs {

    using UnityEngine;
    using System;
    using System.Collections;
    using Clocks;
    using Internal;

    /// <summary>
    /// Recorder input for recording video frames from a `WebCamTexture`.
    /// </summary>
    [Doc(@"WebCamTextureInput")]
    public class WebCamTextureInput : IDisposable {

        #region --Client API--
        /// <summary>
        /// Create a video recording input from a WebCamTexture.
        /// </summary>
        /// <param name="recorder">Media recorder to receive committed frames.</param>
        /// <param name="clock">Clock for generating timestamps.</param>
        /// <param name="webCamTexture">WebCamTexture to record from.</param>
        [Doc(@"WebCamTextureInputCtor")]
        public WebCamTextureInput (IMediaRecorder recorder, IClock clock, WebCamTexture webCamTexture) {
            this.recorder = recorder;
            this.clock = clock;
            this.webCamTexture = webCamTexture;
            this.pixelBuffer = webCamTexture.GetPixels32();
            this.attachment = new GameObject("WebCamTextureInputAttachment").AddComponent<WebCamTextureInputAttachment>();
            attachment.StartCoroutine(OnFrame());
        }

        /// <summary>
        /// Stop recorder input and release resources.
        /// </summary>
        [Doc(@"AudioInputDispose")]
        public void Dispose () => GameObject.Destroy(attachment.gameObject);
        #endregion


        #region --Operations--

        private readonly IMediaRecorder recorder;
        private readonly IClock clock;
        private readonly WebCamTexture webCamTexture;
        private readonly Color32[] pixelBuffer;
        private readonly WebCamTextureInputAttachment attachment;

        private IEnumerator OnFrame () {
            var endOfFrame = new WaitForEndOfFrame();
            while (true) {
                yield return endOfFrame;
                if (!webCamTexture.didUpdateThisFrame)
                    continue;
                webCamTexture.GetPixels32(pixelBuffer);
                recorder.CommitFrame(pixelBuffer, clock.timestamp);
            }
        }

        private sealed class WebCamTextureInputAttachment : MonoBehaviour { }
        #endregion
    }
}