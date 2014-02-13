using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
namespace Downloader
{
    public partial class Interface : Form
    {
        DownloadHandler downloadHandler;
        public delegate void updateProgress(int i);
        public updateProgress updateProgressDelegate;

        public Interface()
        {
            InitializeComponent();
            downloadHandler = new DownloadHandler(this);
            updateProgressDelegate = new updateProgress(updateProgressMethod);
        }

        public void updateProgressMethod(int i)
        {
            progDownload.Value = i;
            if (i == progDownload.Maximum)
            {
                btnAdd.Enabled = true;
                btnDownload.Enabled = true;
                MessageBox.Show(
                    "Downloads are completed",
                    "Notification",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            int items_count = lstLinks.Items.Count;
            string[] items = new string[items_count];

            if (items_count <= 0)
            {
                MessageBox.Show("Provide me a link please!");
                return;
            }

            for(int i = 0; i < items_count; i++)
            {
                items[i] = (string) lstLinks.Items[i];
            }

            progDownload.Maximum = items_count;
            btnAdd.Enabled = false;
            btnDownload.Enabled = false;
            downloadHandler.startDownload(items_count, items);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //checks for validity of the url then adds it to listbox
            if (Download.isValidUrl(txtLink.Text))
            {
                lstLinks.Items.Add(txtLink.Text);
            }
            else
            {
                MessageBox.Show("The URL is invalid", "Oops!");
            }
                  
        }
    }
}
