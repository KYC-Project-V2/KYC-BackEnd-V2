using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Email
    {
        public string FromAddess { get; set; }
        public string ToAddress { get; set; }
        public string? OtpNumber { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? RequestNumber { get; set; }
        public string? Name { get; set; }
        public string? AttachmentPath { get; set; }
        public EmailConfiguration? emailConfiguration { get; set; }
    }
}
