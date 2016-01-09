using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace VorApplication
{

    public interface VorApplicationThalamusClient { }
    public interface VorApplicationThalamusPublisher : IThalamusPublisher { }

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
        }

        VorApplicationThalamusPublisher vorPublisher;

        public VorApplicationClient() : base("VorApplication", "VorThalamusMaster")
        {
            SetPublisher<VorApplicationThalamusPublisher>();
            vorPublisher = new VorApplicationPublisher(Publisher);
        }
    }
}
