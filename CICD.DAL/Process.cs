using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class Process
    {
        private System.Diagnostics.Process _process;
        private readonly bool _redirectOutput;

        public event EventHandler<bool> StartedEvent;
        public event EventHandler<bool> ExitedEvent;
        public event EventHandler<string?> ErrorDataReceived;
        public event EventHandler<string?> OutputDataReceived;

        public Process(bool redirectOutput)
        {
            this._redirectOutput = redirectOutput;
        }

        public void Start(BO.Process process)
        {
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = process.Name,
                UseShellExecute = false,
                RedirectStandardError = this._redirectOutput,
                RedirectStandardInput = false,
                RedirectStandardOutput = this._redirectOutput,
                CreateNoWindow = false,
                WorkingDirectory = process.WorkingDirectory,
                Arguments = process.Arguments,
            };

            this._process = new System.Diagnostics.Process { StartInfo = processStartInfo, EnableRaisingEvents = true };
            this._process.Exited += Process_Exited;
            this._process.ErrorDataReceived += Process_ErrorDataReceived;
            this._process.OutputDataReceived += Process_OutputDataReceived;

            bool started = this._process.Start();

            if (this._redirectOutput)
            {
                this._process.BeginErrorReadLine();
                this._process.BeginOutputReadLine();
            }

            if (this.StartedEvent != null)
                this.StartedEvent(this, started);

            this._process.WaitForExit();
        }

        #region Events

        private void Process_Exited(object? sender, EventArgs e)
        {
            var process = (System.Diagnostics.Process?)sender;

            if (this.ExitedEvent != null)
                this.ExitedEvent(this, process != null && process.ExitCode == 0 ? true : false);
        }

        private void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (this.ErrorDataReceived != null)
                this.ErrorDataReceived(this, e.Data);
        }

        private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (this.OutputDataReceived != null)
                this.OutputDataReceived(this, e.Data);
        }

        #endregion
    }
}
