using System;
namespace I8FlightParser.Abstract
{
	public interface ISearchCriteria
	{
        FormUrlEncodedContent ToFormUrlEncodedContent();
    }
}

