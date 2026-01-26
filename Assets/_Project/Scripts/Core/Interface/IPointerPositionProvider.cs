using System;

public interface IPointerPositionProvider : IService
{
    public event Action<PositionInfo> Changed;

    public PositionInfo Position { get; }
}