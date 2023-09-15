/*       
 *--------------------------------------------------------------------
 * 	   File name: Progra
 * 	Project name: Lab2_Advanced_C#
 *--------------------------------------------------------------------
 * Author’s name and email:	 Kyah Hanson - hansonkm@etsu.edu				
 *          Course-Section:  CSCI-2910-800
 *           Creation Date:	 9/12/2023		
 * -------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_Advanced_C_
{
    public class VideoGame : IComparable<VideoGame>
    {
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public double NA_Sales { get; set; }
        public double EU_Sales { get; set; }
        public double JP_Sales { get; set; }
        public double OtherSales { get; set; }
        public double GlobalSales { get; set; }

        // Default constructor
        public VideoGame()
        {
            Name = "No Name";
            Platform = "No Platform";
            Year = "No Year";
            Genre = "No Genre";
            Publisher = "No Publisher";
            NA_Sales = 0.0;
            EU_Sales = 0.0;
            JP_Sales = 0.0;
            OtherSales = 0.0;
            GlobalSales = 0.0;
        }

        // Copy constructor
        public VideoGame(VideoGame v)
        {
            Name = v.Name;
            Platform = v.Platform;
            Year = v.Year;
            Genre = v.Genre;
            Publisher = v.Publisher;
            NA_Sales = v.NA_Sales;
            EU_Sales = v.EU_Sales;
            JP_Sales = v.JP_Sales;
            OtherSales = v.OtherSales;
            GlobalSales = v.GlobalSales;
        }

        // Parameterized Constructor
        public VideoGame(string name, string platform, string year, string genre, string publisher, double nA_Sales, double eU_Sales, double jP_Sales, double otherSales, double globalSales)
        {
            Name = name;
            Platform = platform;
            Year = year;
            Genre = genre;
            Publisher = publisher;
            NA_Sales = nA_Sales;
            EU_Sales = eU_Sales;
            JP_Sales = jP_Sales;
            OtherSales = otherSales;
            GlobalSales = globalSales;
        }

        // Compares object to another object
        public int CompareTo(VideoGame gameObj)
        {
            if (gameObj == null)
            {
                return 1;
            }

            if (gameObj != null)
            {
                return Comparer<string>.Default.Compare(Name, gameObj.Name);
            }
            else
            {
                throw new ArgumentException("The object does not exist");
            }
        }

        // Print statemenr for object, sales were left out because they were unnecessary for the current reuirements
        public override string ToString()
        {
            string msg = $"{Name}";
            return msg;
        }
    }
}
