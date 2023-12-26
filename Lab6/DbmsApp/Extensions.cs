using AutoMapper.Internal.Mappers;

namespace DbmsApp;

public static class Extensions
{
	public static string ToValues(this DateTime time)
	{
		return $"DATETIMEFROMPARTS({time.Year}, {time.Month}, {time.Day}, {time.Hour}, {time.Minute}, 0 ,0)";
	}
	
	public static string ToValues(this DateTime? time)
	{
		if (time is null)
			return "NULL";
		
		return $"DATETIMEFROMPARTS({time?.Year}, {time?.Month}, {time?.Day}, {time?.Hour}, {time?.Minute}, 0 ,0)";
	}
}