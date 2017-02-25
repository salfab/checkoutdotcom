using System;

using checkoutdotcom.Filters;

namespace checkoutdotcom.Exceptions
{
    /// <summary>
    /// Thrown when a Controller needs to return a body-less 404 response.
    /// </summary>
    /// <remarks>
    ///     This exception is used in conjunction with <see cref="ResourceNotFoundExceptionToHttpStatusCodeConverterActionFilter"/>.
    ///     Using this pattern will keep controller signatures simple and allow them to return domain objects and custom types instead of classes implementing IActionResult.
    ///     The pattern aims at simplifying the development of unit tests, by letting them assert the value returned by an action without the complexity of dealing with the ASP.Net plumbing.
    /// </remarks>
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message)
            : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}