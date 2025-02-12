using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace RealFeel
{
    internal class API
    {
        private HttpClient client = new HttpClient();
        internal bool Busy;

        internal void Send(int Strength, int SensorID = 0, bool IsPulse = false)
        {
            if (Busy)
            {
                Console.WriteLine("Busy..");
                return;
            }

            //Task.Run(() =>
            {
                Busy = true;

                Console.WriteLine($"Sending Strength: {Strength}");

                try
                {
                    _ = client.PostAsync("http://127.0.0.1:9020/vibrate", new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Strength", Strength.ToString()),
                        new KeyValuePair<string, string>("SensorID", SensorID.ToString()),
                        new KeyValuePair<string, string>("IsPulse", (Strength != 0 && Strength != 20 && IsPulse).ToString()), // No strength or full should be constant, for nothing or climax
                    }), new CancellationTokenSource(900).Token).Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"Sent Strength: {Strength}");

                Busy = false;
            }//);
        }

        public class RealFeelRequest
        {
            public int Strength;
            public int SensorID; // Default Is 4 Max. User Can Adjust To More.
            public bool IsPulse;
        }
    }
}
