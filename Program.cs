using Azure;
using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureFileStorage1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
             .AddJsonFile("config.json")
             .Build();
            string connectionString = configuration["StorageConnectionString"];

            await UploadAsync(connectionString, "sample-share", "Writing for december test.txt");
                

        }

        public static async Task UploadAsync(string connectionToString, string shareName, string localFilePath)
        {
            string dirName = "sample-dir-name";
            string fileName = "samlpe-file-name";
            ShareClient share = new ShareClient(connectionToString, shareName);

            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            await directory.CreateAsync();

            ShareFileClient file = directory.GetFileClient(fileName);

            using (FileStream stream = File.OpenRead(localFilePath))
            {
                await file.CreateAsync(stream.Length);
                await file.UploadRangeAsync(
                   new HttpRange(0, stream.Length),
                   stream);
                
            }
           
        }
    }
}
