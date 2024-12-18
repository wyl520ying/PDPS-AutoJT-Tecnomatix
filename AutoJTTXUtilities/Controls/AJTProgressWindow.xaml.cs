using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoJTTXUtilities.Controls
{
    /// <summary>
    /// AJTProgressWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AJTProgressWindow : AJTProgressWindowBase
    {
        public AJTProgressWindow(double x, double y) : base(x, y)
        {
            InitializeComponent();
            //执行任务
            this.RunTasker();
        }

        //取消按钮
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the cancellation token
            if (base.currentCancellationSource != null)
            {
                base.currentCancellationSource.Cancel();
                base.currentCancellationSource.Dispose();
                base.currentCancellationSource = null;
            }
            base.Close();
        }

        #region 异步线程操作

        private async void RunTasker()
        {
            // Enable/disabled buttons so that only one counting task runs at a time.            
            this.Button_Cancel.IsEnabled = true;

            try
            {
                // Set up the progress event handler - this instance automatically invokes to the UI for UI updates
                // this.ProgressBar_Progress is the progress bar control
                IProgress<int> progress = new Progress<int>(count => this.ProgressBar_Progress.Value = count);
                this.ProgressBar_Progress.IsIndeterminate = true;
                base.currentCancellationSource = new CancellationTokenSource();
                await CountToOneHundredAsync(progress, this.currentCancellationSource.Token);

                this.Topmost = false;
                // Operation was successful. Let the user know!
                //MessageBox.Show("Done counting!");
            }
            catch (OperationCanceledException)
            {
                // Operation was cancelled. Let the user know!
                //MessageBox.Show("Operation cancelled.");
                Thread.CurrentThread.IsBackground = true;
                GC.Collect();
            }
            finally
            {
                // Reset controls in a finally block so that they ALWAYS go 
                // back to the correct state once the counting ends, 
                // regardless of any exceptions
                this.Button_Cancel.IsEnabled = false;
                this.ProgressBar_Progress.Value = 0;

                // Dispose of the cancellation source as it is no longer needed
                if (this.currentCancellationSource != null)
                {
                    this.currentCancellationSource.Dispose();
                    this.currentCancellationSource = null;
                }
            }

            this.Close();
        }

        /*
        private async Task CountToOneHundredAsync(IProgress<int> progress, CancellationToken cancellationToken)
        {

            //for (int i = 0; i < automaticDimension.assemblyRelations_1.Count; i++)
            //{
            // This is where the 'work' is performed. 
            // Feel free to swap out Task.Delay for your own Task-returning code! 
            // You can even await many tasks here                

            // ConfigureAwait(false) tells the task that we dont need to come back to the UI after awaiting
            // This is a good read on the subject - https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html

            progress.Report(10);
            await Task.Delay(1, cancellationToken).ConfigureAwait(false);

            await this.quickJointForm.CalcTxGeometryMethod(cancellationToken);

            progress.Report(90);


            //Task task2 = Task.Factory.StartNew(() => this.automaticDimension.TaskRun_Topology());

            //new AutomaticDimension(this.cATIALibrary, new AutoJT.Models.DrawingProjection.GenerateCATDraft._DrawingProjection(this.catPro));

            // If cancelled, an exception will be thrown by the call the task.Delay
            // and will bubble up to the calling method because we used await!

            // Report progress with the current number
            //progress.Report(i);
            //}            
        }
        */

        protected virtual async Task CountToOneHundredAsync(IProgress<int> progress, CancellationToken cancellationToken) { }

        #endregion
    }
}
