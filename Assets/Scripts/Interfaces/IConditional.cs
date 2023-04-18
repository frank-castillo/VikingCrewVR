using System;

public interface IConditional
{
    public bool IsTrue();
    public Action OnComplete { get; set; }
}