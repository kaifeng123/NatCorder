/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using System.Collections;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;
    using UnityEditor;

    public class ReplayCam : MonoBehaviour {

        [Header("Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;

        private IMediaRecorder recorder;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        private AudioSource microphoneSource;
        public AudioListener audioListener;
        public Camera cam;

        private IEnumerator Start () {
            // Start microphone
            microphoneSource = gameObject.AddComponent<AudioSource>();
            microphoneSource.mute =
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            if(recordMicrophone)
                microphoneSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();
        }

        private void OnDestroy () {
            // Stop microphone
            if (recordMicrophone)
            {
                microphoneSource.Stop();
                Microphone.End(null);

            }
               
        }

        public void StartRecording () {
            // Start recording
            var frameRate = 30;
            //var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            //var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            var sampleRate =  AudioSettings.outputSampleRate;
            var channelCount = (int)AudioSettings.speakerMode ;
            var clock = new RealtimeClock();
            recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount);
            // Create recording inputs
            cameraInput = new CameraInput(recorder, clock, cam);
            //audioInput = recordMicrophone ? new AudioInput(recorder, clock, microphoneSource, true) : null;
            audioInput = new AudioInput(recorder, clock, audioListener);
            // Unmute microphone
            microphoneSource.mute = audioInput == null;
        }

        public async void StopRecording () {
            // Mute microphone
            microphoneSource.mute = true;
            // Stop recording
            audioInput?.Dispose();
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            // Playback recording
            Debug.Log($"Saved recording to: {path}");
            var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";

#if UNITY_EDITOR
            EditorUtility.OpenWithDefaultApp(path);
#elif UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + path);
#elif UNITY_ANDROID
            Handheld.PlayFullScreenMovie(path);
#endif
            // 移动端，录频完后，播放录制的视频
            //Handheld.PlayFullScreenMovie($"{prefix}{path}");
        }
    }
}