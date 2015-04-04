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
                        if (!File.Exists(path))
                        {
                            using (var dest = File.Create(path))
                                asset.CopyTo(dest);
                        }
                        else
                        {
                            // In Debugging-Version, replace existing files (effectively resetting every update)
                            // In Release-Version, keep updated files on Device
#if DEBUG
                            using (var dest = File.Create(path))
                                asset.CopyTo(dest);
#endif
                        }
                }
                catch (Exception)
                {
                    // Do nothing..
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