using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PapaFilaRcr
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Menu : MasterDetailPage
	{
		public Menu ()
		{
			InitializeComponent ();
            Detail = new NavigationPage(new MainPage());
        }

        private void NovaVenda(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync(new NovaVenda() { Title = "Nova venda" });            
            IsPresented = false;
        }
        private void Historico(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync(new Historico() { Title = "Histórico de lançamentos" });
            IsPresented = false;
        }
        private void Configuracoes(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync(new ConfiguracoesDb());
            IsPresented = false;
        }
    }
}