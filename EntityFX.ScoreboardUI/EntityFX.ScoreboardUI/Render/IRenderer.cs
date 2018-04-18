﻿namespace EntityFX.ScoreboardUI.Render
{
    public interface IRenderer
    {
        IRenderOptions RenderOptions { get; }
        void Initialize();

        void Render(UiElement uiElement);
    }
}