// -------------------------------------------------------------------------------------------
// <copyright file="LogFileViewerViewModel.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.ViewModel
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;
  using GalaSoft.MvvmLight.Command;
  using LogCompare.Business;
  using LogCompare.Model;

  public sealed class LogFileViewerViewModel : LogCompareViewModelBase
  {
    private readonly LogFileViewerBusinessService businessService;

    private string filePath;

    private ObservableCollection<LogEntryViewModel>? logLines;

    private int timeDifferenceThreshold;

    public LogFileViewerViewModel()
    {
      this.filePath = @"C:\Source\_Misc\LogCompare\general.log.json";
      this.TimeDifferenceThreshold = 1000;
      this.ReadFileCommand = new RelayCommand(this.DoReadFile);
      this.businessService = new LogFileViewerBusinessService();

      this.WhenValueChanged(
        () => this.TimeDifferenceThreshold,
        newValue => this.ForEachLogLine(ll => ll.UseTimeDifferenceThreshold(newValue)));
    }

    public ICommand ReadFileCommand { get; }

    public string FilePath
    {
      get
      {
        return this.filePath;
      }

      set
      {
        this.Set(ref this.filePath, value);
      }
    }

    public int TimeDifferenceThreshold
    {
      get
      {
        return this.timeDifferenceThreshold;
      }

      set
      {
        this.Set(ref this.timeDifferenceThreshold, value);
      }
    }

    public ObservableCollection<LogEntryViewModel>? LogLines
    {
      get
      {
        return this.logLines;
      }

      set
      {
        this.Set(ref this.logLines, value);
      }
    }

    private void ForEachLogLine(Action<LogEntryViewModel> logLineAction)
    {
      if (this.LogLines == null)
      {
        return;
      }

      foreach (LogEntryViewModel logEntryViewModel in this.LogLines)
      {
        logLineAction.Invoke(logEntryViewModel);
      }
    }

    private async void DoReadFile()
    {
      List<LogEntry> logEntries = await this.businessService.ReadFile(this.FilePath);
      if (!logEntries.Any())
      {
        return;
      }

      DateTime? startTime = logEntries.First()?.Timestamp;

      IEnumerable<LogEntryViewModel> logEntryViewModels = logEntries.Select(e => new LogEntryViewModel(e));
      this.LogLines = new ObservableCollection<LogEntryViewModel>(logEntryViewModels);

      this.ForEachLogLine(
        logEntryViewModel =>
        {
          logEntryViewModel.UseStartTime(startTime);
          logEntryViewModel.UseTimeDifferenceThreshold(this.TimeDifferenceThreshold);
        });
    }
  }
}
