using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Controls
{
    internal interface ISupportChunksDrawing
    {
        int ChunkSize { get; }
        void OnBufferChanged(List<Rectangle> clipRectangles);
    }
}