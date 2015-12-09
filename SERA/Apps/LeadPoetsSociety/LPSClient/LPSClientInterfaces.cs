using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using Thalamus;
using LPSClientMessages;
using Thalamus.BML;

namespace LPSClient
{

    public interface ILPSActions : IAction
    {
        void FinishedPerforming(string category, string subcategory);
    }
    public interface ILPSEvents : IPerception
    {
        void Perform(string category, string subcategory);
        void Stop();
    }


    public interface ILibraryActions : IAction
    {
        void ChangeLibrary(string newLibrary);
        void GetLibraries();
        void GetUtterances(string category, string subcategory);
    }

    public interface ILibraryEvents : IPerception
    {
        void LibraryList(string[] libraries);
        void LibraryChanged(string serialized_LibraryContents);
        void Utterances(string library, string category, string subcategory, string[] utterances);
    }


    public interface IFMLSpeech : IAction
    {
        //void PerformUtterance(string id, string utterance, string category);
        void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues);
        void CancelUtterance(string id);
    }

    public interface IFMLSpeechEvents : IPerception
    {
        void UtteranceStarted(string id);
        void UtteranceFinished(string id);
    }

    public interface ILPSClient : ILibraryEvents, IFMLSpeechEvents { }
    public interface ILPSClientPublisher : ILibraryActions, IFMLSpeech, ISpeakControlActions { }
    public interface ILPSClientRpc : ILPSActionsRPC, ILibraryEventsRPC, IXmlRpcProxy {}
}
