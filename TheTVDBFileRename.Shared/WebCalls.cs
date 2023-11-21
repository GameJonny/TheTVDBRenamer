using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTVDBFileRename.Shared
{
    public class WebCalls
    {
        public async Task<DataTable> ReadHTML(string[] urls)
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

        public double Add2Nos(double a, double b)
        { return a + b; }

    }
}
