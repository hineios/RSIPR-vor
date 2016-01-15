using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using Skene;
using EmoteCommonMessages;
using Skene.Interfaces;
using VorApplication;
using System.Drawing;

namespace VorWOZ
{
    public interface VorWOZThalamusClient : Skene.Interfaces.ILibraryEvents, 
        EmoteCommonMessages.IFMLSpeechEvents, IVorActivityEvents { }
    public interface VorWOZThalamusPublisher: IThalamusPublisher, IVorActivity,
        EmoteCommonMessages.IFMLSpeech, Skene.Interfaces.ILibraryActions, EmoteCommonMessages.IGazeStateActions { }

    public class VorWOZClient : ThalamusClient, VorWOZThalamusClient
    {
        public VorWOZThalamusPublisher vorPublisher;
        VorWOZForm vorForm;
        /*
         * Wrapper class to avoid stupid mistakes such as calling methods that don't exist
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
            public void FillWord(string id)
            {
                publisher.FillWord(id);
            }
            public void GazeAtScreen(double x, double y)
            {
                publisher.GazeAtScreen(x, y);
            }
            public void GazeAtTarget(string targetName)
            {
                publisher.GazeAtTarget(targetName);
            }
            public void GetLibraries()
            {
                publisher.GetLibraries();
            }
            public void GetUtterances(string category, string subcategory)
            {
                publisher.GetUtterances(category, subcategory);
            }
            public void GlanceAtScreen(double x, double y)
            {
                publisher.GlanceAtScreen(x, y);
            }
            public void GlanceAtTarget(string targetName)
            {
                publisher.GlanceAtTarget(targetName);
            }
            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }
            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }
            public void StartGame()
            {
                publisher.StartGame();
            }
            public void StopGame()
            {
                publisher.StopGame();
            }
        }

        public VorWOZClient(VorWOZForm form, string master ="VorThalamusMaster") : base("VorWOZ", master)
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

        void IVorActivityEvents.GameStarted()
        {
            vorForm.GameState_Update("Game Started", System.Drawing.Color.YellowGreen, false, true);
            vorForm.FillWordButton_Update(true);
            
        }
        void IVorActivityEvents.GameEnded()
        {
            vorForm.GameState_Update("Game Ended", System.Drawing.Color.Firebrick, false, false);
            vorForm.CleanWordsNotCompleted();
            vorForm.FillWordButton_Update(false);
        }

        void IVorActivityEvents.WordAccepted(string id)
        {
            vorForm.FillWord_Update(id, true);
        }
        void IVorActivityEvents.WordDeclined(string id)
        {
            vorForm.FillWord_Update(id, false);
        }

        public void ApplicationLoaded(string gameconnection_serialized)
        {
            vorForm.GameState_Update("Application Loaded", Color.Orange, false, false);
            GameConnection gc = GameConnection.Deserialize(gameconnection_serialized);
            vorForm.AddWordsNotCompleted(gc.words);
        }

        public void ApplicationReady(string confederate, string participant)
        {
            vorForm.GameState_Update("Application Ready", Color.Gold, true, false);
            vorForm.UpdateTagValuesList(confederate, participant);

        }

        public void WordSelected(string id)
        {
            
        }

        public void TipNotSelected()
        {
            
        }
    }
}
