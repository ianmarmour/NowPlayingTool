using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;


namespace WindowsFormsApplication1
{
    public static class LastFMApiClient
    {
        public static string GetNowPlaying(string username)
        {
            if (username != null)
            {
                var url = "http://ws.audioscrobbler.com/2.0/?method=user.getRecentTracks&user=" + username + "&api_key=1013e235a8dbf6f29bf5ead3e9b16feb";

                //Synch Consumption
                var syncClient = new WebClient();
                syncClient.Headers["Content-Type"] = "application/json;charset=UTF-8";

                var content = syncClient.DownloadString(url);

                using (XmlReader reader = XmlReader.Create(new StringReader(content)))
                {
                    reader.ReadToFollowing("track");
                    reader.MoveToFirstAttribute();
                    bool val;

                    if (reader.Value != "true")
                    {
                        return "No song currently playing.";
                    }

                    string output = reader.Value;
                    val = bool.Parse(output);
                    if (val == true)
                    {
                        reader.ReadToFollowing("artist");
                        reader.MoveToContent();
                        string artist = reader.ReadElementString();
                        reader.ReadToFollowing("name");
                        reader.MoveToContent();
                        string title = reader.ReadElementString();
                        string all = artist + " - " + title;
                        return all;
                    }

                    else
                    {
                        return "No valid song found.";
                    }
                }
            }

            else
            {
                return "No valid path selected.";
            }
        }
            
    }
}
