using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

/*
 * This assembly defines common messages and types that will be exchanged between Thalamus and Unity.
 * If you need to share some types between the Thalamus client and the Unity code, include that (enums, or serialized classes) in here
 * */

namespace LPSClientMessages
{
    public interface ILPSActionsRPC
    {
        [XmlRpcMethod]
        void FinishedPerforming(string category, string subcategory);
    }

    public interface ILPSEventsRPC
    {
        [XmlRpcMethod]
        void Perform(string category, string subcategory);

        [XmlRpcMethod]
        void Stop();

        [XmlRpcMethod]
        void SetLanguage(string language);
    }


    public interface ILibraryActionsRPC
    {
        [XmlRpcMethod]
        void ChangeLibrary(string newLibrary);

        [XmlRpcMethod]
        void GetLibraries();

        [XmlRpcMethod]
        void GetUtterances(string category, string subcategory);
    }

    public interface ILibraryEventsRPC
    {
        [XmlRpcMethod]
        void LibraryList(string[] libraries);

        [XmlRpcMethod]
        void LibraryChanged(string serialized_LibraryContents);

        [XmlRpcMethod]
        void Utterances(string library, string category, string subcategory, string[] utterances);
    }

    public interface IFMLSpeechRPC
    {
        /*[XmlRpcMethod]
        void PerformUtterance(string id, string utterance, string category);*/
        [XmlRpcMethod]
        void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues);
        [XmlRpcMethod]
        void CancelUtterance(string id);
    }

    public interface IFMLSpeechEventsRPC
    {
        [XmlRpcMethod]
        void UtteranceStarted(string id);
        [XmlRpcMethod]
        void UtteranceFinished(string id);
    }
}
