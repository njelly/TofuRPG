using System;

namespace Tofunaut.TofuRPG
{
    public interface IStartScreenRootController
    {
        event EventHandler PlayGameRequested;
        event EventHandler QuitGameRequested;
    }
}