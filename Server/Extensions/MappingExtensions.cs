using AutoMapper;

namespace LabApp.Server.Extensions
{
	public static class MappingExtensions
	{
		public static TDestination MapFromTwo<TSource, TSource2, TDestination>(this IMapper mapper,
			 TSource source, TSource2 source2)
		{
			return mapper.Map(source2, mapper.Map<TDestination>(source));
		}
	}
}