using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Object
{
    public class UploadFileResult : ResultBase<bool>
    {
        public UploadFileResult()
        {
            UploadFileObject = new List<UploadFileObject>();
        }
        public List<UploadFileObject> UploadFileObject { get; set; }

    }

    public class UploadFileObject
    {
        public int Id { get; set; }
        public int CertificateId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string UploadBy { get; set; }
        public string FileType { get; set; }
        public DateTime UploadDate { get; set; }
    }
}


