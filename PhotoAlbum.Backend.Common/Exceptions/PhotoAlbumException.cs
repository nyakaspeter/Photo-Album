using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PhotoAlbum.Backend.Common.Exceptions
{
    public class PhotoAlbumException : Exception
    {
        public int StatusCode { get; set; }

        public PhotoAlbumException(string message, int statusCode = (int)HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
