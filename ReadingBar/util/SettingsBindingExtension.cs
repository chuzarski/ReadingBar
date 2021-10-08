using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace ReadingBar.util
{
    class SettingsBindingExtension : Binding
    {

        public SettingsBindingExtension()
        {
            Init();
        }

        public SettingsBindingExtension(string path) : base(path)
        {
            Init();
        }

        private void Init()
        {
            this.Source = ReadingBar.Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }


    }
}
