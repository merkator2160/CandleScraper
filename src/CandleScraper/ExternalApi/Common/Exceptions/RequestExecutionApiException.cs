using System;
using System.Runtime.Serialization;

namespace CandleScraper.ExternalApi.Common.Exceptions
{
	public class RequestExecutionApiException : ApplicationException
	{
		public RequestExecutionApiException()
		{

		}
		public RequestExecutionApiException(String message) : base(message)
		{

		}
		public RequestExecutionApiException(String message, Exception ex) : base(message)
		{

		}
		protected RequestExecutionApiException(SerializationInfo info, StreamingContext contex) : base(info, contex)
		{

		}
	}
}