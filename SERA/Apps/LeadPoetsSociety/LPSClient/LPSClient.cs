using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CookComputing.XmlRpc;
using Thalamus;
using LPSClientMessages;

namespace LPSClient
{

    public class LPSClient : ThalamusClient, ILPSClient
    {
        /*
         * Usual Thalamus client Publisher
         * */
        private class ThalamusUnityClientPublisher : ILPSClientPublisher, IThalamusPublisher
        {
            dynamic publisher;
            public ThalamusUnityClientPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ChangeLibrary(string newLibrary)
            {
                publisher.ChangeLibrary(newLibrary);
            }

            public void GetLibraries()
            {
                publisher.GetLibraries();
            }

            public void GetUtterances(string category, string subcategory)
            {
                publisher.GetUtterances(category, subcategory);
            }

            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }

            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }

            public void SetLanguage(Thalamus.BML.SpeechLanguages lang)
            {
                publisher.SetLanguage(lang);
            }
        }

        

        /*
         * These are the settings for the local port (where this client receives XmLRpc messages from Unity), 
         * and the remote port (the remote part is running in Unity)
         * They are both generally ran in the same machine
         * */
        private int localPort = 7000;
        private int remotePort = 7001;
        private string remoteAddress = "localhost";


        //these fields are generically used for the XmlRpc server and client, and can just be copied for each solution
        private HttpListener listener;
        private bool serviceRunning;
        private bool shutdown;
        List<HttpListenerContext> httpRequestsQueue = new List<HttpListenerContext>();
        private Thread httpDispatcherThread;
        private Thread messageDispatcherThread;
        private string remoteUri = "";

        private string lastUtteranceId = "";
        private string lastCategory = "";
        private string lastSubcategory = "";

        //This is the XmlRpc client through which we will send messages to Unity
        ILPSClientRpc unityProxy;



        /*
         * as usual, define the Thalamus publisher here. 
         * make it either internal or public, so you can access it from ThalamusUnityClientService
         * */
        internal ILPSClientPublisher ThalamusPublisher;

        public LPSClient(string character = "")
            : base("ThalamusUnityClient", character)
        {
            //as usual, set the Thalamus publisher
            SetPublisher<ILPSClientPublisher>();
            ThalamusPublisher = new ThalamusUnityClientPublisher(Publisher);


            /*
             * create and start two threads: one is receiving the XmlRpc requests via Http and placing them on a queue;
             * the other is a working thread that actially dispatches the messages that are queued.
             * these thus provide an XmlRpc server, to receive messages sent from Unity
             * */
            httpDispatcherThread = new Thread(new ThreadStart(HttpDispatcher));
            messageDispatcherThread = new Thread(new ThreadStart(MessageDispatcher));
            httpDispatcherThread.Start();
            messageDispatcherThread.Start();


            //create and initialize a XmLRpc proxy, which is in this side, the XmlRpc Client, through which we send messages to Unity
            remoteUri = String.Format("http://{0}:{1}", remoteAddress, remotePort);
            unityProxy = XmlRpcProxyGen.Create<ILPSClientRpc>();
            unityProxy.Expect100Continue = true;
            unityProxy.Timeout = 2000;
            unityProxy.Url = remoteUri;
            Debug(Name + " endpoint set to " + remoteUri);

            /*EventInformationChanged += (() =>
            {
                RegisterExtendedInterface("ISpeakActions.Speak");
            });*/
        }

        internal void StopPerforming()
        {
            ThalamusPublisher.CancelUtterance(lastUtteranceId);
        }

        internal void Perform(string category, string subcategory)
        {
            lastUtteranceId = Guid.NewGuid().ToString();
            lastCategory = category;
            lastSubcategory = subcategory;
            ThalamusPublisher.PerformUtteranceFromLibrary(lastUtteranceId, category, subcategory, new string[] { }, new string[] { });
        }


        void ILibraryEvents.LibraryList(string[] libraries)
        {
            try
            {
                Debug("Sent to Unity: LibraryList");
                unityProxy.LibraryList(libraries);
            }
            catch (Exception e)
            {
                DebugException(e);
            }
        }

        void ILibraryEvents.LibraryChanged(string serialized_LibraryContents)
        {
            try
            {
                Debug("Sent to Unity: LibraryChanged");
                unityProxy.LibraryChanged(serialized_LibraryContents);
            }
            catch (Exception e)
            {
                DebugException(e);
            }
        }

        void ILibraryEvents.Utterances(string library, string category, string subcategory, string[] utterances)
        {
            try
            {
                Debug("Sent to Unity: Utterances");
                unityProxy.Utterances(library, category, subcategory, utterances);
            }
            catch (Exception e)
            {
                DebugException(e);
            }
        }

        void IFMLSpeechEvents.UtteranceStarted(string id)
        {
            try
            {
                /*Debug("Sent to Unity: UtteranceStarted");
                unityProxy.UtteranceStarted(id);*/
            }
            catch (Exception e)
            {
                DebugException(e);
            }
        }

        void IFMLSpeechEvents.UtteranceFinished(string id)
        {
            try
            {
                try
                {
                    if (id == lastUtteranceId)
                    {
                        Debug("Sent to Unity: FinishedPerforming");
                        unityProxy.FinishedPerforming(lastCategory, lastSubcategory);
                    }
                    else
                    {
                        DebugError("UtteranceFinished id '{0}' does not match lastUtteranceId '{1}'!", id, lastUtteranceId);
                        unityProxy.FinishedPerforming(lastCategory, lastSubcategory);
                    }
                }
                catch (Exception e)
                {
                    DebugException(e);
                }
            }
            catch (Exception e)
            {
                DebugException(e);
            }
        }

        #region These methods provide the XmlRpc server, by handling the incomming messages that arrive from Unity via XmlRpc. It is generic and shouldn't need modifications.
        /*
         * You don't really need to understand or modify this code to use this :)
         * */


        //this processes each individual message request received from XmlRpc
        public void ProcessMessage(object oContext)
        {
            try
            {
                /*
                 * If you do modify the code and change the names of your classes, you might need to update just this line here
                 * Whenever any message is received via XmlRpc, it is processed and then routed to ThalamusUnityClientService.
                 * It is there that we actually do something with the message we received.
                 * */
                XmlRpcListenerService svc = new LPSClientService(this);
                svc.ProcessRequest((HttpListenerContext)oContext);
            }
            catch (Exception e)
            {
                DebugException(e);
            }
        }


        //carefully cleans and shuts down the servers and threads
        public override void Dispose()
        {
            shutdown = true;

            try
            {
                if (listener != null) listener.Stop();
            }
            catch { }

            try
            {
                if (httpDispatcherThread != null) httpDispatcherThread.Join();
            }
            catch { }

            try
            {
                if (messageDispatcherThread != null) messageDispatcherThread.Join();
            }
            catch { }

            base.Dispose();
        }

        //listens to http requests (XmlRpc runs via HTTP) and places them in a queue, so that more requests can be received while others are being processed
        public void HttpDispatcher()
        {
            while (!serviceRunning)
            {
                try
                {
                    Debug("Attempt to start XmlRpc service on port '" + localPort + "'");
                    listener = new HttpListener();
                    listener.Prefixes.Add(string.Format("http://*:{0}/", localPort));
                    listener.Start();
                    Debug("XMLRPC Listening on " + string.Format("http://*:{0}/", localPort));
                    serviceRunning = true;
                }
                catch
                {
                    localPort++;
                    Debug("Port unavaliable.");
                    serviceRunning = false;
                }
            }

            while (!shutdown)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    lock (httpRequestsQueue)
                    {
                        httpRequestsQueue.Add(context);
                    }
                }
                catch (Exception e)
                {
                    DebugException(e);
                    serviceRunning = false;
                    if (listener != null)
                        listener.Close();
                }
            }
            Debug("Terminated HttpDispatcher");
        }

        // checks the queued xmlrpc requests, grabs their contexts, and dispatches the processing of each individual message to a separate thread
        public void MessageDispatcher()
        {
            while (!shutdown)
            {
                bool performSleep = true;
                try
                {
                    if (httpRequestsQueue.Count > 0)
                    {
                        performSleep = false;
                        List<HttpListenerContext> httpRequests;
                        lock (httpRequestsQueue)
                        {
                            httpRequests = new List<HttpListenerContext>(httpRequestsQueue);
                            httpRequestsQueue.Clear();
                        }
                        foreach (HttpListenerContext r in httpRequests)
                        {
                            (new Thread(new ParameterizedThreadStart(ProcessMessage))).Start(r);
                            performSleep = false;
                        }
                    }


                }
                catch (Exception e)
                {
                    DebugException(e);
                }
                if (performSleep) Thread.Sleep(10);
            }
            Debug("Terminated MessageDispatcher");
        }

        #endregion



        
    }
}
