using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Downloader
{
    class Download
    {
        //A fancy get/set/initialize variable thingy from .NET which I approve.
        public string downloadLink {get; private set;}

        public Download(String link) 
        {
            downloadLink = link;
        }

        //This method checks if the url is valid
        public static bool isValidUrl(string url)
        {
            if (url == null)
                return false;

            try
            {
                new Uri(url);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public string deriveFileName() 
        {
            try
            {
                Uri url = new Uri(this.downloadLink);

                /*
                 * get path from the file if it's not a file, find it in the url. 
                 * If everything fails return null. If it's null it will be handled in the backgroundworker
                 */
                if (url.IsFile)
                {
                    return Path.GetFileName(url.LocalPath);
                }
                else
                {
                    int last_pos_slash = downloadLink.LastIndexOf('/') + 1;
                    return downloadLink.Substring(last_pos_slash, downloadLink.Length - last_pos_slash);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null; 
            }
        }

        
    }
}
