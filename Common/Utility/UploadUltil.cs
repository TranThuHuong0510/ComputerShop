using Common.Core;
using Common.Object;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Utility
{
    public class UploadUltil
    {
        private static readonly HttpClient _Client = new HttpClient();

        /// <summary>
        /// Upload File Common
        /// </summary>
        /// <returns></returns>
        public UploadFileResult UploadFileCommon(UploadCertificateAttachmentParameter data)
        {
            var result = new UploadFileResult();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var files = httpRequest.Files;
                if (files.Count > 0)
                {
                    var listFile = new List<UploadFileObject>();
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        string type = fileName.Substring(fileName.LastIndexOf('-') + 1);
                        string name = fileName.Substring(0, fileName.LastIndexOf('-'));
                        var path = ChangeFileName(name);
                        if (String.IsNullOrEmpty(data.Folder))
                        {
                            data.Folder = String.Empty;
                        }
                        //path = Path.Combine(HttpContext.Current.Server.MapPath(CommonResources.ServerPath + "/UploadFiles/" + folder), path); 

                        string strFilePath = ConfigurationManager.AppSettings["UpLoadPhysicalPath"].ToString() + "\\" + data.Folder;
                        path = Path.Combine(strFilePath, path);

                        if (File.Exists(path))
                        {
                            result.Message = "The file : " + path + " existed!";

                            return result;
                        }

                        listFile.Add(new UploadFileObject
                        {
                            FileType = type,
                            FileName = Path.GetFileName(name),
                            FilePath = GetPath(path)
                        });
                        result.UploadFileObject = listFile;

                        if (!Directory.Exists(strFilePath))
                        {
                            Directory.CreateDirectory(strFilePath);
                        }
                        file.SaveAs(path);
                    }
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                return result;
            }
        }
        public static UploadFileResult UploadImage(string subFolderName, int? byteSize = null, string desiredFileName = null)
        {
            var result = new UploadFileResult();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                var files = httpRequest.Files;
                if (files.Count > 0)
                {
                    var listFile = new List<UploadFileObject>();
                    for (var i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var fileName = Path.GetFileName(file.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        if (!allowedFileExtensions.Contains(fileExtension.ToLower()))
                        {
                            result.Status = false;
                            result.Message = "You are uploading a non-image file";
                            return result;
                        }
                        int iFileSize = file.ContentLength;
                        if (byteSize != null && iFileSize > byteSize)  // 1MB approx (actually less though)
                        {
                            result.Status = false;
                            result.Message = "Max size of uploaded image must be less than 50k";
                            return result;
                        }
                        string name = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToFileTimeUtc() + fileExtension;
                        if (desiredFileName != null)
                        {
                            name = desiredFileName + DateTime.Now.ToFileTimeUtc() + fileExtension;
                        }
                        string strFilePath = HttpContext.Current.Server.MapPath("~/" + subFolderName);

                        Directory.CreateDirectory(strFilePath);
                        file.SaveAs(Path.Combine(strFilePath, name));

                        listFile.Add(new UploadFileObject
                        {
                            FileType = fileExtension,
                            FileName = name,
                            FilePath = subFolderName + "/" + name
                        });
                        result.UploadFileObject = listFile;
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = "No file selected";
                    return result;
                }
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                Logger.CreateLog(Logger.Levels.ERROR, "Upload Util", "UploadImage(string subFolderName,string desiredFileName=null))", string.Empty, ex.ToString());
                return result;
            }
        }


        public static async Task<HttpResponseMessage> UploadImageToCloud()
        {
            var httpRequest = HttpContext.Current.Request;
            try
            {
                var uri = ConfigurationManager.AppSettings["ImageAzureApi"].ToString() + "/api/v1/application/upload/image";
                var appId = ConfigurationManager.AppSettings["ImageAppId"].ToString();
                var folderId = ConfigurationManager.AppSettings["ImageFolderId"].ToString();

                HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(uri);

                var file = httpRequest.Files[0];
                var fileName = Path.GetFileName(file.FileName);

                var paramsSend = new Dictionary<string, string> {
                    { "Description", "Description" },
                    { "FolderId", folderId},
                    { "Name", fileName},
                    { "UploadTo","1" }
                };

                string originalBoundary = "--------------------------" + DateTime.Now.Ticks.ToString();
                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString();
                byte[] boundaryBytesInit = System.Text.Encoding.UTF8.GetBytes(boundary + "\r\n");
                byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n" + boundary + "\r\n");
                byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n" + boundary + "--\r\n");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.ContentType = "multipart/form-data; boundary=" + originalBoundary;
                request.Method = "POST";
                request.KeepAlive = true;
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "PostmanRuntime/7.2.0";
                request.Headers.Add("applicationId", appId);

                Stream requestStream = request.GetRequestStream();

                int bytesRead = 0;
                var extension = Path.GetExtension(fileName).ToLower();
                string mimeType = extension == ".jpg" || extension == ".jpeg" || extension == ".png" ? "jpeg" : extension.Replace(".", "");
                byte[] formFileItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: image/{2}\r\n\r\n", "File", fileName, mimeType));
                requestStream.Write(boundaryBytesInit, 0, boundaryBytesInit.Length);
                requestStream.Write(formFileItemBytes, 0, formFileItemBytes.Length);
                byte[] buffer = new byte[1024];
                while ((bytesRead = file.InputStream.Read(buffer, 0, 1024)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                foreach (string key in paramsSend.Keys)
                {
                    byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", key, paramsSend[key]));
                    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    requestStream.Write(formItemBytes, 0, formItemBytes.Length);
                }

                requestStream.Write(trailer, 0, trailer.Length);
                requestStream.Close();

                using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    var responseData = reader.ReadToEnd();
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StringContent(responseData, Encoding.UTF8, "application/json");
                    return response;
                };

            }
            catch (Exception ex)
            {
                Logger.CreateLog(Logger.Levels.ERROR, httpRequest, "UploadImageToCloud()", string.Empty, ex.ToString());
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }

        }

        public static async Task<HttpResponseMessage> UploadToCloud(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            var uri = ConfigurationManager.AppSettings["ImageAzureApi"].ToString() + "/api/v1/application/upload/image";
            var appId = ConfigurationManager.AppSettings["ImageAppId"].ToString();
            var folderId = ConfigurationManager.AppSettings["ImageFolderId"].ToString();

            var paramsSend = new Dictionary<string, string> {
                { "Description", "Description" },
                { "FolderId", folderId},
                { "Name", fileName},
            };

            string originalBoundary = "--------------------------" + DateTime.Now.Ticks.ToString();
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundaryBytesInit = System.Text.Encoding.UTF8.GetBytes(boundary + "\r\n");
            byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n" + boundary + "\r\n");
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n" + boundary + "--\r\n");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "multipart/form-data; boundary=" + originalBoundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "PostmanRuntime/7.2.0";
            request.Headers.Add("applicationId", appId);

            Stream requestStream = request.GetRequestStream();

            if (File.Exists(filePath))
            {
                int bytesRead = 0;
                var extension = Path.GetExtension(fileName).ToLower();
                string mimeType = extension == ".jpg" || extension == ".jpeg" || extension == ".png" ? "jpeg" : extension.Replace(".", "");
                byte[] formFileItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: image/{2}\r\n\r\n", "File", fileName, mimeType));
                //byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: image/jpeg\r\n\r\n", "File", fileName));
                requestStream.Write(boundaryBytesInit, 0, boundaryBytesInit.Length);
                requestStream.Write(formFileItemBytes, 0, formFileItemBytes.Length);
                byte[] buffer = new byte[1024];

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    while ((bytesRead = fileStream.Read(buffer, 0, 1024)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }

                    fileStream.Close();
                }
            }

            foreach (string key in paramsSend.Keys)
            {
                byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", key, paramsSend[key]));
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                var responseData = reader.ReadToEnd();
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(responseData, Encoding.UTF8, "application/json");
                return response;
            };
        }

        public ResultBase<bool> DeleteFileCommon(DeleteAttachmentParameter data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data.FilePart))
                {
                    data.FilePart = GetPhysicalPath(data.FilePart);
                    File.Delete(data.FilePart);
                    return new ResultBase<bool>
                    {
                        Status = true
                    };
                }
                return new ResultBase<bool>
                {

                    Status = false
                };

            }
            catch (Exception ex)
            {
                return new ResultBase<bool>
                {
                    Message = ex.Message,
                    Status = false
                };
            }
        }
        public string GetServerPath(string filepath)
        {

            try
            {

                return ConfigurationManager.AppSettings["FileShare"] + filepath;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

        }
        public string GetPhysicalPath(string filepath)
        {

            try
            {

                return ConfigurationManager.AppSettings["UpLoadPhysicalPath"] + filepath;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

        }
        public string GetPath(string filepath)
        {
            string strAttachmentFile = String.Empty;
            try
            {
                var path = filepath.Substring(filepath.IndexOf(ConfigurationManager.AppSettings["UpLoadPhysicalPath"], StringComparison.Ordinal) + ConfigurationManager.AppSettings["UpLoadPhysicalPath"].Length).Replace("\\", "/");
                return path;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }

        }
        public string GetFileType(string extension)
        {
            var path = Path.GetExtension(extension);
            if (!String.IsNullOrEmpty(path))
            {
                path = path.ToLower().Remove(0, 1);
                if (path == "doc" || path == "docx")
                {
                    return "Word";
                }
                if (path == "xls" || path == "xlsx")
                {
                    return "Excel";
                }
                if (path == "ppt" || path == "pptx")
                {
                    return "PowerPoint";
                }
                if (path == "txt")
                {
                    return "Txt";
                }
                if (path == "csv")
                {
                    return "Csv";
                }
                if (path == "xml")
                {
                    return "Xml";
                }
                if (path == "pdf")
                {
                    return "Pdf";
                }
                if (path == "jpeg" || path == "jpg" || path == "png" || path == "gif")
                {
                    return "Image";
                }
            }
            return String.Empty;
        }
        #region Private

        private string ChangeFileName(string fileName)
        {
            return $"{DateTime.UtcNow:yyyyMMddHHmmssfff}-{fileName}";
        }

        #endregion Private
    }
}
