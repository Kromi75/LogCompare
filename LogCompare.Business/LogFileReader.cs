// -------------------------------------------------------------------------------------------
// <copyright file="LogFileReader.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Business
{
  using System.Collections.Generic;
  using System.IO;
  using System.Threading.Tasks;
  using LogCompare.Model;

  public sealed class LogFileReader
  {
    private readonly ILogLineParser logLineParser;

    public LogFileReader(string filePath, ILogLineParser logLineParser)
    {
      this.FilePath = filePath;
      this.logLineParser = logLineParser;
    }

    public string FilePath { get; }

    public async Task<List<LogEntry>> Read()
    {
      List<LogEntry> entries = new List<LogEntry>();

      await using FileStream stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read);
      using StreamReader reader = new StreamReader(stream);

      LogEntry previousEntry = LogEntry.Empty;
      while (!reader.EndOfStream)
      {
        string? line = await reader.ReadLineAsync();
        LogEntry entry = this.logLineParser.Parse(line, previousEntry);
        previousEntry = entry;
        entries.Add(entry);
      }

      return entries;
    }
  }
}
