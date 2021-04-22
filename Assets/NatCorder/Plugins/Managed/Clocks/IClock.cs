/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders.Clocks {

    /// <summary>
    /// Clock for generating recording timestamps.
    /// The spacing between timestamps determines the actual framerate of the recording.
    /// Clocks are important for synchronizing audio and video tracks when recording with audio.
    /// Clocks are thread-safe, so they can be used on multiple threads simultaneously.
    /// </summary>
    public interface IClock {
        
        /// <summary>
        /// Current timestamp in nanoseconds.
        /// The very first value reported by this property MUST always be zero.
        /// </summary>
        long timestamp { get; }
    }
}