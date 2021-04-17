using Microsoft.Extensions.Logging;
using Sealbreaker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MIMSV3SiteMapGenerator
{
    class FTPService
    {
        public static void MoveToWebSiteFolder(string directory, ILogger log)
        {

            RemoveDirectoryFiles(log);
            foreach (var file in Directory.GetFiles(directory))
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(file);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(AppSettings.AzureFTPBaseURL + fileInfo.Name + "");
                    request.EnableSsl = true;
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(AppSettings.AzureFTPUserName, AppSettings.AzureFTPPassword);
                    byte[] fileContents;
                    using (StreamReader sourceStream = new StreamReader(file))
                    {
                        fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    }

                    request.ContentLength = fileContents.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(fileContents, 0, fileContents.Length);
                    }

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                    }
                }
                catch (Exception ex)
                {
                    log.LogError(ex.StackTrace + ex.Message);
                }
            }
        }

        //private static void CreateDirectory()
        //{
        //    var directoryPath = "ftp://waws-prod-sg1-039.ftp.azurewebsites.windows.net/site/wwwroot/Output";
        //    if (!DoesFtpDirectoryExist(directoryPath))
        //    {
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
        //        request.Method = WebRequestMethods.Ftp.MakeDirectory;

        //        request.Credentials = new NetworkCredential(@"uat-mimsv3\$uat-mimsv3", "itvjScDDvl03ZJ8cqoBQ3WZ5ZsfnDdcTsu4LDPcMyDTGy34oBxxCqo4518jm");

        //        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
        //        {
        //        }
        //    }
        //}

        private static void RemoveDirectoryFiles(ILogger log)
        {
            try
            {
                var directoryPath = AppSettings.AzureFTPBaseURL;
                if (DoesFtpDirectoryExist(directoryPath))
                {
                    var filesList = DirectoryListing(directoryPath);

                    foreach (var file in filesList)
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath + file);
                        request.Method = WebRequestMethods.Ftp.DeleteFile;

                        request.Credentials = new NetworkCredential(AppSettings.AzureFTPUserName, AppSettings.AzureFTPPassword);

                        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                        {
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace + ex.Message);
            }
        }



        public static List<string> DirectoryListing(string directoryPath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(AppSettings.AzureFTPUserName, AppSettings.AzureFTPPassword);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            List<string> result = new List<string>();

            while (!reader.EndOfStream)
            {
                result.Add(reader.ReadLine());
            }

            reader.Close();
            response.Close();

            return result.Where(i => i.Contains("SiteMap")).ToList();
        }

        private static bool DoesFtpDirectoryExist(string dirPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(AppSettings.AzureFTPUserName, AppSettings.AzureFTPPassword);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                }
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }

    }
}
