using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZKK_App.Droid;
using Xamarin.Forms;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(PersonalStorage_Android))]


namespace ZKK_App.Droid
{
    public class PersonalStorage_Android : IPersonalStorage
    {

        public bool CopyAssets()
        {
            //In Android, we cannot edit Assets and Resources are converted.
            //Therefore, copy Assets to personal Storage
            string[] liste = Forms.Context.Assets.List("");
            
            foreach (string s in liste)
            {
                //Console.WriteLine("Asset Found: " + s);
                try
                {
                    var path = this.GetFullFilePath(s);
                    using (var asset = Forms.Context.Assets.Open(s))
                    using (var dest = File.Create(path))
                        asset.CopyTo(dest);
                }
                catch (Exception e)
                {
                    //Console.WriteLine("not possible: " + e.Message);
                }
            }

            return true;
        }


        public string GetFullFilePath(string name)
        {
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), name);
            return path;
        }
    }
}