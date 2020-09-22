// -------------------------------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Model
{
  using System;

  public sealed class LogEntry
  {
    static LogEntry()
    {
      LogEntry.Empty = new LogEntry(LogEntryKind.Empty);
      LogEntry.Error = new LogEntry(LogEntryKind.Error);
    }

    public LogEntry(Guid id, in LogEntry previousEntry, in DateTime timestamp, string message, string sourceContext, string? callerMember, in TimeSpan duration)
    {
      this.Kind = LogEntryKind.Normal;
      this.Id = id;
      this.Timestamp = timestamp;
      this.Message = message;
      this.SourceContext = sourceContext;
      this.CallerMember = callerMember;
      this.Duration = duration;
      this.PreviousEntry = previousEntry;

      if (previousEntry != LogEntry.Empty)
      {
        this.TimeDifference = this.Timestamp - previousEntry.Timestamp;
      }
      else
      {
        this.TimeDifference = TimeSpan.Zero;
      }
    }

    private LogEntry(LogEntryKind kind)
    {
      this.Kind = kind;
      this.Id = Guid.Empty;
      this.PreviousEntry = LogEntry.Empty;
      this.Timestamp = DateTime.MinValue;
      this.TimeDifference = TimeSpan.Zero;
    }

    public enum LogEntryKind
    {
      Normal,

      Empty,

      Error
    }

    /// <summary>
    /// Liefert eine Instanz, die einen Fehler beim Parsen repräsentiert.
    /// </summary>
    public static LogEntry Error { get; }

    /// <summary>
    /// Liefert eine leere Instanz.
    /// </summary>
    public static LogEntry Empty { get; }

    public LogEntryKind Kind { get; }

    /// <summary>
    /// Liefert die eindeutige ID für diesen Logeintrag.
    /// </summary>
    /// <remarks>
    /// Die ID wird beim Einlesen der Logdaten vergeben, sie ist nicht in den Logdaten enthalten.
    /// </remarks>
    public Guid Id { get; }

    /// <summary>
    /// Liefert den Zeitstempel des Logeintrags.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Liefert die Zeitdifferenz zum vorherigen Logeintrag.
    /// </summary>
    public TimeSpan TimeDifference { get; }

    /// <summary>
    /// Liefert die in der Lognachricht enthaltene Dauer.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Verweist auf den vorherigen Logeintrag.
    /// </summary>
    /// <remarks>
    /// Wird verwendet, um die Zeitdifferenz zum vorherigen Logeintrag zu berechnen.
    /// </remarks>
    public LogEntry PreviousEntry { get; }

    /// <summary>
    /// Liefert die Lognachricht.
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Liefert den Source-Context des Logeintrags.
    /// </summary>
    public string? SourceContext { get; }

    /// <summary>
    /// Liefert den Namen des Members, das den Logeintrag geschrieben hat.
    /// </summary>
    public string? CallerMember { get; }

    /// <summary>
    /// Liefert einen String, der das aktuelle Objekt repräsentiert.
    /// </summary>
    /// <returns>Einen String, der das aktuelle Objekt repräsentiert.</returns>
    public override string ToString()
    {
      return $"{this.Timestamp} {this.Message}";
    }
  }
}
