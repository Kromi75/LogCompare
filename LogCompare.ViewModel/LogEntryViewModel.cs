// -------------------------------------------------------------------------------------------
// <copyright file="LogEntryViewModel.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.ViewModel
{
  using System;
  using LogCompare.Model;

  public class LogEntryViewModel : LogCompareViewModelBase
  {
    private readonly LogEntry logEntry;

    private double timeDifferenceIndex;

    private DateTime? logStartTime;

    private int timeDifferenceThreshold;

    public LogEntryViewModel(LogEntry logEntry)
    {
      this.logEntry = logEntry;
    }

    public bool IsError
    {
      get
      {
        return this.logEntry.Kind == LogEntry.LogEntryKind.Error;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.logEntry.Kind == LogEntry.LogEntryKind.Empty;
      }
    }

    /// <summary>
    /// Liefert den Zeitstempel des Logeintrags.
    /// </summary>
    public string TimestampAsString
    {
      get
      {
        return this.logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
      }
    }

    /// <summary>
    /// Liefert die Zeitdifferenz zum vorherigen Logeintrag.
    /// </summary>
    public double? TimeDifference
    {
      get
      {
        if (this.logEntry.TimeDifference == TimeSpan.MinValue)
        {
          return null;
        }

        return this.logEntry.TimeDifference.TotalMilliseconds;
      }
    }

    public double TimeDifferenceIndex
    {
      get
      {
        return this.timeDifferenceIndex;
      }

      private set
      {
        this.Set(ref this.timeDifferenceIndex, value);
      }
    }

    /// <summary>
    /// Liefert die in der Lognachricht enthaltene Dauer.
    /// </summary>
    public TimeSpan Duration
    {
      get
      {
        return this.logEntry.Duration;
      }
    }

    /// <summary>
    /// Liefert die Lognachricht.
    /// </summary>
    public string? Message
    {
      get
      {
        return this.logEntry.Message;
      }
    }

    /// <summary>
    /// Liefert den Source-Context des Logeintrags.
    /// </summary>
    public string? SourceContext
    {
      get
      {
        return this.logEntry.SourceContext;
      }
    }

    /// <summary>
    /// Liefert den Namen des Members, das den Logeintrag geschrieben hat.
    /// </summary>
    public string? CallerMember
    {
      get
      {
        return this.logEntry.CallerMember;
      }
    }

    public double? TimeOffset { get; set; }

    public void UseTimeDifferenceThreshold(in int threshold)
    {
      this.timeDifferenceThreshold = threshold;
      this.CalculateTimeDifferenceIndex();
    }

    public void UseStartTime(in DateTime? startTime)
    {
      this.logStartTime = startTime;
      this.CalculateTimeOffset();
    }

    private void CalculateTimeOffset()
    {
      if (this.logStartTime == null)
      {
        this.TimeOffset = null;
        return;
      }

      this.TimeOffset = (this.logEntry.Timestamp - this.logStartTime.Value).TotalMilliseconds;
    }

    private void CalculateTimeDifferenceIndex()
    {
      TimeSpan timeDifference = this.logEntry.TimeDifference;
      if (timeDifference == TimeSpan.MinValue)
      {
        this.TimeDifferenceIndex = 0;
        return;
      }

      double differenceMilliseconds = timeDifference.TotalMilliseconds;
      if (differenceMilliseconds >= this.timeDifferenceThreshold)
      {
        this.TimeDifferenceIndex = 1;
        return;
      }

      double index = Math.Round(differenceMilliseconds / this.timeDifferenceThreshold, 3);
      this.TimeDifferenceIndex = index;
    }
  }
}
