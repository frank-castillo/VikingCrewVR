using System;

public interface IConditional
{
    bool IsTrue();
    Action OnComplete { get; set; }
}