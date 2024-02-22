using System;
using System.Threading.Tasks;
using System.Windows;

namespace mfc_tomsk
{
  
    public partial class Progress : Window
    {
        public Progress()
        {
            InitializeComponent();
            Pro();
        }

        private async void Pro()
        {
            await ProgressGo();
        }

        private async Task ProgressGo()
        {
            await Task.Delay(400);
            Close();
        }
    }
}
