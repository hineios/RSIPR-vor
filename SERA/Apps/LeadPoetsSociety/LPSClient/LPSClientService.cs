using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using LPSClientMessages;
using Thalamus.BML;

namespace LPSClient
{
    public class LPSClientService : XmlRpcListenerService, ILPSEventsRPC, ILibraryActionsRPC
    {
        private LPSClient thalamusUnityClient;
        public LPSClientService(LPSClient thalamusUnityClient)
        {
            this.thalamusUnityClient = thalamusUnityClient;
        }

        //[XmlRpcMethod]
        public void Perform(string category, string subcategory)
        {
            Console.WriteLine("Received from Unity: SentFromUnityToThalamus({0}, {1})", category, subcategory);
            thalamusUnityClient.Perform(category, subcategory);
        }

        //[XmlRpcMethod]
        public void Stop()
        {
            Console.WriteLine("Received from Unity: Stop()");
            thalamusUnityClient.StopPerforming();
        }

        //[XmlRpcMethod]
        public void ChangeLibrary(string newLibrary)
        {
            Console.WriteLine("Received from Unity: ChangeLibrary({0})", newLibrary);
            thalamusUnityClient.ThalamusPublisher.ChangeLibrary(newLibrary);
        }

        //[XmlRpcMethod]
        public void GetLibraries()
        {
            Console.WriteLine("Received from Unity: GetLibraries()");
            thalamusUnityClient.ThalamusPublisher.GetLibraries();
        }

        //[XmlRpcMethod]
        public void GetUtterances(string category, string subcategory)
        {
            Console.WriteLine("Received from Unity: GetUtterances({0}, {1})", category, subcategory);
            thalamusUnityClient.ThalamusPublisher.GetUtterances(category, subcategory);
        }

        //[XmlRpcMethod]
        public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
        {
            thalamusUnityClient.ThalamusPublisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
        }

        //[XmlRpcMethod]
        public void CancelUtterance(string id)
        {
            thalamusUnityClient.ThalamusPublisher.CancelUtterance(id);
        }

        //[XmlRpcMethod]
        public void SetLanguage(string lang)
        {
            try
            {
                thalamusUnityClient.ThalamusPublisher.SetLanguage((SpeechLanguages)Enum.Parse(typeof(SpeechLanguages), lang));
            }
            catch (Exception e)
            {
                thalamusUnityClient.DebugException(e);
            }
        }
    }
}
