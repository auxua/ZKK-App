using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using ZKK_App.WinPhone;
using ZKK_App;
using Xamarin.Forms;
using Windows.Storage;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


[assembly: Xamarin.Forms.Dependency(typeof(PersonalStorage_WinPhone))]
namespace ZKK_App.WinPhone
{
    public class PersonalStorage_WinPhone : IPersonalStorage
    {

        public bool CopyAssets()        
        {
            CopyAssetsAsync().GetAwaiter().GetResult();
            return true;
        }

        public bool isFilePresent(string fileName)
        {
            bool fileExists = true;
            Stream fileStream = null;
            StorageFile file = null;

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dfolder = local.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists).AsTask().Result;
            
            try
            {
                file = dfolder.GetFileAsync(fileName).AsTask().Result;
                fileStream = file.OpenStreamForReadAsync().GetAwaiter().GetResult();
                fileStream.Dispose();
            }
            catch (Exception)
            {
                // If the file dosn't exits it throws an exception, make fileExists false in this case 
                fileExists = false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }

            return fileExists;
        }

        async Task<bool> CopyAssetsAsync()
        {
            Windows.ApplicationModel.Package package = Windows.ApplicationModel.Package.Current;
            Windows.Storage.StorageFolder installedLocation = package.InstalledLocation;

            //String FilesDir = Path.Combine(installedLocation.Path, "Files");
            StorageFolder FilesFolder = installedLocation.GetFolderAsync("Files").AsTask().Result;
            //StorageFolder FilesFolder = installedLocation.GetFolderAsync("Files").GetResults();
            //var fFiles = FilesFolder.GetFilesAsync().GetResults();
            var fFiles = FilesFolder.GetFilesAsync().AsTask().Result;
            //var fFiles = fFilesT.GetResults();

            // On Windows Phone, Copy from Files/ to Data/
            //string[] files = Directory.GetFiles(FilesDir);
            

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dfolder = local.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists).AsTask().Result;
            
            foreach (var file in fFiles)
            {
                // Get the File Name
                string theFile = Path.GetFileName(file.Path);

                // Check whether File is existing (on existing: quit)
                if (this.isFilePresent(theFile))
                {
                    continue;
                }

                // Get a Stream of the target File
                var targetFile = dfolder.CreateFileAsync(theFile, CreationCollisionOption.ReplaceExisting).AsTask().Result;
                var ts = targetFile.OpenAsync(FileAccessMode.ReadWrite).AsTask().Result;
                Stream targetStream = ts.AsStream();

                // Get a Stream of the Source File
                var sourceFile = FilesFolder.GetFileAsync(theFile).AsTask().Result;
                var ss = sourceFile.OpenAsync(FileAccessMode.Read).AsTask().Result;
                Stream sourceStream = ss.AsStream();

                // Copy contents and close Files!
                sourceStream.CopyTo(targetStream);
                sourceStream.Close();
                //targetStream.Flush();
                targetStream.Close();

                /*
                // Read Data
                StreamReader sourceReader = new StreamReader(sourceStream);
                string data = sourceReader.ReadToEnd();

                // Write Data
                StreamWriter targetWriter = new StreamWriter(targetStream);
                targetWriter.Write(data);
                targetWriter.Flush();

                // Close Streams
                sourceReader.Close();
                targetWriter.Close();
                */

            }

            return true;

        }


        public string GetFullFilePath(string name)
        {
            //On Windows Phone it is in the Data-SubDir.
            //String fname = "Files/" + name;

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            string folder = Path.Combine(local.Path, "Data");

            String fname = Path.Combine(folder,name);
            return fname;
        }
    }
}