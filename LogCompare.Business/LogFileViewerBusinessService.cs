// -------------------------------------------------------------------------------------------
// <copyright file="LogFileViewerBusinessService.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Business
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using LogCompare.Model;

  public sealed class LogFileViewerBusinessService
  {
    public async Task<List<LogEntry>> ReadFile(string filePath)
    {
      ILogLineParser lineParser = new LogLineParser();
      LogFileReader fileReader = new LogFileReader(filePath, lineParser);
      return await fileReader.Read();
    }
  }
}
