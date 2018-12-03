using System;
using System.Net;

namespace CandleScraper.ExternalApi.Common.Exceptions
{
	public class HttpServerException : Exception
	{
		public HttpServerException(HttpStatusCode statusCode, String message) : base(message)
		{
			StatusCode = statusCode;
		}

		public HttpStatusCode StatusCode { get; }
	}
}