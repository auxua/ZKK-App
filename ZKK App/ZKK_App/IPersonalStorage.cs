using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ZKK_App
{
    public interface IPersonalStorage
    {
        /// <summary>
        /// Copies all known (coded) Assets to the personal Storage
        /// </summary>
        /// <returns>false, if an error occured</returns>
        bool CopyAssets();

        /// <summary>
        /// For a given File name, get the full File path depending on the OS
        /// </summary>
        /// <param name="name">The file name</param>
        /// <returns>The full file path, not checked whether the File exists!</returns>
        String GetFullFilePath(string name);
    }
}
