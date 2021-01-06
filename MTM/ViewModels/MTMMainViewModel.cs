using MVVMCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MTM.ViewModels
{
    class MTMMainViewModel:ViewModelBase
    {
        public RelayCommand RunMTMCmd { get; set; }
        public RelayCommand BrownTestFoldercmd { get; set; }
        public RelayCommand SelectTestcaseCmd { get; set; }
        public RelayCommand SelectMasterScriptCmd { get; set; }
        public RelayCommand RunReportCmd  { get; set; }
        string log = string.Empty;
        public string Log
        {
            get
            {
                return log;
            }
            set
            {
                if (log!= value)
                {
                    log = value;
                    RaisePropertyChanged("Log");
                }
            }
        }
        string testfolder;
        public string TestFolder
        {
            get
            {
                return testfolder;
            }
            set
            {
                if (value!= testfolder)
                {
                    testfolder = value;
                    RaisePropertyChanged("TestFolder");
                }
            }
        }
        string masterScript;
        public string MasterScript
        {
            get
            {
                return masterScript;
            }
            set
            {
                if (value != masterScript)
                {
                    masterScript = value;
                    RaisePropertyChanged("MasterScript");
                }
            }
        }
        List<TestCase> testselected = new List<TestCase>();
        public List<TestCase> TestSelected
        {
            get
            {
                return testselected;
            }
            set
            {
                if (testselected != value)
                {
                    testselected = value;

                    RaisePropertyChanged("TestSelected");
                }
            }
        }
    }
}
