// -------------------------------------------------------------------------------------------
// <copyright file="LogStream.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Model
{
  using System.Collections.Generic;

  public class LogStream
  {
    public LogStream(string name)
    {
      this.Name = name;
      this.Entries = new List<LogEntry>();
    }

    public string Name { get; }

    public IEnumerable<LogEntry> Entries { get; }
  }
}
