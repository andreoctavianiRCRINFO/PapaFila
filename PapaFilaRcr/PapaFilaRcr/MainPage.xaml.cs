using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PapaFilaRcr.Models;

namespace PapaFilaRcr
{
    public partial class MainPage : ContentPage
    {
        public string endereco;
        public string porta;
        public string nomebanco;
        public string usuario;
        public string senha;

        public MainPage()
        {
            InitializeComponent();            
        }
    }
}
