﻿namespace Tofunaut.TofuRPG
{
    public enum EAppState
    {
        None,
        Initialization,
        StartScreen,
        InGame,
    }

    public interface IAppStateMachine
    {
        EAppState CurrentAppState { get; }
        void EnterState(EAppState state);
    }

    public interface IInitializationState
    {
        bool IsComplete { get; }
    }

    public interface IStartScreenState { }
    public interface IInGameState { }
}