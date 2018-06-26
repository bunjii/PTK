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

namespace PTK.Optimization
{
    //--------------------------------------------------------
    //  UI for inputting and executing setting values of GA
    //--------------------------------------------------------
    public partial class GAForm : Form
    {
        private GeneticAlgorithmComponent03 comp;   //Component to be attached to the form

        public GAForm(GeneticAlgorithmComponent03 _comp)
        {
            InitializeComponent();
            Owner = Grasshopper.Instances.DocumentEditor;
            comp = _comp;
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (TransitionOperatingState())
            {
                FitnessChart.Series["AverageChart"].Points.Clear();         //Reset graph of average value
                FitnessChart.Series["MaxMinChart"].Points.Clear();          //Reset graph of Min-Max value
                FitnessChart.Series["BestFitnessUpdate"].Points.Clear();    //Reset graph of BestFitness value
                RestrictionFitnessChart.Series["AverageChart"].Points.Clear();          //Reset graph of average value
                RestrictionFitnessChart.Series["MaxMinChart"].Points.Clear();           //Reset graph of Min-Max value
                RestrictionFitnessChart.Series["BestFitnessUpdate"].Points.Clear();     //Reset graph of BestFitness value

                await Task.Run(() =>
                {
                    GeneticAlgo.GARun();    //Run in a separate thread
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
                if (!GeneticAlgo.ResumeRun())  //Perform resume processing
                {
                    //Resume Failure
                    FitnessChart.Series["AverageChart"].Points.Clear();         //Reset graph of average value
                    FitnessChart.Series["MaxMinChart"].Points.Clear();          //Reset graph of Min-Max value
                    FitnessChart.Series["BestFitnessUpdate"].Points.Clear();    //Reset graph of BestFitness value
                    RestrictionFitnessChart.Series["AverageChart"].Points.Clear();          //Reset graph of average value
                    RestrictionFitnessChart.Series["MaxMinChart"].Points.Clear();           //Reset graph of Min-Max value
                    RestrictionFitnessChart.Series["BestFitnessUpdate"].Points.Clear();     //Reset graph of BestFitness value
                }

                await Task.Run(() =>
                {
                    GeneticAlgo.GARun();    //Run in a separate thread
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

            IndivitualNumBox.Value = comp.IndividualNum;
            EliteRateBox.Value = (decimal)comp.EliteRate;
            MutationRateBox.Value = (decimal)comp.MutationRate;
            if (comp.IsMinimize)
                MinRadioButton.Checked = true;
            else
                MaxRadioButton.Checked = true;
            MaxGenerationCheck.Checked = comp.IsEnableMaxGene;
            MaxGenerationBox.Value = comp.MaxGeneration;
            ContinuationCheck.Checked = comp.IsEnableContinuation;
            ContinuationBox.Value = comp.ContinuationNum;
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

        public delegate void DrawChartDelegate(int _x, double _yAve, double _yMax, double _yMin, bool _IsDrawBestFitness);
        public void DrawChart(int _x, double _yAve, double _yMax, double _yMin, bool _IsDrawBestFitness)
        {
            if (this.InvokeRequired)    //Use Invoke to securely access the form when calling from another thread
            {
                DrawChartDelegate d = new DrawChartDelegate(DrawChart);
                this.Invoke(d, _x, _yAve, _yMax, _yMin, _IsDrawBestFitness);
                return;
            }
            else
            {
                FitnessChart.Series["AverageChart"].Points.AddXY(_x, _yAve);        //Add average value to graph
                FitnessChart.Series["MaxMinChart"].Points.AddXY(_x, _yMax, _yMin);  //Add Min-Max value to graph
                if (RestrictionFitnessChart.Series["AverageChart"].Points.Count >= 20)
                {
                    RestrictionFitnessChart.Series["AverageChart"].Points.RemoveAt(0);
                    RestrictionFitnessChart.Series["MaxMinChart"].Points.RemoveAt(0);
                }
                RestrictionFitnessChart.Series["AverageChart"].Points.AddXY(_x, _yAve);        //Add average value to graph
                RestrictionFitnessChart.Series["MaxMinChart"].Points.AddXY(_x, _yMax, _yMin);  //Add Min-Max value to graph
                //-------ReScaling RestrictionFitnessChart Area
                RestrictionFitnessChart.ChartAreas["ChartArea1"].AxisX.Minimum = RestrictionFitnessChart.Series["AverageChart"].Points.ElementAt(0).XValue;
                RestrictionFitnessChart.ChartAreas["ChartArea1"].AxisX.Maximum = RestrictionFitnessChart.Series["AverageChart"].Points.Last().XValue;
                RestrictionFitnessChart.ChartAreas["ChartArea1"].AxisY.Minimum = RestrictionFitnessChart.Series["MaxMinChart"].Points.Min(p => p.YValues[1]);   //Get Y-min
                RestrictionFitnessChart.ChartAreas["ChartArea1"].AxisY.Maximum = RestrictionFitnessChart.Series["MaxMinChart"].Points.Max(p => p.YValues[0]);   //Get Y-max
                //-------Best Fitness point add to graph
                if (_IsDrawBestFitness)
                {
                    if (GetIsFitnessMinimize())
                    {
                        FitnessChart.Series["BestFitnessUpdate"].Points.AddXY(_x, _yMin);
                        RestrictionFitnessChart.Series["BestFitnessUpdate"].Points.AddXY(_x, _yMin);
                    }
                    else
                    {
                        FitnessChart.Series["BestFitnessUpdate"].Points.AddXY(_x, _yMax);
                        RestrictionFitnessChart.Series["BestFitnessUpdate"].Points.AddXY(_x, _yMax);
                    }
                }
            }
        }

        //-------Changed State for algorithm running
        public bool TransitionOperatingState()
        {
            if (GeneticAlgo.ResetGeneticAlgo())
            {
                GeneticAlgo.IsAbort = false;
                StopButton.Enabled = true;
                StartButton.Enabled = false;
                RestartButton.Enabled = false;
                ConfigurationGroup.Enabled = false;
                ExitConditionsGroup.Enabled = false;
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
                GeneticAlgo.IsAbort = true;
                StopButton.Enabled = false;
                StartButton.Enabled = true;
                RestartButton.Enabled = true;
                ConfigurationGroup.Enabled = true;
                ExitConditionsGroup.Enabled = true;
                SaveGenerationGroup.Enabled = true;
            }
        }

        //-------Function to get a value from a form
        public int GetIndivitualNum()
        {
            return (int)IndivitualNumBox.Value;
        }
        public double GetEliteRate()
        {
            return (double)EliteRateBox.Value;
        }
        public double GetMutationRate()
        {
            return (double)MutationRateBox.Value;
        }
        public bool GetIsFitnessMinimize()
        {
            return MinRadioButton.Checked;
        }
        public bool GetIsMaxGenerationChecked()
        {
            return MaxGenerationCheck.Checked;
        }
        public int GetMaxGeneration()
        {
            return (int)MaxGenerationBox.Value;
        }
        public bool GetIsContinuationChecked()
        {
            return ContinuationCheck.Checked;
        }
        public int GetContinuation()
        {
            return (int)ContinuationBox.Value;
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
            GeneticAlgo.IsAbort = true;
            base.OnClosed(e);
            GeneAlgoOption.gaForm = null;
        }

        //-------Reflected in the component variable when the input value of the form is changed
        private void IndivitualNumBox_ValueChanged(object sender, EventArgs e)
        {
            comp.IndividualNum = GetIndivitualNum();
        }
        private void EliteRateBox_ValueChanged(object sender, EventArgs e)
        {
            comp.EliteRate = GetEliteRate();
        }
        private void MutationRateBox_ValueChanged(object sender, EventArgs e)
        {
            comp.MutationRate = GetMutationRate();
        }
        private void MinRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            comp.IsMinimize = GetIsFitnessMinimize();
        }
        private void MaxGenerationCheck_CheckedChanged(object sender, EventArgs e)
        {
            comp.IsEnableMaxGene = GetIsMaxGenerationChecked();
            MaxGenerationBox.Enabled = GetIsMaxGenerationChecked();
        }
        private void MaxGenerationBox_ValueChanged(object sender, EventArgs e)
        {
            comp.MaxGeneration = GetMaxGeneration();
        }
        private void ContinuationCheck_CheckedChanged(object sender, EventArgs e)
        {
            comp.IsEnableContinuation = GetIsContinuationChecked();
            ContinuationBox.Enabled = GetIsContinuationChecked();
        }
        private void ContinuationBox_ValueChanged(object sender, EventArgs e)
        {
            comp.ContinuationNum = GetContinuation();
        }
        private void SaveFolderTextBox_Leave(object sender, EventArgs e)
        {
            comp.SavePath = GetSavePath();
        }

        private void GAForm_Load(object sender, EventArgs e)
        {

        }
    }
}
