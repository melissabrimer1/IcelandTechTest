using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace IcelandTechTest
{
    class InventoryManagement
    {
        
        public static string dataFile = @"..\..\..\INPUT_DATA.csv";
        static void Main(string[] args)
        {
            using (TextFieldParser parser = new TextFieldParser(dataFile))
            {
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = false;
                var csv = new StringBuilder();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string ItemName = fields[0];
                    int SellIn = Convert.ToInt32(fields[1]);
                    int Quality = Convert.ToInt32(fields[2]);

                    int TomorrowSellIn = GetTomorrowSellIn(ItemName, SellIn);
                    int TomorrowQuality = GetTomorrowQuality(ItemName, SellIn, Quality);

                    if (ItemName != "INVALID_ITEM")
                    {
                        var line = string.Format("{0},{1},{2}", ItemName, TomorrowSellIn, TomorrowQuality);
                        csv.AppendLine(line);
                        Console.WriteLine("Item Name : " + ItemName + "\n" +
                                          "Todays's days till/past sell by : " + SellIn + "\n" +
                                          "Tomorrow's days till/past sell by : " + TomorrowSellIn + "\n" +
                                          "Today's Quality Rating : " + Quality + "\n" +
                                          "Tomorrow's Quality Rating : " + TomorrowQuality);
                    }
                    else
                    {
                        var line = string.Format("{0}", "NO SUCH ITEM");
                        csv.AppendLine(line);
                        Console.WriteLine("NO SUCH ITEM");
                    }
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllText(path + "\\OutputData.csv",csv.ToString());
            }
            //DataTable inputData = GetData(data_file);
        }

        private static int GetTomorrowSellIn(string ItemName, int SellIn)
        {
            int TomorrowSellIn = SellIn;

            if (ItemName != "SOAP")
            {
                TomorrowSellIn--;
            }

            return TomorrowSellIn;
        }

        private static int GetTomorrowQuality(string ItemName, int SellIn, int Quality)
        {
            int TomorrowQuality = Quality;
            int TomorrowSellIn = SellIn;

            if (ItemName == "AGED_BRIE")
            {
                TomorrowQuality++;
            }
            else if (ItemName == "CHRISTMAS_CRACKERS")
            {
                TomorrowSellIn--;

                if (TomorrowSellIn <= 10  && TomorrowSellIn > 5)
                {
                    TomorrowQuality = Quality + 2;
                }
                else if (TomorrowSellIn <= 5 && TomorrowSellIn > -1)
                {
                    TomorrowQuality = Quality + 3;
                }
                else if (TomorrowSellIn > 10)
                {
                    TomorrowQuality++;
                }
                else
                {
                    TomorrowQuality = 0;
                }
            }
            else if (ItemName == "SOAP")
            {

            }
            else if (ItemName == "FROZEN_ITEM")
            {
                TomorrowQuality--;

                if (TomorrowSellIn < 0)
                {
                    TomorrowQuality--;
                }
            }
            else if (ItemName == "FRESH_ITEM")
            {
                TomorrowQuality = Quality - 2;

                if (TomorrowSellIn < 0)
                {
                    TomorrowQuality = TomorrowQuality - 2;
                }
            }

            if (TomorrowQuality < 0)
            {
                TomorrowQuality = 0;
            }
            else if (TomorrowQuality > 50)
            {
                TomorrowQuality = 50;
            }
            return TomorrowQuality;
        }

    }
}
