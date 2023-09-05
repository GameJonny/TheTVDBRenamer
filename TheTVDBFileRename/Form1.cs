using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Web;
using System.Web.UI.WebControls;
using System.Net;
using System.Diagnostics;

namespace TheTVDBFileRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
           
        }


        private async Task ReadFiles()
        {
            try
            {
                string key = Properties.Settings.Default.APIKey;
                if (string.IsNullOrEmpty(key) && Properties.Settings.Default.EnableTranslation)
                {
                    key = Interaction.InputBox("If you want to use the Translate feature," +
                        " enter your API Key", "Use translation?");

                    if (!string.IsNullOrEmpty(key))
                    {
                        Properties.Settings.Default.APIKey = key;
                        Properties.Settings.Default.Save();
                       
                       
                    }
                    else
                    {
                        Properties.Settings.Default.EnableTranslation = false;
                        Properties.Settings.Default.Save();
                        cbxTranlate.Checked = false;
                    }
                }

                if (string.IsNullOrEmpty(this.txtFolder.Text))
                {
                    MessageBox.Show("Specify a folder where the video files are kept", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtFolder.Focus(); 
                    return;
                }
                else
                {
                    if (!Directory.Exists(this.txtFolder.Text))
                    {
                        MessageBox.Show("Specified a folder does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.txtFolder.Focus();
                        return;
                    }
                }
                if (string.IsNullOrEmpty(this.txtUrls.Text))
                {
                    MessageBox.Show("Add 1 or more URLs from https://thetvdb.com/. 1 URL per line", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtUrls.Focus();
                    return;
                }

                string[] url = this.txtUrls.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string dir = this.txtFolder.Text;

                //start by getting the tv.db data from the web, which we are reordering to make sure we are in sequential order
                using (DataTable dt = await ReadHTML(url))
                {
                  //Next get the files in the folder 
                    DirectoryInfo di = new DirectoryInfo(dir);
                    FileSystemInfo[] files = di.GetFileSystemInfos();
                   
                    //remove firectories and order by file name
                    var orderedFiles = files.Where(f => File.GetAttributes(f.FullName) != FileAttributes.Directory).OrderBy(f => f.Name).ToArray();

                    //now get all of the files as string array
                    string[] filenames = (from f in orderedFiles select f.FullName).ToArray();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (filenames.Length < i)
                            continue;

                        //Get the current file name
                        string fileName = filenames[i];

                        //Get the episode data row
                        DataRow episode = dt.Rows[i];

                        //Episode ref will be S01E01 etc.
                        string episodeRef = episode[0].ToString();

                        //Episode title may need to translate
                        string episodeTitle = episode[1].ToString().Trim();

                        bool translate = false;
                        
                        if (Properties.Settings.Default.EnableTranslation)
                        {
                            var romaji = GetCharsInRange(episodeTitle, 0x0020, 0x007E);
                            if (!romaji.Any())
                            {
                                var hiragana = GetCharsInRange(episodeTitle, 0x3040, 0x309F);
                                if (!hiragana.Any())
                                {
                                    var katakana = GetCharsInRange(episodeTitle, 0x30A0, 0x30FF);
                                    if (!katakana.Any())
                                    {
                                        var kanji = GetCharsInRange(episodeTitle, 0x4E00, 0x9FBF);
                                        if (katakana.Any())
                                        {
                                            translate = true;
                                        }
                                    }
                                    else translate = true;
                                }
                                else translate = true;
                            }
                            else translate = true;


                            if (translate && !string.IsNullOrWhiteSpace(key))
                                episodeTitle = await TranslateText(episodeTitle, key);
                        }
                        



                        //get rid of any bad path chars
                        foreach (var item in Path.GetInvalidFileNameChars())
                            episodeTitle = episodeTitle.Replace(item.ToString(), "");
                        foreach (var item in Path.GetInvalidPathChars())
                            episodeTitle = episodeTitle.Replace(item.ToString(), "");

                        //get the season number
                        int seasonNo = int.Parse(episodeRef.Substring(1, episodeRef.IndexOf("E") - 1));

                        //Create the season folder
                        string newFolder = Path.Combine(dir, $"Season {seasonNo}");
                        if (!Directory.Exists(newFolder))
                            Directory.CreateDirectory(newFolder);

                        //Format the file name
                        string newName = $"{episodeRef.Trim()} - {episodeTitle.Trim()}{Path.GetExtension(fileName)}";

                        //Build the new path
                        string moveLoc = Path.Combine(newFolder, newName);

                        //Move the file with the new name to the season folder
                        File.Move(fileName, moveLoc);
                    }

                }




            }
            catch { }
        }

        private static IEnumerable<char> GetCharsInRange(string text, int min, int max)
        {
            return text.Where(e => e >= min && e <= max);
        }

        private async Task<DataTable> ReadHTML(string[] urls)
        {
            try
            {
                DataTable table = null;

                foreach (var url in urls)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpResponseMessage response = await client.GetAsync(url))
                        {
                            using (HttpContent content = response.Content)
                            {
                                string htmlCode = await content.ReadAsStringAsync();

                                HtmlDocument doc = new HtmlDocument();
                                doc.LoadHtml(htmlCode);
                                var headers = doc.DocumentNode.SelectNodes("//tr/th");


                                if (table == null)
                                {
                                    table = new DataTable();
                                    foreach (HtmlNode header in headers)
                                    {
                                        string txt = header.InnerText;
                                        txt = txt.Replace("&times;", "x");
                                        txt = txt.Replace("&#039;", "'");
                                        txt = txt.Replace("season finale", "").Trim();
                                        table.Columns.Add(txt); // create columns from th
                                                                // select rows with td elements 
                                    }

                                }

                                foreach (var row in doc.DocumentNode.SelectNodes("//tr[td]"))
                                {
                                    var txt = row.SelectNodes("td").Select(td => td.InnerText.Replace("&times;", "x").Replace("&#039;", "'").Replace("season finale", "").Replace("series finale", "").Trim());
                                    table.Rows.Add(txt.ToArray());
                                }



                            }
                        }
                    }
                }

                DataTable dt2 = table.Clone();
                foreach (DataRow dr in table.Select("", "Column1 ASC"))
                    dt2.ImportRow(dr);

                return dt2;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtFolder.Text = folderDlg.SelectedPath;
                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRename_Click(object sender, EventArgs e)
        {
            try
            {
                await ReadFiles();
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> TranslateText(string text, string APIKey)
        {
            try
            {
                //create client to handle request
                using (HttpClient client = new HttpClient())
                {
                    //build the request here
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("https://text-translator2.p.rapidapi.com/translate"),
                        Headers =
                        {
                            { "X-RapidAPI-Key", APIKey }, //Your API Key goes here https://rapidapi.com/dickyagustin/api/text-translator2/pricing
                            { "X-RapidAPI-Host", "text-translator2.p.rapidapi.com" },
                        },
                        Content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "source_language", "ja" }, //Source language Japanese
                            { "target_language", "en" }, //Source language English
                            { "text", text }, //Text to be translated
                        }),
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode(); //make sure we have a success
                        var body = await response.Content.ReadAsStringAsync();


                        //Deal with the response body
                        dynamic resp = JsonConvert.DeserializeObject<dynamic>(body);
                        if (resp != null)
                        {
                            if (resp.status != null && resp.status == "success")
                            {
                                if (resp.data != null && resp.data.translatedText != null)
                                    return resp.data.translatedText;
                            }
                        }
                    }
                }
                    

                return text;

            }
            catch (Exception x)
            {
                return text;

            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://rapidapi.com/dickyagustin/api/text-translator2/pricing");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cbxTranlate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.EnableTranslation = cbxTranlate.Checked;
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {

               
            }
        }
    }

   

}




