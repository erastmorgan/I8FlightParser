using System;
namespace I8FlightParser.Abstract
{
	public interface ISearchCriteria
	{
        Guid SearchId { get; }

        FormUrlEncodedContent ToFormUrlEncodedContent();
    }
}

