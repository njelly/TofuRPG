using System;

namespace Tofunaut.TofuRPG
{
    public interface IInitializationController
    {
        bool IsComplete { get; }
        event EventHandler OnComplete;
    }
}