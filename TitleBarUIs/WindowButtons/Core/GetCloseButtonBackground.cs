﻿using System;
using System.Globalization;
using System.Windows;

namespace TitleBarUIs
{
    /// <inheritdoc href="MarkupConverter"/>
    /// <summary>
    /// Get CloseButton Background, in Hover and Pressed process.
    /// </summary>
    internal class GetCloseButtonBackground : MarkupConverter
    {
        public string HoverMode { set; get; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DependencyObject d)) throw new ArgumentException("GetCloseButtonBackground: Value must be not Null", nameof(value));

            if (HoverMode == "Pressed")
            {
                var pr = d.GetValue(WindowButtonsBase.CloseButtonPressedBackgroundProperty);  //at first directly
                return pr ?? (d.GetValue(WindowButtonsBase.CloseButtonHoverBackgroundProperty) //level1 hover 
                              ?? d.GetValue(WindowButtonsBase.ButtonsPressedBackgroundProperty)); //level2 common
            }
            else
                //if (HoverMode == "Hover")
            {
                var pr = d.GetValue(WindowButtonsBase.CloseButtonHoverBackgroundProperty);
                return pr ?? d.GetValue(WindowButtonsBase.ButtonsHoverBackgroundProperty);
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Method not implement in GetCloseButtonBackground.ConvertBack:");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
