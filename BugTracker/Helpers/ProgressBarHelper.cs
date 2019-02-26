using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    
    public class ProgressBarHelper
    {
        public static string GetColorClass(int progress)
        {
            var colorClass = "";

            if (progress > 0 && progress <= 25)
            {
                colorClass = "bg-danger";
            }
            else if (progress > 25 && progress <= 50)
            {
                colorClass = "bg-warning";
            }
            else if (progress > 50 && progress <= 75)
            {
                colorClass = "bg-info";
            }
            else
            {
                colorClass = "bg-success";
            }

            return colorClass;
        }

         
    }
}