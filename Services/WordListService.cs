using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Services
{
    public class WordListService
    {
        public Task<IEnumerable<string>> GetWordsAsync(int numWords, int level)
        {
            return Task.Run(() =>
            {
                var resStream = this.GetType()
                    .GetTypeInfo()
                    .Assembly.GetManifestResourceStream("Services.level" + level + ".json");

                JsonSerializer serializer = new JsonSerializer();

                using (var sr = new StreamReader(resStream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<List<string>>(jsonTextReader).PickRandom(numWords);
                }
            });
        }
    }

    public static class EnumerableExtension
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
