using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sharprdf.WebApp.Models
{
    public class SourceCodeFileViewModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}