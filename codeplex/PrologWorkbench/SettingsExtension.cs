/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows.Data;

namespace Prolog.Workbench
{
    public class SettingsExtension : Binding
    {
        public SettingsExtension()
        {
            Initialize();
        }

        public SettingsExtension(string path)
            : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Source = Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
