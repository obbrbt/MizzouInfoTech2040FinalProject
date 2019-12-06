using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlaylistAnalyzer
{
    class MusicModel
    {
        public string Name { get; set;}
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int Size { get; set; }
        public int Time { get; set; }
        public int Year { get; set; }
        public int Plays { get; set; }

        public override string ToString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}",
                Name, Artist, Album, Genre, Size, Time, Year, Plays);
        }
    }
}
