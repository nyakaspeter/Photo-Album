using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Backend.Common.Dtos;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Net;

namespace PhotoAlbum.Backend.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerDefaultResponse]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorDto))]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(ErrorDto))]
    [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(ErrorDto))]
    [SwaggerResponse(HttpStatusCode.Forbidden, typeof(ErrorDto))]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorDto))]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
