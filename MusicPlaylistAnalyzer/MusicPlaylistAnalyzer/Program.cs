using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicPlaylistAnalyzer
{
    class Program
    {
        private static List<MusicModel> musicModels;
        static void Main(string[] args)
        {
            string musicplaylistfilepath = "";
            string reportfilepath = "";

            try
            {
                musicplaylistfilepath = args[0];
                reportfilepath = args[1];
            }
            catch(Exception)
            {
                Console.WriteLine("Please supply the file names to open this program.");
                Console.WriteLine("Windows Syntax: MusicPlaylistAnalyzer.exe <music_playlist_file_path> <report_file_path>");
                Console.WriteLine("MacOS Syntax: dotnet MusicPlaylistAnalyzer.dll <music_playlist_file_path> <report_file_path>");
            }

            if (File.Exists(musicplaylistfilepath))
            {
                //do something
                Console.WriteLine("Reading File Data...");
                musicModels = new List<MusicModel>();
                ReadMusicPlaylistFile(musicplaylistfilepath);
                Console.WriteLine("Generating Report...");
                if (GenerateReportFile(reportfilepath))
                {
                    Console.WriteLine("Report Generation Succeeded!");
                }
                else
                {
                    Console.WriteLine("Report Generation Failed!");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static bool GenerateReportFile(string reportfilepath)
        {
            //How many songs received 200 or more plays?
            var Plays = from musicmodels in musicModels where musicmodels.Plays >= 200 select musicmodels;
            //How many songs are in the playlist with the Genre of “Alternative”?
            var Genre = from musicmodels in musicModels where musicmodels.Genre == "Alternative" select musicmodels;
            //How many songs are in the playlist with the Genre of “Hip - Hop / Rap”?
            var HipHop = from musicmodels in musicModels where musicmodels.Genre == "Hip-Hop/Rap" select musicmodels;
            //What songs are in the playlist from the album “Welcome to the Fishbowl?”
            var Album = from musicmodels in musicModels where musicmodels.Album == "Welcome to the Fishbowl" select musicmodels;
            //What are the songs in the playlist from before 1970 ?
            var Year = from musicmodels in musicModels where musicmodels.Year < 1970 select musicmodels;
            //What are the song names that are more than 85 characters long?
            var NameLength = from musicmodels in musicModels where musicmodels.Name.Length > 85 select musicmodels;
            //What is the longest song ? (longest in Time)
            var LongestSong = from musicmodels in musicModels orderby musicmodels.Time descending select musicmodels;

            try
            {
                StreamWriter writer = new StreamWriter(reportfilepath, false);
                writer.Write("1. How many songs received 200 or more plays?" + "\n");
                writer.Write(Plays.Count().ToString() + "\n");
                writer.Write("2. How many songs are in the playlist with the Genre of “Alternative”?" + "\n");
                writer.Write(Genre.Count().ToString() + "\n");
                writer.Write("3. How many songs are in the playlist with the Genre of “Hip - Hop / Rap”?" + "\n");
                writer.Write(HipHop.Count().ToString() + "\n");
                writer.Write("4. What songs are in the playlist from the album “Welcome to the Fishbowl?”" + "\n");
                foreach (MusicModel model in Album)
                {
                    writer.Write(model.ToString() + "\n");
                }
                writer.Write("5. What are the songs in the playlist from before 1970 ?" + "\n");
                foreach (MusicModel model in Year)
                {
                    writer.Write(model.ToString() + "\n");
                }
                writer.Write("6. What are the song names that are more than 85 characters long?" + "\n");
                foreach (MusicModel model in NameLength)
                {
                    writer.Write(model.Name + "\n");
                }
                writer.Write("7. What is the longest song ? (longest in Time)" + "\n");
                writer.Write(LongestSong.First().ToString() + "\n");
                writer.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to write to file.");
                return false;
            }
            
            return true;
        }

        static void ReadMusicPlaylistFile(string musicPlaylistFilepath)
        {
            string line = "";
            bool firstLineDone = false;

            try
            {
                StreamReader reader = new StreamReader(musicPlaylistFilepath);
                while ((line = reader.ReadLine()) != null)
                {
                    if (firstLineDone)
                    {
                        var model = ProcessLine(line);
                        musicModels.Add(model);
                    }
                    else
                    {
                        firstLineDone = true;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not read file " + musicPlaylistFilepath);
            }
        }

        static MusicModel ProcessLine(string line)
        {
            var musicmodel = new MusicModel();
            int count = 0;

            var lines = line.Split('\t');
            foreach(string value in lines)
            {
                if(count == 0)
                {
                    musicmodel.Name = value;
                }
                else if(count == 1)
                {
                    musicmodel.Artist = value;
                }
                else if (count == 2)
                {
                    musicmodel.Album = value;
                }
                else if (count == 3)
                {
                    musicmodel.Genre = value;
                }
                else if (count == 4)
                {
                    musicmodel.Size = Int32.Parse(value);
                }
                else if (count == 5)
                {
                    musicmodel.Time = Int32.Parse(value);
                }
                else if (count == 6)
                {
                    musicmodel.Year = Int32.Parse(value);
                }
                else if (count == 7)
                {
                    musicmodel.Plays = Int32.Parse(value);
                }

                count++;
            }

            return musicmodel;
        }
    }
}
