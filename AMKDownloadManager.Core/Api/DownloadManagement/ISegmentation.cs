using System.Collections.Generic;
using AMKsGear.Core.Collections;

namespace AMKDownloadManager.Core.Api.DownloadManagement
{
    /// <summary>
    /// Provides informational access to SegmentationContext.
    /// </summary>
    public interface ISegmentation
    {
        /// <summary>
        /// Total resource size.
        /// </summary>
        long TotalSize { get; }
        
        /// <summary>
        /// Provides Mutable access to SegmentationContext.FilledRanges
        /// </summary>
        /// <remarks>
        /// Changing entries will affect original SegmentationContext's segments.
        /// </remarks>
        IEnumerable<Segment> FilledRanges { get; }
        /// <summary>
        /// Provides Mutable access to SegmentationContext.ReservedRanges
        /// </summary>
        /// <remarks>
        /// Changing entries will affect original SegmentationContext's segments.
        /// </remarks>
        IEnumerable<Segment> ReservedRanges { get; }
        
        /// <summary>
        /// Provides access to SegmentationContext.Properties used to store additional data.
        /// </summary>
        PropertyBag Properties { get; }

        void Clean();

        IEnumerable<Segment> Reverse();
        
    }
}