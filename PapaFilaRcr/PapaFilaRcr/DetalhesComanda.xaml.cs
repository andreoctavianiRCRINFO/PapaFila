using System;
using System.Collections.Generic;
using PapaFilaRcr.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PapaFilaRcr
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetalhesComanda : ContentPage
	{
        List<HistoricoD> historicoD = new List<HistoricoD>();
        public DetalhesComanda (int codigo, decimal total)
		{
			InitializeComponent ();            
            try
            {
                Conexao.conexaoSqlLite();                
                var result = Conexao.sqliteconnection.Query<HistoricoD>("SELECT * FROM historicod where idcomanda = " + codigo);
                foreach (var valor in result)
                {
                    historicoD.Add(new HistoricoD
                    {
                        idcomanda = valor.idcomanda,
                        codigo = valor.codigo,
                        item = valor.item,
                        Id = valor.Id,
                        status = valor.status,
                        descricao = valor.descricao,
                        marca = valor.marca,
                        departamento = valor.departamento,
                        preco = Convert.ToDecimal(valor.preco.ToString("N2")),
                        quantidade = Convert.ToDecimal(valor.quantidade.ToString("N3")),
                        total = Convert.ToDecimal(valor.total.ToString("N2"))                        
                    });
                    
                    ValorTotal.Text = total.ToString("N2");
                }
                historicod.ItemsSource = historicoD;
                NumeroComanda.Text = "COMANDA N° 9999" + codigo.ToString();
            }
            catch (Exception e)
            {
                DisplayAlert("Papa Fila", e.ToString(), "Ok");
            }
        }
	}
}