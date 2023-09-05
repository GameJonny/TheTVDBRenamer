using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Data;
using System.Xml;
using HtmlAgilityPack;
using System.Data.Common;
using System.Runtime.Remoting.Messaging;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using System.Globalization;
using System.Net;

namespace FileRename
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
                bool comp = false;
                Task.Run(async () => { await ReadFiles(); comp = true; });

                do
                {

                } while (!comp);

            }
			catch (Exception)
			{

				throw;
			}
        }

        private static async Task ReadFiles()
        {
            try {
                string[] url = {
                    "https://thetvdb.com/series/hunter-x-hunter-2011/seasons/official/1",
                    "https://thetvdb.com/series/hunter-x-hunter-2011/seasons/official/2",
                    "https://thetvdb.com/series/hunter-x-hunter-2011/seasons/official/3",
                };
                string dir = "X:\\Anime\\Hunter X Hunter 2011\\";

                using (DataTable dt = await readHTML(url))
                {
                    DataTable dt2 = dt.Clone();
                    foreach (DataRow dr in dt.Select("", "Column1 ASC"))
                        dt2.ImportRow(dr);

                    DirectoryInfo di = new DirectoryInfo(dir);
                    FileSystemInfo[] files = di.GetFileSystemInfos();
                    var orderedFiles = files.Where(f => File.GetAttributes(f.FullName) != FileAttributes.Directory).OrderBy(f => f.Name).ToArray();

                   var filenames = (from f in orderedFiles select f.FullName).ToArray();

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                       
                        string fileName = filenames[i];
                                              

                        DataRow episode = dt2.Rows[i];

                        string episodeRef = episode[0].ToString();
                        string episodeTitle = episode[1].ToString().Trim();
                        

                        foreach (var item in Path.GetInvalidFileNameChars())
                            episodeTitle = episodeTitle.Replace(item.ToString(), "");
                        foreach (var item in Path.GetInvalidPathChars())
                            episodeTitle = episodeTitle.Replace(item.ToString(), "");

                        int seasonNo = int.Parse(episodeRef.Substring(1, episodeRef.IndexOf("E")-1));

                        string newFolder = Path.Combine(dir, $"Season {seasonNo}");

                        string newName = $"{episodeRef.Trim()} - {episodeTitle.Trim()}{Path.GetExtension(fileName)}";

                       
                        if (!Directory.Exists(newFolder))
                        {
                            Directory.CreateDirectory(newFolder);
                        }

                        string moveLoc = Path.Combine(newFolder, newName);

                        File.Move(fileName, moveLoc);
                    }

                }

                

             
            }
            catch { }
        }

		private static async Task<DataTable> readHTML(string[] urls)
		{
			try {
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
                                    var txt = row.SelectNodes("td").Select(td => td.InnerText.Replace("&times;", "x").Replace("&#039;", "'").Replace("season finale", "").Replace("series finale","").Trim());
                                    table.Rows.Add(txt.ToArray());
                                }
                                   

                               
                            }
                        }
                    }
                }

                return table;
            } 
			catch { }
            return null;
		}

        private void test()
        {
            

# Add your subscription key and endpoint  
subscription_key = "replace_your_key"
endpoint = "https://api.cognitive.microsofttranslator.com"

# Add your location, also known as region. The default is global.  
# This is required if using a Cognitive Services resource.  
location = "global"


path = '/translate'
constructed_url = endpoint + path
  
params = {
                'api-version': '3.0',  
    'from': 'en',  
    'to': ['de', 'it']  
}
            constructed_url = endpoint + path


headers = {
                'Ocp-Apim-Subscription-Key': subscription_key,  
    'Ocp-Apim-Subscription-Region': location,  
    'Content-type': 'application/json',  
    'X-ClientTraceId': str(uuid.uuid4())
}

# You can pass more than one object in body.  
            body = [{
                'text': 'Hello World!'
            }]  
  
request = requests.post(constructed_url, params=params, headers = headers, json = body)
response = request.json()


print(json.dumps(response, sort_keys = True, ensure_ascii = False, indent = 4, separators = (',', ': ')))
        }
    }
}
