using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            CsvReader();
            Console.ReadKey();
        }

        

        public static void CsvReader()
        {
            //Reading from the csv vile using stream reader
            var reader = new StreamReader(File.OpenRead(@"C:\Users\JukePenguin\source\repos\ConsoleApp1\ConsoleApp1\call_info.csv"));

            //Characters to clean up the data for reading.
            char[] charsToTrim = { '"', ' ', ',' };

            //A list for each column. We will add to an array later? No we did not. 
            
            List<string> dateList = new List<string>();
            List<string> timeList = new List<string>();
            List<string> durationList = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                //We split the text with the comma and then fill our 3 lists after we trim em up. 
                //We change the characers in dates to make it fit the DateTime structure. Never used this though.

                var values = line.Split(',');
                
                dateList.Add(values[0].Trim(charsToTrim).Replace('-', '/'));
                timeList.Add(values[1].Trim(charsToTrim));
                durationList.Add(values[2].Trim(charsToTrim));
            }
            
            //I decided to put this in a method to clean up how it looked.
            CallInfo(timeList, durationList);

        }


        public static void CallInfo(List<string> timeList, List<string> durationList)
        {
            //These variables will write our final results.
            int maxCalls = 0;
            string callTime = null;
            
            for (int x = 1; x < timeList.Count; x++)
            {
                double firstDurationTime = Convert.ToDouble(durationList[x]);
                double firstStartTime = TimeSpan.Parse(timeList[x]).TotalSeconds;
                //Reset activeCalls for each new loop.
                int activeCalls = 0;

                for (int y = 1; y < timeList.Count; y++)
                {
                    double secondDurationTime = Convert.ToDouble(durationList[y]);
                    double secondStartTime = TimeSpan.Parse(timeList[y]).TotalSeconds;

                    //Here we check for overlap of calls. If true we had to our active calls. Basically 1.start < 2.end && 2.start < 1.end
                    if(firstStartTime <= secondStartTime + secondDurationTime  && secondStartTime < firstStartTime + firstDurationTime )
                    {
                        activeCalls++;
                    }
                    //If not true we go to check if our active call list is longer than our max.
                    else
                    {
                        if (activeCalls > maxCalls)
                        {
                            maxCalls = activeCalls;
                            callTime = timeList[y];
                        }
                        //We add a break here for the first call will no longer be able to reach any of the other calls.
                        break;
                    }
                }
            }
            //We get 313 at 11:03. My logic makes sense(to me), but there is one thing I did not take into account and that was what if a call is short. 
            //We still add that call to our total, but after a call disconnected it should be taken off the number.
            Console.Write("Max calls {0} occured at {1}.", maxCalls.ToString(), callTime);
            
        }
    }
}
