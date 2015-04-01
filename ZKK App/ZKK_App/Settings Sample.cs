using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App
{
    class Settings
    {
        /*
	 * About Files:
	 * 
	 * news.txt 
	 *      will be used for News storage
	 *      Structure: (where # is Delimiter)
	 *          Title#Content#Date
	 *      
	 * rooms
	 *      will be used as indexing structure for the rooms 
	 *      Structure: (# as Delimiter)
	 *          Title#imagename#rooms#comment
	 *      where image is the name of the image that should be loaded for this building
	 *      
	 * ak-zapf, ak-kif, ak-koma, ak-common
	 *      will be files listing the workshops of the conferences
	 *      Structure: (# as Delimiter) - may get enhanced
	 *          Title#Description
	 *          
	 * roomfinder
	 *      will be the file listing the rooms and the corresponding information about it
	 *      Structure: (# as Delimiter)
	 *          Roomname#Description
	 * 
	 * aklist-zapf, aklist-kif, aklist-koma, aklist-zkk
	 *      will be the files listing the Workshops and their room and time
	 *      Structure: (# as Delimiter) - may get enhanced
	 *          Day#Time#Title#room
	 *          
	 * policies
	 *      will be used for the policies and rules
	 *      Structure: (# as Delimiter) - may get enhanced (e.g. image?)
	 *          Title#|#Text
	 *          
	 * 
         */ 
        
        /// <summary>
        /// The Web URL of the News Page
        /// Will be used for WebView in Online Mode
        /// </summary>
        public static readonly string NewsWebPage = "";

        /// <summary>
        /// The URL of the news-textfile.
        /// Will be used for offline mode news
        /// </summary>
        public static readonly string NewsFileSource = "";
        
        /// <summary>
        /// The Delimiter of fields inside the newsfile
        /// </summary>
        public static readonly string TextFileDelimiter = "";

        /// <summary>
        /// The base URL for files to be downloaded in the app.
        /// For security reasons, downloads are limited to this domain
        /// </summary>
        public static readonly string AppFilesBaseUrl = "";
        
        /// <summary>
	/// The URL of the plan of the conference
	/// </summary>
        public static readonly string PlanTableUrl = "http://zkk.fsmpi.rwth-aachen.de/images/ablaufplan.jpg";

        /// <summary>
        /// The URL to the contents file used for updates
        /// This file only stores the names of further files to be downloaded
        /// </summary>
        public static readonly string AppContentsFileSource = AppFilesBaseUrl + "contents.txt";

        /// <summary>
        /// Inside Files, use the Escape Sequence instead of linebreaks.
        /// The App needs to re-transform thelinebreaks
        /// </summary>
        public static readonly string LineBreakEscape = "<br />";

        /// <summary>
        /// The list of files that are well-known and should be downloaded on updates
        /// </summary>
        public static readonly string[] ContentFiles = { "ak-zkk", "ak-zapf", "ak-kif", "ak-koma", "aklist-kif", "aklist-koma", "aklist-zapf", "aklist-zkk", "rooms", "roomfinder", "policies", "links" };

        public static readonly string PropertyUpdateDate = "";

        public static readonly string PropertyConferenceSelection = "";

        public static readonly string PropertyNewsSource = "";
    }
}
