using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZKK_App.WinPhone;
using ZKK_App;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(PersonalStorage_WinPhone))]
namespace ZKK_App.WinPhone
{
    public class PersonalStorage_WinPhone : IPersonalStorage
    {

        public bool CopyAssets()
        {
            // On Windows Phone, Copy from Files/ to Data/
            string[] files = Directory.GetFiles("Files/");

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    // it is a file and no directory
                    // get the file without directory prefix
                    string theFile = file.Remove(0, 6);
                    string target = this.GetFullFilePath(theFile);
                    
                    if (!File.Exists(target))
                    {
                        File.Copy(file, target);
                    }
                    else
                    {
                        // Copy the file only if being in debug-mode
#if DEBUG
                        File.Copy(file,target,true);
#endif
                    }
                }
            }
            
            return true;


              
        }


        public string GetFullFilePath(string name)
        {
            //On Windows Phone it is in the Data-SubDir.
            //String fname = "Files/" + name;
            
            String fname = "Data/" + name;
            return fname;
        }
    }
}