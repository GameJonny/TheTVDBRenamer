This project was created to help me rename files on my plex server. 

**Disclaimer - The developer does not take responsibility for any loss of data or damages to hardware/software caused by the installation and/or use of this application. Backup files before using the renamer. **

The application takes two values. The path for the series that you want to rename and a list of URLs attaining to the series.

The path should contain all video files in sequential order. Sub folders will be skipped.

To get URLS, go to https://thetvdb.com/ and find your series, then click on seasons and pick your preferred order.
Copy each season url and paste them into the field making sure that you are observing 1 url per line

The Application will create a season folder based on the episode ID and move all renamed files there. For example, if your episode ID = S01E01
then the subfolder Season 1 will be created and the file will be named S01E01 - xxxxxxxxxx Where "xxxxxxxxxx" is the episode title.

With regards to Episode titles, the program will attempt to translate the title from Japanese to English if configured. 

To configure this: 

1. Go to https://rapidapi.com/dickyagustin/api/text-translator2/pricing
2. Subscribe to the free teir (or a paid one if you want)
3. Click Apps on the top right of the page
4. On the left of the page click my apps and expand default-application_xxxxx
5. Click Authorization and copy the auth key
6. When prompted, paste the auth key into the application and press ok

You will be prompted for this when you first try and rename files. The application will save this value for future use.

The translation API is only called when Japanese characters are found in the string.

The field can be left blank or uncheck the translation box.

NuGet used 
Newtonsoft.Json
https://www.newtonsoft.com/json

HtmlAgilityPack
https://html-agility-pack.net/

Quick code refs

Translation code
```
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
```


Detect Japanese chars
```
 private static IEnumerable<char> GetCharsInRange(string text, int min, int max)
 {
      return text.Where(e => e >= min && e <= max);
 }

```

Useage:
```
bool translate = false;

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

//if translate = true then japanese chars found
```

Read HTML and output a DataTable

```
 private async Task<DataTable> readHTML(string url)
    {
        try
        {
            DataTable table = null;
         
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


                        table = new DataTable();
                        foreach (HtmlNode header in headers)
                        {
                            string txt = header.InnerText;
                                
                            table.Columns.Add(txt); // create columns from th
                                                    // select rows with td elements 
                        }
                            

                        foreach (var row in doc.DocumentNode.SelectNodes("//tr[td]"))
                        {
                            var txt = row.SelectNodes("td").Select(td => td.InnerText.Replace("&times;", "x").Replace("&#039;", "'").Replace("season finale", "").Replace("series finale", "").Trim());
                            table.Rows.Add(txt.ToArray());
                        }



                    }
                }
            }
            

           

            return table;
        }
        catch (Exception)
        {

            throw;
        }

    }
```
