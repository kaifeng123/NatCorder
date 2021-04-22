/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders {

    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// Waveform audio recorder.
    /// </summary>
    [Doc(@"WAVRecorder")]
    public sealed class WAVRecorder : IMediaRecorder {

        #region --Client API--
        /// <summary>
        /// Not supported.
        /// </summary>
        [Doc(@"FrameSizeNotSupported")]
        public (int width, int height) frameSize => default;

        /// <summary>
        /// Create an WAV recorder.
        /// </summary>
        /// <param name="sampleRate">Audio sample rate.</param>
        /// <param name="channelCount">Audio channel count.</param>
        [Doc(@"WAVRecorderCtor")]
        public WAVRecorder (int sampleRate, int channelCount) {
            this.sampleRate = sampleCount;
            this.channelCount = channelCount;
            this.stream = new FileStream(Utility.GetPath(".wav"), FileMode.Create);
            // Allocate WAVE header
            stream.Write(new byte[44], 0, 44);
        }

        /// <summary>
        /// This recorder does not support committing pixel buffers.
        /// </summary>
        [Doc(@"CommitFrameNotSupported")]
        public void CommitFrame<T> (T[] pixelBuffer, long timestamp) where T : struct { }

        /// <summary>
        /// This recorder does not support committing pixel buffers.
        /// </summary>
        [Doc(@"CommitFrameNotSupported")]
        public void CommitFrame (IntPtr nativeBuffer, long timestamp) { }

        /// <summary>
        /// Commit an audio sample buffer for encoding.
        /// </summary>
        /// <param name="sampleBuffer">Raw PCM audio sample buffer, interleaved by channel.</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds.</param>
        [Doc(@"CommitSamples")]
        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            // Convert to short array
            var shortBuffer = new short[sampleBuffer.Length];
            var byteBuffer = new byte[Buffer.ByteLength(shortBuffer)];
            for (int i = 0; i < sampleBuffer.Length; i++) 
                shortBuffer[i] = (short)(sampleBuffer[i] * short.MaxValue);
            // Write to output stream
            Buffer.BlockCopy(shortBuffer, 0, byteBuffer, 0, byteBuffer.Length);
            stream.Write(byteBuffer, 0, byteBuffer.Length);
            sampleCount += sampleBuffer.Length;
        }

        /// <summary>
        /// Finish writing and return the path to the recorded media file.
        /// </summary>
        [Doc(@"FinishWriting")]
        public Task<string> FinishWriting () {
            // Write header
            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(Encoding.UTF8.GetBytes("RIFF"), 0, 4);
            stream.Write(BitConverter.GetBytes(stream.Length - 8), 0, 4);
            stream.Write(Encoding.UTF8.GetBytes("WAVE"), 0, 4);
            stream.Write(Encoding.UTF8.GetBytes("fmt "), 0, 4);
            stream.Write(BitConverter.GetBytes(16), 0, 4);
            stream.Write(BitConverter.GetBytes((ushort)1), 0, 2);
            stream.Write(BitConverter.GetBytes(channelCount), 0, 2);                              // Channel count
            stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);                                // Sample rate
            stream.Write(BitConverter.GetBytes(sampleRate * channelCount * sizeof(short)), 0, 4); // Output rate in bytes
            stream.Write(BitConverter.GetBytes((ushort)(channelCount * 2)), 0, 2);                // Block alignment
            stream.Write(BitConverter.GetBytes((ushort)16), 0, 2);                                // Bits per sample
            stream.Write(Encoding.UTF8.GetBytes("data"), 0, 4);
            stream.Write(BitConverter.GetBytes(sampleCount * sizeof(ushort)), 0, 4);             // Total sample count
            // Close stream and return
            stream.Dispose();
            return Task.FromResult(stream.Name);
        }
        #endregion


        #region --Operations--
        private readonly int sampleRate, channelCount;
        private readonly FileStream stream;
        private int sampleCount;
        #endregion
    }
}