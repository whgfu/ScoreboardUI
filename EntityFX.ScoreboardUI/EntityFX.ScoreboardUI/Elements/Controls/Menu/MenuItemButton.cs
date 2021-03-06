﻿using System;
using System.Collections.Generic;
using EntityFX.ScoreboardUI.Drawing;
using EntityFX.ScoreboardUI.Elements.Controls.StatusBar;

namespace EntityFX.ScoreboardUI.Elements.Controls.Menu
{
    public class MenuItemButton<TData> : MenuItem<TData>
    {
        private readonly IList<MenuItemButton<TData>> _subMenuItems = new List<MenuItemButton<TData>>();

        public IEnumerable<MenuItemButton<TData>> SubMenuItems
        {
            get { return _subMenuItems; }
        }

        public virtual void AddChild(MenuItemButton<TData> childControl)
        {
            _subMenuItems.Add(childControl);
            childControl.Parent = this;
            //ScoreboardContext.CurrentState.AddToControlList(childControl);
            //childControl.CompositionLevel = CompositionLevel + 1;
        }

        public MenuItemButton()
        {
            BackgroundColor = ScoreboardContext.RenderEngine.RenderOptions.ColorScheme.ButtonsBackgroundColor;
            ForegroundColor = ScoreboardContext.RenderEngine.RenderOptions.ColorScheme.ButtonsForegroundColor;
            Location = new Point() { Top = 0 };
            InternalControl = new Button
            {
                Text = Text,
                Parent = this,
                Location = new Point() { Top = 0 },
                Position = PositionEnum.RELATIVE,
                IsEnabled = IsEnabled
            };
        }

        public override bool IsEnabled
        {
            get
            {
                return base.IsEnabled;
            }
            set
            {
                if (InternalControl != null) {
                    InternalControl.IsEnabled = value;
                }

                base.IsEnabled = value;
            }
        }
    }
}