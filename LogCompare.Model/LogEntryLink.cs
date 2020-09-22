// -------------------------------------------------------------------------------------------
// <copyright file="LogEntryLink.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare.Model
{
  using System;

  public class LogEntryLink
  {
    public LogEntryLink(Guid leftId, Guid rightId)
    {
      this.LeftId = leftId;
      this.RightId = rightId;
    }

    public Guid LeftId { get; }
    
    public Guid RightId { get; }
  }
}
