// -------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="i-SOLUTIONS Health GmbH">
//   <CopyrightText>(C) 1997-2020 i-SOLUTIONS Health GmbH</CopyrightText>
// </copyright>
// -------------------------------------------------------------------------------------------
namespace LogCompare
{
  using System.Globalization;
  using System.Windows;
  using System.Windows.Markup;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name);

      this.InitializeComponent();
    }
  }
}
