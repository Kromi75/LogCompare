// -------------------------------------------------------------------------------------------
// <copyright file="LogLineParser.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Business
{
  using System;
  using System.Text.Json;
  using LogCompare.Model;

  public interface ILogLineParser
  {
    LogEntry Parse(string? line, LogEntry previousEntr);
  }

  public class LogLineParser : ILogLineParser
  {
    public LogEntry Parse(string? line, LogEntry previousEntry)
    {
      if (string.IsNullOrEmpty(line))
      {
        return LogEntry.Empty;
      }

      JsonDocument? json = JsonDocument.Parse(line);

      if (json == null)
      {
        return LogEntry.Empty;
      }

      try
      {
        Guid id = Guid.NewGuid();
        JsonElement jsonRootElement = json.RootElement;
        DateTime timestamp = DateTime.Parse(jsonRootElement.GetProperty("@t").GetString() ?? "01.01.1900");
        string message = jsonRootElement.GetProperty("@mt").GetString() ?? string.Empty;
        string sourceContext = jsonRootElement.GetProperty("SourceContext").GetString() ?? string.Empty;
        string? callerMember = null;
        if (jsonRootElement.TryGetProperty("CallerMemberName", out JsonElement callerMemberElement))
        {
          callerMember = callerMemberElement.GetString();
        }

        TimeSpan durationSpan = TimeSpan.MinValue;
        if (jsonRootElement.TryGetProperty("DurationMilliseconds", out JsonElement durationMillisecondsElement))
        {
          if (durationMillisecondsElement.ValueKind == JsonValueKind.Number)
          {
            ulong durationMilliseconds = durationMillisecondsElement.GetUInt64();
            durationSpan = TimeSpan.FromMilliseconds(durationMilliseconds);
          }
          else if (durationMillisecondsElement.ValueKind == JsonValueKind.String)
          {
            string? rcDuration = durationMillisecondsElement.GetString();
            durationSpan = string.IsNullOrEmpty(rcDuration) ? TimeSpan.MinValue : TimeSpan.Parse(rcDuration);
          }
        }

        if (jsonRootElement.TryGetProperty("RcDuration", out JsonElement rcDurationElement))
        {
          string? rcDuration = rcDurationElement.GetString();
          durationSpan = string.IsNullOrEmpty(rcDuration) ? TimeSpan.MinValue : TimeSpan.Parse(rcDuration);
        }

        return new LogEntry(id, previousEntry, timestamp, message, sourceContext, callerMember, durationSpan);
      }
      catch
      {
        return LogEntry.Error;
      }
    }
  }
}
