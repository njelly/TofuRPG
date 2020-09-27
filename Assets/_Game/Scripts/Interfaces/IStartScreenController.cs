using System;

namespace Tofunaut.TofuRPG
{
    public enum EStartScreenState
    {
        None,
        Root,
    }

    public interface IStartScreenController
    {
        event EventHandler EnterGameRequested;

        EStartScreenState CurrentStartScreenState { get; }
        void EnterState(EStartScreenState state);
    }
}