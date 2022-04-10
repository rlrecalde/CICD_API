using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Process : Interfaces.IProcess
    {
        private readonly ILogger<Process> _logger;

        private enum ProcessStatus
        {
            None, Started, Exited,
        }

        private ProcessStatus _processStatus;
        private bool _succeeded;
        private IList<string> _dataLines;
        private IList<string> _errorLines;

        public Process(ILogger<Process> logger)
        {
            this._logger = logger;
        }

        public async Task<BO.ProcessResponse> ExecuteAsync(BO.Process process, bool redirectOutput = false)
        {
            this._processStatus = ProcessStatus.None;
            this._succeeded = false;
            this._dataLines = new List<string>();
            this._errorLines = new List<string>();
            var processData = new DAL.Process(redirectOutput);

            processData.StartedEvent += ProcessData_StartedEvent;
            processData.ExitedEvent += ProcessData_ExitedEvent;
            processData.OutputDataReceived += ProcessData_OutputDataReceived;
            processData.ErrorDataReceived += ProcessData_ErrorDataReceived;

            this._logger.LogInformation($"Executing command: {process.Name} {process.Arguments}");

            _ = Task.Run(() =>
            {
                processData.Start(process);
            });

            while (this._processStatus == ProcessStatus.None)
                await Task.Delay(50);

            if (this._processStatus == ProcessStatus.Exited)
            {
                var response = new BO.ProcessResponse { Sucess = false, DataLines = this._dataLines, ErrorLines = this._errorLines };

                return response;
            }

            while (this._processStatus == ProcessStatus.Started)
                await Task.Delay(50);

            var processResponse = new BO.ProcessResponse { Sucess = false, DataLines = this._dataLines, ErrorLines = this._errorLines };

            if (this._succeeded)
                processResponse.Sucess = true;

            return processResponse;
        }

        #region Private Methods

        private void ProcessData_ExitedEvent(object? sender, bool success)
        {
            this._processStatus = ProcessStatus.Exited;
            this._succeeded = success;
        }

        private void ProcessData_StartedEvent(object? sender, bool started)
        {
            this._processStatus = ProcessStatus.Started;
        }

        private void ProcessData_OutputDataReceived(object? sender, string? data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                this._logger.LogInformation(data);
                this._dataLines.Add(data);
            }
        }

        private void ProcessData_ErrorDataReceived(object? sender, string? data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                this._logger.LogError(data);
                this._errorLines.Add(data);
            }
        }

        #endregion
    }
}
