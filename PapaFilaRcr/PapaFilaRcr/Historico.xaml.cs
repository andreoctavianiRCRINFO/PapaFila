using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PapaFilaRcr.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PapaFilaRcr
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Historico : ContentPage
	{
        List<HistoricoC> historico = new List<HistoricoC>();
		public Historico ()
		{
			InitializeComponent ();
            try
            {
                Conexao.conexaoSqlLite();
                var table = Conexao.sqliteconnection.Table<HistoricoC>();
                foreach (var valor in table)
                {
                    historico.Add(new HistoricoC {
                        id = valor.id,
                        numero = valor.numero,
                        datahora = valor.datahora,
                        total = valor.total
                    });
                }
                historicoc.ItemsSource = historico;
            }
            catch (Exception e)
            {
                DisplayAlert("Papa Fila", e.ToString(), "Ok");
            }
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var detalhes = e.Item as HistoricoC;
            await Navigation.PushAsync(new DetalhesComanda(detalhes.id, detalhes.total) { Title = "Detalhes da comanda" });
        }
    }
}