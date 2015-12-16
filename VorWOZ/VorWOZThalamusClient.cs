using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using Skene;
using EmoteCommonMessages;
using Skene.Interfaces;

namespace VorWOZ
{
    public interface VorWOZThalamusClient : Skene.Interfaces.ILibraryEvents, 
        EmoteCommonMessages.IFMLSpeechEvents { }
    public interface VorWOZThalamusPublisher: IThalamusPublisher, 
        EmoteCommonMessages.IFMLSpeech, Skene.Interfaces.ILibraryActions { }

    public class VorWOZClient : ThalamusClient, VorWOZThalamusClient
    {
        /*
         * Wrapper class to avoid stupid mistakes such as calling methods that aren't implemented by the dynamic publisher
         */
        private class VorWOZPublisher: VorWOZThalamusPublisher
        {
            dynamic publisher;

            public VorWOZPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
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

            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }

            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }
        }

        public VorWOZThalamusPublisher vorPublisher;
        VorWOZForm vorForm;

        public VorWOZClient(VorWOZForm form) : base("VorWOZ", "VorThalamusMaster")
        {
            SetPublisher<VorWOZThalamusPublisher>();
            vorPublisher = new VorWOZPublisher(Publisher);
            vorForm = form;
        }

        void ILibraryEvents.LibraryList(string[] libraries)
        {
            vorForm.UpdateLibrariesList(libraries);
        }

        void ILibraryEvents.LibraryChanged(string serialized_LibraryContents)
        {
            vorForm.ChangeLibrary(serialized_LibraryContents);
        }

        void ILibraryEvents.Utterances(string library, string category, string subcategory, string[] utterances)
        {
            throw new NotImplementedException();
        }

        void IFMLSpeechEvents.UtteranceStarted(string id)
        {
            vorForm.RobotStateChange("Speaking", System.Drawing.Color.Red);
        }

        void IFMLSpeechEvents.UtteranceFinished(string id)
        {
            vorForm.RobotStateChange("Idle", System.Drawing.Color.Green);
        }
    }
}
