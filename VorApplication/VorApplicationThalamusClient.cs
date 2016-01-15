using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using System.Net.Http;

namespace VorApplication
{

    public interface VorApplicationThalamusClient : IVorActivity { }
    public interface VorApplicationThalamusPublisher : IThalamusPublisher , IVorActivityEvents { }

    public class VorApplicationClient : ThalamusClient, VorApplicationThalamusClient
    {
        /*
         * Wrapper class to avoid stupid mistakes such as calling methods that aren't implemented by the dynamic publisher
         */
        private class VorApplicationPublisher : VorApplicationThalamusPublisher
        {
            dynamic publisher;

            public VorApplicationPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ApplicationLoaded(string gameconnection_serialized)
            {
                publisher.ApplicationLoaded(gameconnection_serialized);
            }
            public void ApplicationReady(string confederate, string participant)
            {
                publisher.ApplicationReady(confederate, participant);
            }
            public void GameEnded()
            {
                publisher.GameEnded();
            }
            public void GameStarted()
            {
                publisher.GameStarted();
            }
            public void TipNotSelected()
            {
                publisher.TipNotSelected();
            }
            public void WordAccepted(string id)
            {
                publisher.WordAccepted(id);
            }
            public void WordDeclined(string id)
            {
                publisher.WordDeclined(id);
            }
            public void WordSelected(string id)
            {
                publisher.WordSelected(id);
            }
        }

        public VorApplicationThalamusPublisher vorPublisher;
        string httpserver = "http://146.193.224.186:3000/";
        public VorApplicationClient(string master = "VorThalamusMaster") : base("VorApplication", master)
        {
            SetPublisher<VorApplicationThalamusPublisher>();
            vorPublisher = new VorApplicationPublisher(Publisher);
        }
        public void PassServer(string httpserver, int port)
        {
            this.httpserver = "http://" + httpserver + ":" + port +"/";
        }
        public void SendPOST(string url, string json1, string json2)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(httpserver);
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>(json1, json1)
                });
                    var result = client.PostAsync(url, content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to POST to {0}{1}", httpserver, url);
                    Console.WriteLine(e.ToString());
                }
                
            }
        }
       
        void IVorActivity.StartGame()
        {
            Console.WriteLine("Thalamus IAction StartGame");
            SendPOST("start/changeStat", "active", "true");
            
            vorPublisher.GameStarted();
        }
        void IVorActivity.StopGame()
        {
            Console.WriteLine("Thalamus IAction StopGame");
            SendPOST("start/changeStat", "active", "false");
            vorPublisher.GameEnded();
        }
        void IVorActivity.FillWord(string id)
        {
            Console.WriteLine("Thalamus IAction FillWord {0}", id);
            SendPOST("words", "id", id);
        }
    }
}
