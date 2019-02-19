using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class iconHelper
    {
        public static string GetAssociatedIcon(string fileName)
        {
            var icon = "";
            var fileExtension = Path.GetExtension(fileName);

            switch (fileExtension)
            {
                case ".pdf":
                    icon = "fa fa-file-pdf-o";
                    break;
                case ".doc":
                case ".docx":
                    icon = "fa fa-file-word-o";
                    break;
                case ".xls":
                case ".xlsx":
                    icon = "fa fa-file-excel-o";
                    break;
                case ".ppt":
                case ".pptx":
                    icon = "fa fa-file-powerpoint-o";
                    break;
                case ".txt":
                    icon = "fa faf-file-text-o";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".tiff":
                case ".bmp":
                case ".gif":
                    icon = "fa fa-file-image-o";
                    break;
                default:
                    icon = "fa fa-file-code-o";
                    break;
                   

            }
            return icon;
        }
    }
}