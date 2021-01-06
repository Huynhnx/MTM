using MTM.ViewModels;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MTM
{
    class DispatcherAsyncResult : IAsyncResult
    {
        private readonly IAsyncResult result;

        public DispatcherAsyncResult(DispatcherOperation operation)
        {
            this.Operation = operation;
            this.result = operation.Task;
        }

        public DispatcherOperation Operation { get; }

        public bool IsCompleted => this.result.IsCompleted;
        public WaitHandle AsyncWaitHandle => this.result.AsyncWaitHandle;
        public object AsyncState => this.result.AsyncState;
        public bool CompletedSynchronously => this.result.CompletedSynchronously;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISynchronizeInvoke
    {
        private readonly Dispatcher dispatcher;

       

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            // Obtaining a DispatcherOperation instance
            // and wrapping it with our proxy class
            return new DispatcherAsyncResult(
                this.dispatcher.BeginInvoke(method, DispatcherPriority.Normal, args));
        }

        public object EndInvoke(IAsyncResult result)
        {
            DispatcherAsyncResult dispatcherResult = result as DispatcherAsyncResult;
            dispatcherResult.Operation.Wait();
            return dispatcherResult.Operation.Result;
        }

        public object Invoke(Delegate method, object[] args)
        {
            return dispatcher.Invoke(method, DispatcherPriority.Normal, args);
        }

        public bool InvokeRequired => !this.dispatcher.CheckAccess();

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        public MainWindow()
        {
            this.dispatcher =  Dispatcher.CurrentDispatcher;
            MTMMainViewModel vm = new MTMMainViewModel();
            vm.RunMTMCmd = new MVVMCore.RelayCommand(Commands.RunMTMCmdInvoke);
            vm.BrownTestFoldercmd = new MVVMCore.RelayCommand(Commands.BrownTestFoldercmdInvoke);
            vm.SelectTestcaseCmd = new MVVMCore.RelayCommand(Commands.SelectCmdInvoke);
            vm.SelectMasterScriptCmd = new MVVMCore.RelayCommand(Commands.SelectMasterScriptCmdInvoke);
            vm.RunReportCmd = new MVVMCore.RelayCommand(Commands.RunReportCmdInvoke);
            this.DataContext = vm;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.ScrollToEnd();
        }
    }
}
