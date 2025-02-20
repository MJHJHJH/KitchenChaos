using System;
public static class AllEventHander
{
    public static event EventHandler<OnSelectedCounterChangedEventArgs>
        onSelectedCounterChangedEvent = null;
    public static void CallOnSelectedCounterChangedEvent(object sender, OnSelectedCounterChangedEventArgs onSelectedCounterChangedEventArgs)
    {
        onSelectedCounterChangedEvent?.Invoke(sender, onSelectedCounterChangedEventArgs);
    }

    public static event EventHandler<OnCuttingCounterChangeEventArgs>
        onCuttingCounterChangeEvent = null;
    public static void CallOnCuttingCounterChangedEvent(object sender,
     OnCuttingCounterChangeEventArgs onCuttingCounterChangeEventArgs)
    {
        onCuttingCounterChangeEvent?.Invoke(sender, onCuttingCounterChangeEventArgs);
    }
}