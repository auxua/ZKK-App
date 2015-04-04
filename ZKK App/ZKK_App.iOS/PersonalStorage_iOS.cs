using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZKK_App.iOS;
using ZKK_App;
using Xamarin.Forms;
using MonoTouch;
using UIKit;
using Foundation;


[assembly: Xamarin.Forms.Dependency(typeof(PersonalStorage_iOS))]


namespace ZKK_App.iOS
{
    public class PersonalStorage_iOS : IPersonalStorage
    {

        public bool CopyAssets()
        {
            // The Assets-Folder provided with the App
            var assetsFolder = "Files/";
            foreach (string path in Directory.GetFiles(assetsFolder))
            {
                if (File.Exists(path))
                {
                    // This path is a file - get the desired location on device for it
                    var target = this.GetFullFilePath(path.Substring(assetsFolder.Length));

                    // In Debugging-Version, replace existing files (effectively resetting every update)
                    // In Release-Version, keep updated files on Device
#if DEBUG
                    File.Copy(path, target);
#else
                    if (!File.Exists(target))
                        File.Copy(path, target);
#endif
                }
                else
                {
                    // This path is a directory
                    // Should never happen...
                }
            } 
            return true;
              
        }

        public string GetFullFilePath(string name)
        {
            string cacheLibrary;

            // iOS 8 changed the Directories, therefore we need to decide on Version
            if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // prior to iOS 8 we can use this
                // Beware! This code is taken from documentation and never tested!
                cacheLibrary = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //cacheLibrary = Path.Combine(documents, "..", "Library", "Caches");

            }
            else
            {
                // On iOS 8, we need to use designated Folders!
                //var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
                cacheLibrary = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User)[0].Path;
                //cacheLibrary = Path.Combine(documents.AbsoluteString, "..", "Library", "Caches");
            }

            return Path.Combine(cacheLibrary, name);


        }
    }
}