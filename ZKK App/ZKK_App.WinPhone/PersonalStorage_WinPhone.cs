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
            //On Windows Phone their is no need for this!
            //We can use the Files directly
            return true;
              
        }


        public string GetFullFilePath(string name)
        {
            //On Windows Phone it is in the Files-SubDir.
            String fname = "Files/" + name;
            return fname;
        }
    }
}