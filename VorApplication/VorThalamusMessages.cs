using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VorApplication
{
    public interface IVorActivity : Thalamus.IAction
    {
        void StartGame();
        void StopGame();
        void FillWord(string id);
    }

    public interface IVorActivityEvents: Thalamus.IPerception
    {
        void ApplicationLoaded(string gameconnection_serialized);
        void ApplicationReady(string confederate, string participant);
        void GameStarted();
        void GameEnded();
        void TipNotSelected();
        void WordAccepted(string id);
        void WordDeclined(string id);
        void WordSelected(string id);
    }
}
