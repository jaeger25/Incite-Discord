using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Incite.Discord.Helpers
{
    public static class FileHelper
    {
        public static async IAsyncEnumerable<string> ReadExportFileAsync(string fileUrl)
        {
            using HttpClient client = new HttpClient();
            using var response = await client.GetStreamAsync(fileUrl);
            using TextReader reader = new StreamReader(response);

            for (string line = await reader.ReadLineAsync(); line != null; line = await reader.ReadLineAsync())
            {
                yield return line;
            }
        }
    }
}
