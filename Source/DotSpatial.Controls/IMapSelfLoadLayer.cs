// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMapSelfLoadLayer : IMapLayer
    {
        string Filename { get; set; }

        string FilePath { get; set; }
    }
}
