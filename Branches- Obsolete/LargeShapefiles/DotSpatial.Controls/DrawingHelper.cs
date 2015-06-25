using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Contains misc utility methods
    /// </summary>
    internal static class DrawingHelper
    {
        public static void DrawUsingChunks<T>(
            this ISupportChunksDrawing layer,
            MapArgs args, List<T> list, List<Rectangle> clipRectangles, bool useChunks, Action<MapArgs, List<T>> draw)
        {
            var chunkSize = layer.ChunkSize;
            if (useChunks == false || list.Count < chunkSize)
            {
                draw(args, list);
                return;
            }

            var count = list.Count;
            var numChunks = (int)Math.Ceiling(count / (double)chunkSize);
            for (var chunk = 0; chunk < numChunks; chunk++)
            {
                var numFeatures = chunkSize;
                if (chunk == numChunks - 1) numFeatures = count - (chunk * chunkSize);
                draw(args, list.GetRange(chunk * chunkSize, numFeatures));

                if (numChunks > 0 && chunk < numChunks - 1)
                {
                    layer.OnBufferChanged(clipRectangles);
                    Application.DoEvents();
                }
            }
        }
    }
}