namespace System.Collections.Generic
{
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    
    internal static class IEnumerableExtensions
    {
        internal static IEnumerable<MediaTypeWithQualityHeaderValue>
            ToMediaTypeWithQualityHeaderValues(this IEnumerable<string> source)
        {
            foreach (var mediaType in source)
            {
                yield return new MediaTypeWithQualityHeaderValue(mediaType);
            }
        }
    }
}