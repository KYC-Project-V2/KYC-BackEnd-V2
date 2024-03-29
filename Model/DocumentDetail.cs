using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DocumentDetail
    {
        public string Id { get; set; }
        public string? RequestNo { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public string? Sex { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? FolderPath { get; set; }
        public string? FullPath { get; set; }
        public string? OCRInformation { get; set; }
        public bool IsVaidDocumentType { get; set; } = false;
        //public string? PhisicalPath { get; set; }
        //public string? RepositoryPath { get; set; }

    }
}
