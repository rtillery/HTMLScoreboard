using System;
using System.Collections.Specialized;
using System.IO;

namespace WpfApp1
{
    public class HTMLTemplate
    {
        static public void Fill(String templateFilename, String htmlFilename_, NameValueCollection pairs)
        {
            String content;
            using (var input = new StreamReader(templateFilename))
            {
                content = input.ReadToEnd();
            }
            foreach (var key in pairs.AllKeys)
            {
                content = content.Replace("[" + key + "]", pairs[key]);
            }
            using (var output = new StreamWriter(htmlFilename_))
            {
                output.Write(content);
            }
        }
    }
}