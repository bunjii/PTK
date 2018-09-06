using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grasshopper.GUI;


namespace PTK
{
    public partial class PSOForm : Form
    {
        private ParticleSwarmOptimizationComponent comp;

        public PSOForm()
        {
            InitializeComponent();
        }
        public PSOForm(ParticleSwarmOptimizationComponent _comp)
        {
            InitializeComponent();
            Owner = Grasshopper.Instances.DocumentEditor;
            comp = _comp;
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (TransitionOperatingState())
            {
                await Task.Run(() =>
                {
                    ParticleSwarmOptimization.PSORun();
                });
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            TransitionStopingState();
        }

        private async void ResumeButton_Click(object sender, EventArgs e)
        {
            if (TransitionOperatingState())
            {
                if (!ParticleSwarmOptimization.ResumeRun())  //Perform resume processing
                {
                    //Resume Failure
                    //FitnessChart.Series["AverageChart"].Points.Clear();         //Reset graph of average value
                    //FitnessChart.Series["MaxMinChart"].Points.Clear();          //Reset graph of Min-Max value
                    //FitnessChart.Series["BestFitnessUpdate"].Points.Clear();    //Reset graph of BestFitness value
                    //RestrictionFitnessChart.Series["AverageChart"].Points.Clear();          //Reset graph of average value
                    //RestrictionFitnessChart.Series["MaxMinChart"].Points.Clear();           //Reset graph of Min-Max value
                    //RestrictionFitnessChart.Series["BestFitnessUpdate"].Points.Clear();     //Reset graph of BestFitness value
                }

                await Task.Run(() =>
                {
                    ParticleSwarmOptimization.PSORun();    //Run in a separate thread
                });
            }
        }

        private void OpenSaveFolder_Click(object sender, EventArgs e)
        {
            SaveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(SaveFolderTextBox.Text);
            SaveFileDialog.FileName = System.IO.Path.GetFileNameWithoutExtension(SaveFolderTextBox.Text);
            //DialogResult dr = SaveFolderBrowserDialog.ShowDialog();     //Show folder selection dialog
            DialogResult dr = SaveFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                SaveFolderTextBox.Text = SaveFileDialog.FileName;
                //SaveFolderTextBox.Text = SaveFolderBrowserDialog.SelectedPath;  //Reflect selected folder in text box
                comp.SavePath = GetSavePath();                                  //Reflected in component variables
            }
        }

        //-------Read settings from Comp
        public void SetOptionFromComp()
        {
            OwnerCanvasEnable(false);

            ParticleNumBox.Value = comp.ParticleNum;
            WBox.Value = (decimal)comp.W;
            C1Box.Value = (decimal)comp.C1;
            C2Box.Value = (decimal)comp.C2;
            //MutationRateBox.Value = (decimal)comp.MutationRate;
            //if (comp.IsMinimize)
            //    MinRadioButton.Checked = true;
            //else
            //    MaxRadioButton.Checked = true;
            //MaxGenerationCheck.Checked = comp.IsEnableMaxGene;
            //MaxGenerationBox.Value = comp.MaxGeneration;
            //ContinuationCheck.Checked = comp.IsEnableContinuation;
            //ContinuationBox.Value = comp.ContinuationNum;
            SaveFolderTextBox.Text = comp.SavePath;
        }


        public delegate void WriteLogDelegate(String _logText);
        public void WriteLog(String _logText)
        {
            if (this.InvokeRequired)    //Use Invoke to securely access the form when calling from another thread
            {
                WriteLogDelegate d = new WriteLogDelegate(WriteLog);
                this.Invoke(d, _logText);
                return;
            }
            else
            {
                OutputLogBox.AppendText(_logText + Environment.NewLine);
            }
        }


        //-------Changed State for algorithm running
        public bool TransitionOperatingState()
        {
            if (ParticleSwarmOptimization.ResetPSO())
            {
                ParticleSwarmOptimization.IsAbort = false;
                StopButton.Enabled = true;
                StartButton.Enabled = false;
                ResumeButton.Enabled = false;
                ConfigurationGroup.Enabled = false;
                //ExitConditionsGroup.Enabled = false;
                SaveGenerationGroup.Enabled = false;
                return true;
            }
            return false;
        }

        //-------Changed State for algorithm stopping
        public delegate void TransitionStopingStateDelegate();
        public void TransitionStopingState()
        {
            if (this.InvokeRequired)    //Use Invoke to securely access the form when calling from another thread
            {
                TransitionStopingStateDelegate d = new TransitionStopingStateDelegate(TransitionStopingState);
                this.Invoke(d);
                return;
            }
            else
            {
                ParticleSwarmOptimization.IsAbort = true;
                StopButton.Enabled = false;
                StartButton.Enabled = true;
                ResumeButton.Enabled = true;
                ConfigurationGroup.Enabled = true;
                //ExitConditionsGroup.Enabled = true;
                SaveGenerationGroup.Enabled = true;
            }
        }

        //-------Function to get a value from a form
        public int GetParticleNum()
        {
            return (int)ParticleNumBox.Value;
        }
        public double GetW()
        {
            return (double)WBox.Value;
        }
        public double GetC1()
        {
            return (double)C1Box.Value;
        }
        public double GetC2()
        {
            return (double)C2Box.Value;
        }
        public string GetSavePath()
        {
            return SaveFolderTextBox.Text;
        }


        //-------Disable GH's canvas (becomes a white background)
        private void OwnerCanvasEnable(bool IsEnable)
        {
            GH_DocumentEditor ghCanvas = Owner as GH_DocumentEditor;
            if (IsEnable)
                ghCanvas.EnableUI();
            else
                ghCanvas.DisableUI();
        }

        //-------Reset form when closing form
        protected override void OnClosed(EventArgs e)
        {
            OwnerCanvasEnable(true);
            //GeneticAlgo.IsAbort = true;
            base.OnClosed(e);
            PSOOption.psoFrom = null;
        }

        //-------Reflected in the component variable when the input value of the form is changed

        private void ParticleNumBox_ValueChanged(object sender, EventArgs e)
        {
            comp.ParticleNum = GetParticleNum();
        }
        private void WBox_ValueChanged(object sender, EventArgs e)
        {
            comp.W = GetW();
        }
        private void C1Box_ValueChanged(object sender, EventArgs e)
        {
            comp.C1 = GetC1();
        }
        private void C2Box_ValueChanged(object sender, EventArgs e)
        {
            comp.C2 = GetC2();
        }

        private void SaveFolderTextBox_Leave(object sender, EventArgs e)
        {
            comp.SavePath = GetSavePath();
        }
    }
}
