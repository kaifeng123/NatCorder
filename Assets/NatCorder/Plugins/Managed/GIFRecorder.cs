/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders {

    using System;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// Animated GIF image recorder.
    /// </summary>
    [Doc(@"GIFRecorder")]
    public sealed class GIFRecorder : IMediaRecorder {

        #region --Client API--
        /// <summary>
        /// Image size.
        /// </summary>
        [Doc(@"FrameSize")]
        public (int width, int height) frameSize => recorder.frameSize;

        /// <summary>
        /// Create a GIF recorder.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="frameDuration">Frame duration in seconds.</param>
        [Doc(@"GIFRecorderCtor")]
        public GIFRecorder (int width, int height, float frameDuration) => this.recorder = new NativeRecorder((callback, context) => Bridge.CreateGIFRecorder(width, height, frameDuration, Utility.GetPath(".gif"), callback, context));

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have RGBA32 pixel layout.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer containing video frame to commit.</param>
        /// <param name="timestamp">Not used.</param>
        [Doc(@"CommitFrame"), Code(@"RecordWebCam")]
        public void CommitFrame<T> (T[] pixelBuffer, long timestamp) where T : struct => recorder.CommitFrame(pixelBuffer, timestamp);

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have RGBA32 pixel layout.
        /// </summary>
        /// <param name="nativeBuffer">Pixel buffer in native memory to commit.</param>
        /// <param name="timestamp">Not used.</param>
        [Doc(@"CommitFrame")]
        public void CommitFrame (IntPtr nativeBuffer, long timestamp) => recorder.CommitFrame(nativeBuffer, timestamp);

        /// <summary>
        /// This recorder does not support committing sample buffers.
        /// </summary>
        [Doc(@"CommitSamplesNotSupported")]
        public void CommitSamples (float[] sampleBuffer, long timestamp) { }

        /// <summary>
        /// Finish writing and return the path to the recorded media file.
        /// </summary>
        [Doc(@"FinishWriting", @"FinishWritingDiscussion")]
        public Task<string> FinishWriting () => recorder.FinishWriting();
        #endregion

        private readonly IMediaRecorder recorder;
    }
}