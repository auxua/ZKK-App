using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Xamarin.Forms;

namespace ZKK_App.aks
{
    /// <summary>
    /// A class for managing the Workshops
    /// they can be imported in the constructor from textfiles using Multithreading
    /// </summary>
    class AKDB
    {
        List<AKItem> zapf;
        List<AKItem> kif;
        List<AKItem> koma;
        List<AKItem> zkk;

        public AKDB()
        {
            zapf = new List<AKItem>();
            kif = new List<AKItem>();
            koma = new List<AKItem>();
            zkk = new List<AKItem>();

            // just create the paths
            string pathzapf = DependencyService.Get<IPersonalStorage>().GetFullFilePath("ak-zapf");
            string pathkif = DependencyService.Get<IPersonalStorage>().GetFullFilePath("ak-kif");
            string pathkoma = DependencyService.Get<IPersonalStorage>().GetFullFilePath("ak-koma");
            string pathzkk = DependencyService.Get<IPersonalStorage>().GetFullFilePath("ak-common");

            // use Threading for the import
            List<Thread> threads = new List<Thread>();

            threads.Add(new Thread(() => { import(pathzapf, zapf); }));
            threads.Add(new Thread(() => { import(pathkif, kif); }));
            threads.Add(new Thread(() => { import(pathkoma, koma); }));
            threads.Add(new Thread(() => { import(pathzkk, zkk); }));

            // Start every thread!
            foreach (Thread t in threads)
                t.Start();

            // Join the Threads for awaiting the completion
            foreach (Thread t in threads)
                t.Join();

            if (failed) throw new Exception("File not available!");
        }

        bool failed = false;

        /// <summary>
        /// import from source and store the AKItems in target
        /// </summary>
        /// <param name="source">the source file to read from</param>
        /// <param name="target">the target list to store the data in</param>
        private void import(string source, List<AKItem> target)
        {
            StreamReader sr;
            
            try
            {
                sr = new StreamReader(source);
            }
            catch (Exception)
            {
                failed = true;
                return;
            }

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            //target = new List<AKItem>();
            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 2)
                {
                    AKItem a = new AKItem();
                    a.Detail = words[1].Replace(Settings.LineBreakEscape,"\n");
                    a.Title = words[0];
                    target.Add(a);
                }
            }
            //Close File, finished!
            sr.Close();
        }

        public IEnumerable<AKItem> GetItems(Conferences conf)
        {
            switch (conf)
            {
                case Conferences.ALL:
                    return zkk;
                case Conferences.KIF:
                    return kif;
                case Conferences.KOMA:
                    return koma;
                case Conferences.ZAPF:
                    return zapf;
            }
            // just in case...
            return zkk;
        }
    }
}
