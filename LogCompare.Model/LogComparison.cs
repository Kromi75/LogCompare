// -------------------------------------------------------------------------------------------
// <copyright file="LogComparison.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Model
{
  using System.Collections.Generic;

  public class LogComparison
  {
    public LogComparison()
    {
      this.LogStreams = new List<LogStream>();
      this.EntryLinks = new List<LogEntryLink>();
    }

    public IEnumerable<LogStream> LogStreams { get; }

    public IEnumerable<LogEntryLink> EntryLinks { get; }
  }
}
