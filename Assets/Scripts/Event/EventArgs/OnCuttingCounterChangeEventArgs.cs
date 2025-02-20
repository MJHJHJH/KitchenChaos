using System;
public class OnCuttingCounterChangeEventArgs : EventArgs
{
    public BaseCounter baseCounter;
    public OnCuttingCounterChangeEventArgs(BaseCounter _baseCounter)
    {
        this.baseCounter = _baseCounter;
    }

}