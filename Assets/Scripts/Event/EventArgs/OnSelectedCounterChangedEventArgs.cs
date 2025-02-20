using System;
public class OnSelectedCounterChangedEventArgs : EventArgs
{
    public BaseCounter baseCounter;
    public OnSelectedCounterChangedEventArgs(BaseCounter _baseCounter)
    {
        this.baseCounter = _baseCounter;
    }

}