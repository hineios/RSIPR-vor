using UnityEngine;
using System.Collections;
using CookComputing.XmlRpc;
using LPSClientMessages;


/*
 * This class is very similar to ThalamusUnityClientService.
 * The service will deal with the messages that arrive from the Thalamus client, so that corresponds to implementing the ITUCActions interface.
 * */
public class LPSService : XmlRpcListenerService, ILPSActionsRPC, ILibraryEventsRPC
{
    /*
     * Again, declare a ThalamusUnity field and pass it in the constructor, so we can send the messages back to the MonoBehaviour class
     * */
    LPS thalamusUnity;
    public LPSService(LPS thalamusUnity)
    {
        this.thalamusUnity = thalamusUnity;
    }
    /*
     * Just forward the message to the ThalamusUnity instance
     * */

    public void LibraryChanged(string serialized_LibraryContents)
    {
        thalamusUnity.ExternalAction(() => thalamusUnity.LibraryChanged(serialized_LibraryContents));
    }

    public void LibraryList(string[] libraries)
    {
        thalamusUnity.ExternalAction(() => thalamusUnity.LibraryList(libraries));
    }

    public void Utterances(string library, string category, string subcategory, string[] utterances)
    {
        thalamusUnity.ExternalAction(() => thalamusUnity.Utterances(library, category, subcategory, utterances));
    }

    public void FinishedPerforming(string category, string subcategory)
    {
        thalamusUnity.ExternalAction(() => thalamusUnity.FinishedPerforming(category, subcategory));
    }
}
