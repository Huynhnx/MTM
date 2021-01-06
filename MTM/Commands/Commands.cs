using MTM.ViewModels;
using MTM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;

namespace MTM
{
    public class Commands
    {
        private static Runspace runSpace;
        private static PipelineExecutor pipelineExecutor;
        private static MTMMainViewModel mainVm;
        public static void RunReportCmdInvoke(object obj)
        {

        }

        public static void RunMTMCmdInvoke(object obj)
        {
           
            MainWindow view = obj as MainWindow;
            mainVm = view.DataContext as MTMMainViewModel;
            StopScript();
            // create Powershell runspace
            runSpace = RunspaceFactory.CreateRunspace();
            // open it
            runSpace.Open();
            List<string> TestCase = new List<string>();
            for (int i=0;i<mainVm.TestSelected.Count;i++)
            {
                TestCase.Add(mainVm.TestSelected[i].ParentFolder);
            }
            pipelineExecutor = new PipelineExecutor(runSpace, view, mainVm.MasterScript, TestCase);
            pipelineExecutor.OnDataReady += new PipelineExecutor.DataReadyDelegate(pipelineExecutor_OnDataReady);
            pipelineExecutor.OnDataEnd += new PipelineExecutor.DataEndDelegate(pipelineExecutor_OnDataEnd);
            pipelineExecutor.OnErrorReady += new PipelineExecutor.ErrorReadyDelegate(pipelineExecutor_OnErrorReady);
            pipelineExecutor.Start();

        }
        public static void BrownTestFoldercmdInvoke(object obj)
        {
           
            MainWindow view = obj as MainWindow;
            mainVm = view.DataContext as MTMMainViewModel;
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mainVm.TestFolder=  dialog.SelectedPath;
            }
        }
        public static void SelectCmdInvoke(object obj)
        {

            MainWindow view = obj as MainWindow;
            mainVm = view.DataContext as MTMMainViewModel;
            SelectView smv = new SelectView();
            
            SelectViewModel smvm = new SelectViewModel();
            smvm.OkCmd = new MVVMCore.RelayCommand(OkCmdInvoke);
            string[] filePaths = Directory.GetFiles(mainVm.TestFolder, "*.svb",
                                         SearchOption.AllDirectories);
            foreach(string test in filePaths)
            {
                TestCase tc1 = new TestCase();
                tc1.ParentFolder = Path.GetDirectoryName(test);
                tc1.TestName = test;
                tc1.status = false;
                smvm.ListTest.Add(tc1);
            }
            
            smv.DataContext = smvm;
            smv.Owner=view;
            smv.ShowDialog();
        }
        public static void SelectMasterScriptCmdInvoke(object obj)
        {

            MainWindow view = obj as MainWindow;
            MTMMainViewModel vm = view.DataContext as MTMMainViewModel;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "PowerShell Script(*.PS1)|*.ps1";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                vm.MasterScript = openFileDialog1.FileName;
            }
        }
        #region PipelineCmd
        private static void StopScript()
        {
            if (pipelineExecutor != null)
            {
                pipelineExecutor.OnDataReady -= new PipelineExecutor.DataReadyDelegate(pipelineExecutor_OnDataReady);
                pipelineExecutor.OnDataEnd -= new PipelineExecutor.DataEndDelegate(pipelineExecutor_OnDataEnd);
                pipelineExecutor.Stop();
                pipelineExecutor = null;
            }
        }
        private static void pipelineExecutor_OnDataReady(PipelineExecutor sender, ICollection<PSObject> data)
        {
            foreach (PSObject obj in data)
            {
                AppendLine(obj.ToString());
            }
        }

        private static void pipelineExecutor_OnDataEnd(PipelineExecutor sender)
        {
            if (sender.Pipeline.PipelineStateInfo.State == PipelineState.Failed)
            {
                AppendLine(string.Format("Error in script: {0}", sender.Pipeline.PipelineStateInfo.Reason));
            }
            else
            {
                AppendLine("Ready.");
            }
        }
        private static void  pipelineExecutor_OnErrorReady(PipelineExecutor sender, ICollection<object> data)
        {
            foreach (object e in data)
            {
                AppendLine("Error : " + e.ToString());
            }
        }
        private static void AppendLine(string line)
        {
            //listBoxChanged = true;
            //if (listBoxOutput.Items.Count > 10000) listBoxOutput.Items.RemoveAt(0);
            //listBoxOutput.Items.Add(line);
            //listBoxOutput.TopIndex = listBoxOutput.Items.Count - 1;
            mainVm.Log += line +"\n";
        }
        #endregion
        #region SelectCmd
        public static void OkCmdInvoke(object obj)
        {

            SelectView view = obj as SelectView;
            SelectViewModel vm = view.DataContext as SelectViewModel;
            mainVm.TestSelected = vm.ListTest.Where(x => x.selected == true).ToList();
            view.Close();
        }
        #endregion


    }
}
