using Skene.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VorWOZ
{
    public partial class VorWOZForm : Form
    {
        VorWOZClient tclient;
        LibraryInfo lib;
        string lastUtterance;
        string[] tagNames = new string[] {};
        string[] tagValues = new string[] {};

        public VorWOZForm()
        {
            InitializeComponent();
            tclient = new VorWOZClient(this);
        }

        private void PanicSendButton_Click(object sender, EventArgs e)
        {
            object msg = PanicSentences.SelectedItem;
            if( msg != null)
            {
                tclient.vorPublisher.PerformUtterance("", msg.ToString(), "");
                PanicSentences.ClearSelected();
            }else if(PanicTextBox.TextLength != 0)
            {
                tclient.vorPublisher.PerformUtterance("", PanicTextBox.Text.ToString(), "");
            }
        }

        private void ChangeLibraryButton_Click(object sender, EventArgs e)
        {
            object lib = LibrariesList.SelectedItem;
            if(lib != null)
            {
                tclient.vorPublisher.ChangeLibrary(lib.ToString());
                LibrarySelected.Text = "changing...";
                LibrarySelected.ForeColor = Color.Orange;
            }
        }

        private void LibrariesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedLib = LibrariesList.SelectedItem;
            if (selectedLib != null) { 
                if (selectedLib.ToString().Equals(LibrarySelected.Text))
                {
                    ChangeLibraryButton.Enabled = false;
                } else
                {
                    ChangeLibraryButton.Enabled = true;
                }
            }
        }

        private void GetLibrariesButton_Click(object sender, EventArgs e)
        {
            LibrariesList.Items.Clear();
            tclient.vorPublisher.GetLibraries();
        }

        private void LibCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            LibSubCategories.Items.Clear();
            object selectedCat = LibCategories.SelectedItem;
            if (selectedCat != null)
            {
                foreach (string sub in lib.Categories[selectedCat.ToString()])
                {
                    LibSubCategories.Items.Add(sub);
                }
            }
            PerformUtterance.Enabled = false;
        }
        
        private void PerformUtterance_Click(object sender, EventArgs e)
        {
            object cat = LibCategories.SelectedItem;
            object subCat = LibSubCategories.SelectedItem;
            lastUtterance = new Guid().ToString();

            if ( cat != null & subCat != null)
            {
                tclient.vorPublisher.PerformUtteranceFromLibrary(lastUtterance, cat.ToString(), subCat.ToString(), tagNames, tagValues);
            }
        }

        private void GazeConfederate_Click(object sender, EventArgs e)
        {
            tclient.vorPublisher.GazeAtTarget("person2");
        }

        private void GlanceConfederate_Click(object sender, EventArgs e)
        {
            tclient.vorPublisher.GlanceAtTarget("person2");
        }

        private void GazeParticipant_Click(object sender, EventArgs e)
        {
            tclient.vorPublisher.GazeAtTarget("person");
        }
        
        private void GlanceParticipant_Click(object sender, EventArgs e)
        {
            tclient.vorPublisher.GlanceAtTarget("person");
        }

        private void GazeElsewhere_Click(object sender, EventArgs e)
        {
            tclient.vorPublisher.GazeAtTarget("random");
        }

        private void GazeGame_Click(object sender, EventArgs e)
        {
            tclient.vorPublisher.GazeAtTarget("clicks");
        }

        private void UpdateTagValues_Click(object sender, EventArgs e)
        {
            tagNames = TagNamesList.Text.Split('\n');
            tagValues = TagValuesList.Text.Split('\n');
        }
        private void VorWOZForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tclient.Dispose();
        }

        private void LibSubCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selected = LibSubCategories.SelectedItem;
            if (selected != null)
            {
                PerformUtterance.Enabled = true;
            }
            else
            {
                PerformUtterance.Enabled = false;
            }
        }



        /*
         * Public methods to allow Updates to the interface
         */
        public void RobotStateChange(string text, Color color)
        {
            this.Invoke((MethodInvoker)delegate {
                RobotState.Text = text;
                RobotState.ForeColor = color;
            });
        }

        public void ThalamusClientChangeState(string text, Color color)
        {
            this.Invoke((MethodInvoker)delegate {
                ThalamusState.Text = text;
                ThalamusState.ForeColor = color;
            });
        }

        public void ChangeLibrary(string serialized_LibraryContents)
        {
            this.Invoke((MethodInvoker)delegate {
                lib = LibraryInfo.DeserializeFromJson(serialized_LibraryContents);

                LibrarySelected.Text = lib.LibraryName;
                LibrarySelected.ForeColor = Color.Green;
                ChangeLibraryButton.Enabled = false;

                LibCategories.Items.Clear();
                LibSubCategories.Items.Clear();

                foreach(string key in lib.Categories.Keys)
                {
                    LibCategories.Items.Add(key);
                }
            });
        }

        public void UpdateLibrariesList(string[] libraries)
        {
            this.Invoke((MethodInvoker)delegate {
                foreach (string str in libraries)
                {
                    LibrariesList.Items.Add(str);
                }
            });
        }

        
    }
}
