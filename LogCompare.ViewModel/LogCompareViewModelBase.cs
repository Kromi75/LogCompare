// -------------------------------------------------------------------------------
// SynchronizedViewModelBase.cs
// -------------------------------------------------------------------------------
namespace LogCompare.ViewModel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq.Expressions;
  using System.Reflection;
  using System.Runtime.CompilerServices;
  using GalaSoft.MvvmLight;

  public abstract class LogCompareViewModelBase : ViewModelBase
  {
    private readonly Dictionary<string, object> whenValueChangedActions;

    private bool isSynchronizing;

    private bool hasValueChangedActions;

    protected LogCompareViewModelBase()
    {
      this.whenValueChangedActions = new Dictionary<string, object>();
      this.PropertyChanged += this.OnPropertyChanged;
    }

    protected virtual void OnSynchronize(string propertyName)
    {
    }

    protected new bool Set<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
      if (propertyName == null)
      {
        throw new ArgumentNullException(nameof(propertyName));
      }

      T oldValue = field;
      if (!base.Set(ref field, newValue, propertyName))
      {
        return false;
      }

      if (!this.hasValueChangedActions)
      {
        return true;
      }

      if (this.whenValueChangedActions.ContainsKey(propertyName))
      {
        if (this.whenValueChangedActions[propertyName] is Action<T> actionWithNewValueOnly)
        {
          actionWithNewValueOnly.Invoke(newValue);
        }
        else
        {
          Action<T, T>? actionWithOldAndNewValue = this.whenValueChangedActions[propertyName] as Action<T, T>;
          actionWithOldAndNewValue?.Invoke(oldValue, newValue);
        }
      }

      return true;
    }

    /// <summary>
    /// Definiert eine Aktion, die nach dem Setzen eines neuen Wertes für eine Eigenschaft ausgeführt wird.
    /// </summary>
    /// <typeparam name="T">Typ der Eigenschaft.</typeparam>
    /// <param name="propertyExpression">Ausdruck, der die Eigenschaft identifiziert.</param>
    /// <param name="action">Auszuführenden Aktion mit dem neuen Wert als Parameter.</param>
    protected void WhenValueChanged<T>(Expression<Func<T>> propertyExpression, Action<T> action)
    {
      this.AddValueChangedAction(propertyExpression, action);
    }

    /// <summary>
    /// Definiert eine Aktion, die nach dem Setzen eines neuen Wertes für eine Eigenschaft ausgeführt wird.
    /// </summary>
    /// <typeparam name="T">Typ der Eigenschaft.</typeparam>
    /// <param name="propertyExpression">Ausdruck, der die Eigenschaft identifiziert.</param>
    /// <param name="action">Auszuführenden Aktion mit dem alten und dem neuen Wert als Parameter.</param>
    protected void WhenValueChanged<T>(Expression<Func<T>> propertyExpression, Action<T, T> action)
    {
      this.AddValueChangedAction(propertyExpression, action);
    }

    private void AddValueChangedAction<T>(Expression<Func<T>> propertyExpression, object action)
    {
      if (propertyExpression == null)
      {
        throw new ArgumentNullException(nameof(propertyExpression));
      }

      if (action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      if (!(propertyExpression.Body is MemberExpression body))
      {
        throw new ArgumentException("Invalid argument", nameof(propertyExpression));
      }

      PropertyInfo? member = body?.Member as PropertyInfo;
      if (member == null)
      {
        throw new ArgumentException("Argument is not a property", nameof(propertyExpression));
      }

      this.whenValueChangedActions.Add(member.Name, action);
      this.hasValueChangedActions = true;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      if (this.isSynchronizing || propertyChangedEventArgs?.PropertyName == null)
      {
        return;
      }

      this.isSynchronizing = true;

      this.OnSynchronize(propertyChangedEventArgs.PropertyName);

      this.isSynchronizing = false;
    }

  }
}
