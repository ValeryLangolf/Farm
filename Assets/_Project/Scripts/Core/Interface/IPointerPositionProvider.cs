using System;

public interface IPointerPositionProvider
{
    public event Action<PositionInfo> Changed;
}