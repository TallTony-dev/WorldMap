using System;
using System.Collections.Generic;
using System.Text;

namespace DriverLib
{
    internal interface ICamera
    {
        public Task<Image> GetImage();

    }

    public struct Image
    {
        public string ImageType; //file extension ex (jpeg, png, bmp, etc.)
        public byte[] Data;
    }
}
