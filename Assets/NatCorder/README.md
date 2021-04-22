# NatCorder API
NatCorder is a lightweight, easy-to-use, native video recording API for iOS, Android, macOS, and Windows. NatCorder comes with a rich featureset including:
+ Record any texture, anything that can be rendered into a texture, or any pixel data.
+ Record to MP4 videos and animated GIF images.
+ Control recording quality and file size with bitrate and keyframe interval.
+ Record at any resolution. You get to specify what resolution recording you want.
+ Get path to recorded video in device storage.
+ Record game audio with video.
+ Support for recording HEVC videos.

## Fundamentals of Recording
NatCorder provides a simple recording API with instances of the `IMediaRecorder` interface. **NatCorder works by encoding video and audio frames on demand**. To start recording, simply create a recorder corresponding to the media type you want to record:
```csharp
var gifRecorder = new GIFRecorder(...);
var mp4Recorder = new MP4Recorder(...);
var hevcRecorder = new HEVCRecorder(...);
```

Once you create a recorder, you then commit frames to it. You can commit video and audio frames to these recorders. These committed frames are then encoded into a media file. When committing frames, you must provide the frame data with a corresponding timestamp. The spacing between timestamps determine the final frame rate of the recording.

### Committing Video Frames
NatCorder records video using pixel buffers. The pixel buffers must be 32-bit per pixel, RGBA encoding (`TextureFormat.RGBA32`). The managed type of the pixel buffer is entirely flexible. As a result, you can commit a `Color32[]`, a `byte[]`, an `int[]`, or any struct array that can be interpreted as an `RGBA32` pixel buffer.

When committing a pixel buffer for encoding, you will need to provide a corresponding timestamp. For this purpose, you can use implementations of the `IClock` interface. Here is an example illustrating recording a `WebCamTexture`:
```csharp
async void Start () {
    // Start camera
    var cameraTexture = new WebCamTexture();
    cameraTexture.Play();
    // Create a clock for generating recording timestamps
    var clock = new RealtimeClock();
    // Create a recorder
    var recorder = new MP4Recorder(...);
    // Record 150 frames
    for (int i = 0; i < 150; i++) {
        // Commit the frame to NatCorder for encoding
        recorder.CommitFrame(cameraTexture.GetPixels32(), clock.timestamp);
        // Wait for some milliseconds
        await Task.Delay(10);
    }
    // Finish writing
    var recordingPath = await recorder.FinishWriting();
}
```

### Committing Audio Frames
NatCorder records audio provided as interleaved PCM sample buffers (`float[]`). Similar to recording video frames, you will call the `IMediaRecorder.CommitSamples` method, passing in a sample buffer and a corresponding timestamp. It is important that the timestamps synchronize with those of video, so it is recommended to use the same `IClock` for generating video and audio timestamps. Below is an example illustrating recording game audio using Unity's `OnAudioFilterRead` callback:
```csharp
void OnAudioFilterRead (float[] sampleBuffer, int channels) {
    // Commit the audio frame
    recorder.CommitSamples(sampleBuffer, clock.Timestamp);
}
```

## Easier Recording with Recorder Inputs
In most cases, you will likely just want to record a game camera optionally with game audio. To do so, you can use NatCorder's recorder `Inputs`. A recorder `Input` is a lightweight utility class that eases out the process of recording some aspect of a Unity application. NatCorder comes with two recorder inputs: `CameraInput` and `AudioInput`. You can create your own recorder inputs to do more interesting things like add a watermark to the video, or retime the video. Here is a simple example showing recording a game camera:
```csharp
IClock clock;
IMediaRecorder recorder;
CameraInput cameraInput;
AudioInput audioInput;

void StartRecording () {
    // Create a recording clock
    clock = new RealtimeClock();
    // Start recording
    mediaRecorder = new ...;
    // Create a camera input to record the main camera
    cameraInput = new CameraInput(recorder, clock, Camera.main);
    // Create an audio input to record the scene's AudioListener
    audioInput = new AudioInput(recorder, clock, audioListener);
}

async void StopRecording () {
    // Destroy the recording inputs
    cameraInput.Dispose();
    audioInput.Dispose();
    // Stop recording
    var recordingPath = await recorder.FinishWriting();
}
```

___

## Using NatCorder with NatDevice
If you use NatCorder with our NatDevice API, then you will have to remove a duplicate copy of the `NatRender.aar` library. The duplicate libraries can be found at `NatCorder > Plugins > Android > NatRender.aar` or `NatDevice > Plugins > Android > NatRender.aar`.

## Tutorials
- [Unity Recording Made Easy](https://medium.com/@olokobayusuf/natcorder-unity-recording-made-easy-f0fdee0b5055)
- [Audio Workflows](https://medium.com/@olokobayusuf/natcorder-tutorial-audio-workflows-1cfce15fb86a)

## Requirements
- Unity 2018.3+
- Android API Level 21+
- iOS 11+
- macOS 10.13+
- Windows 10+, 64-bit only

## Notes
- NatCorder doesn't support recording UI canvases that are in Screen Space - Overlay mode. See [this](https://forum.unity3d.com/threads/render-a-canvas-to-rendertexture.272754/#post-1804847).
- When recording audio, make sure that the 'Bypass Listener Effects' and 'Bypass Effects' flags on your `AudioSource`s are turned off.
- If you face `DllNotFound` errors on standalone Windows builds, install the latest Visual C++ redistributable from Microsoft.
- Recording may fail when a dimension (width or height) is an odd number. Always make sure to record at even resolutions.

## Quick Tips
- Please peruse the included scripting reference in the `Docs` folder.
- To discuss or report an issue, visit Unity forums [here](https://forum.unity.com/threads/natcorder-video-recording-api.505146/)
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com)

Thank you very much!