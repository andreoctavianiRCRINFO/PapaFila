using System;
using PapaFilaRcr.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PapaFilaRcr
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfiguracoesEtiqBal : ContentPage
	{
		public ConfiguracoesEtiqBal ()
		{
			InitializeComponent ();
		}

        private void Entry_Completed(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(digitos.Text))
            {
                DisplayAlert("Papa Fila", "Informe a quantidade de dígitos dos produtos de balança", "Ok");
                digitos.Focus();
            }
            else
            {
                digverificador.Focus();
            }
        }        

        private void Salvar_Clicked(object sender, EventArgs e)
        {
            Conexao.conexaoSqlLite();
            var insere = new ConfEtiqBal();           
            insere.NumeroDigitos = Convert.ToInt32(digitos.Text);
            insere.ImprimeDigito = digverificador.SelectedItem.ToString();
            insere.ImprimePesoPreco = pesovalor.SelectedItem.ToString();            
            Conexao.sqliteconnection.InsertOrReplace(insere);
            DisplayAlert("Papa Fila", "Configuração gravada", "Ok");
        }

        private void Pesovalor_SelectedIndexChanged(object sender, EventArgs e)
        {
            salvar.IsEnabled = true;
        }

        private void Digverificador_SelectedIndexChanged(object sender, EventArgs e)
        {
            pesovalor.Focus();
        }
    }
}