using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    internal interface ICamera
    {
        /// <summary>
        /// Gets an image from the forward direction
        /// </summary>
        public Task<Image> GetImageFromFront();

        /// <summary>
        /// Gets an image from the direction specified, NOT BY COMPASS, north = forwards, south = backwards
        /// </summary>
        public Task<Image> GetImageFromDirection(Direction direction);
    }

    public struct Image
    {
        public string ImageType; //file extension ex (jpeg, png, bmp, etc.)
        public byte[] Data;
    }
}
