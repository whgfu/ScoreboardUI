﻿using System.Collections.Generic;
using System.Linq;
using EntityFX.ScoreboardUI.Elements.Scoreboards;

namespace EntityFX.ScoreboardUI
{
    internal class StoreboardNavigationEngine : IStoreboardNavigationEngine
    {
        private readonly Stack<StateItem> _scoreboardNavigationStack = new Stack<StateItem>();

        public Stack<Scoreboard> NavigationStack
        {
            get { return new Stack<Scoreboard>(_scoreboardNavigationStack.Select(_ => _.Scoreboard)); }
        }

        public StateItem Previous { get; private set; }

        public StateItem Current { get; set; }

        public void GoBack(object data = null)
        {
            Previous = _scoreboardNavigationStack.Pop();
            if (_scoreboardNavigationStack.Count != 0)
            {
                StateItem stateItem = _scoreboardNavigationStack.Peek();
                stateItem.NavigationData = data;
                Current = stateItem;
                Current.Scoreboard.IsVisible = true;
                Current.Scoreboard.NavigateInternal(NavigationType.GoBack, stateItem.NavigationData);
                Current.Scoreboard.Render();
            }
            else
            {
                Current = null;
            }
        }

        public void Navigate<TScoreboard>(object data = null) where TScoreboard : Scoreboard, new()
        {
            StateItem firstOrDefault =
                _scoreboardNavigationStack.FirstOrDefault(_ => _ is TScoreboard);
            Scoreboard scoreboard;
            if (firstOrDefault != null)
            {
                scoreboard = firstOrDefault.Scoreboard;

                if (scoreboard != null)
                {
                    Scoreboard popScoreboard = null;
                    do
                    {
                        popScoreboard = _scoreboardNavigationStack.Pop().Scoreboard;
                    } while (popScoreboard != null && scoreboard != popScoreboard);
                }
            }
            else
            {
                Current.ScoreboardState.IsNavigating = true;
                scoreboard = new TScoreboard();
            }
            Navigate(scoreboard, data);
        }

        public void Navigate(Scoreboard scoreboard, object data = null)
        {
            var stateItem = new StateItem
            {
                Scoreboard = scoreboard,
                ScoreboardState = new ScoreboardState(),
                NavigationData = data
            };

            Current.ScoreboardState.IsNavigating = false;

            Current = stateItem;

            if (_scoreboardNavigationStack.Count > 0)
            {
                StateItem currentStackItem = _scoreboardNavigationStack.Peek();
                Previous = currentStackItem;
                Previous.Scoreboard.IsVisible = false;
            }

            _scoreboardNavigationStack.Push(stateItem);
            if (!Current.ScoreboardState.ScoreboardInitialized) Current.Scoreboard.InitializeInternal();
            Current.Scoreboard.NavigateInternal(NavigationType.Navigate, stateItem.NavigationData);
            scoreboard.Render();
            ScoreboardContext.CurrentState.ResetFocus();
        }
    }
}