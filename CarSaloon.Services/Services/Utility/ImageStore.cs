using CarSaloon.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSaloon.Services.Services.Utility
{
    public static class ImageStore
    {
        public static string SaveImage(BaseViewModel model,string imagesFolderName)
        {
            string imageFileName = null;

            if (model.Image != null)
            {
                imageFileName = Guid.NewGuid().ToString().Substring(0,10) + "_" + model.Image.FileName;

                string filePath=Path.Combine(imagesFolderName,imageFileName);

                using var filestream=new FileStream(filePath, FileMode.Create);

                model.Image.CopyTo(filestream);
            }

            return imageFileName;
        }
    }
}
