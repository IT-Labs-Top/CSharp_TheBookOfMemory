using System.IO;
using System.Net.Http;
using Serilog;
using TheBookOfMemory.Models.Client;

namespace TheBookOfMemory.Utilities
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items) => items.ForEach(source.Add);


        public static void AddWhere<T>(this ICollection<T> source, IEnumerable<T> items, Func<T, bool> predicate) => items.ForEachWhere(source.Add, predicate);

        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> items) => items.ForEach(i => source.Remove(i));

        public static void RemoveWhere<T>(this ICollection<T> source, IEnumerable<T> items, Func<T, bool> predicate) => items.ForEachWhere(i => source.Remove(i), predicate);

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        public static void ForEachWhere<T>(this IEnumerable<T> source, Action<T> action, Func<T, bool> predicate)
        {
            foreach (var item in source) if (predicate(item)) action(item);
        }

        public static async Task<string> LoadImageAndGetPath(this IMainApiClient client, ILogger logger, string url, string localPath = "AllImages")
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            if (File.Exists(Path.GetFullPath(url))) return url;
            var filename = url.Replace('/', '_');
            if (string.IsNullOrEmpty(filename)) return string.Empty;

            var imageFile = Path.GetFullPath(Path.Combine(localPath, filename));

            if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);
            if (File.Exists(imageFile)) return imageFile;

            HttpResponseMessage? response = null;
            await client
                .LoadImage(url.TrimStart('/'))
                .TryExecuteRequest(r => response = r, logger);
            if (response is null) return string.Empty;
            if (File.Exists(imageFile)) return imageFile;
            await using var fs = new FileStream(
                imageFile,
                FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
            return imageFile;
        }

        public static async Task TryExecuteRequest<T>(this Task<T> task, Action<T> onSuccess, Action onFailed, ILogger logger)
        {
            try
            {
                onSuccess(await task);
            }
            catch (Exception e)
            {
                onFailed();
                logger.Error(e.Message);
            }
        }
        public static async Task TryExecuteRequest<T>(this Task<T> task, Action<T> onSuccess, ILogger logger)
        {
            try
            {
                onSuccess(await task);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
        public static async Task TryExecuteRequest<T>(this Task<T> task, Func<T, Task> onSuccess, ILogger logger)
        {
            try
            {
                await onSuccess(await task);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
        public static async Task TryExecuteRequest(this Task task, ILogger logger)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
        public static async Task TryExecuteRequest(this Task task, Action onSuccess, ILogger logger)
        {
            try
            {
                await task;
                onSuccess();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
