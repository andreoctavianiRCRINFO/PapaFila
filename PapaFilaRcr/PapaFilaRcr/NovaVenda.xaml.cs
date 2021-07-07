using System;
using ZXing.Net.Mobile.Forms;
using Xamarin.Essentials;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using PapaFilaRcr.Models;

namespace PapaFilaRcr
{    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovaVenda : ContentPage
    {
        ObservableCollection<ItensNovaVenda> Items = new ObservableCollection<ItensNovaVenda>();
        decimal ValorTotal = 0;
        int i = 1;

        public NovaVenda()
        {
            InitializeComponent();            
            Conexao.Conecta();
            quantidade.Text = "1";
            codbarras.Focus();
        }        

        private void OpenScanner(object sender, EventArgs e)
        {
            Scanner();
        }

        public async void Scanner()
        {

            var ScannerPage = new ZXingScannerPage();

            ScannerPage.OnScanResult += (result) =>
            {
                try
                {                    
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));                    
                }
                catch (Exception ex)
                {
                    DisplayAlert("Papa Fila", ex.ToString(), "Ok");
                }
                // Parar de escanear
                ScannerPage.IsScanning = false;

                //Campo cobarras recebe código lido
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    codbarras.Text = result.Text;
                    quantidade.Focus();
                });
            };


            await Navigation.PushAsync(ScannerPage);

        }

        private void Codbarras_Completed(object sender, EventArgs e)
        {
            if (codbarras.Text == "")
            {
                DisplayAlert("Papa Fila", "Informe o código de barras", "Ok");
                codbarras.Focus();
            }
            else
            {
                quantidade.Focus();
            }
        }

        private void Quantidade_Completed(object sender, EventArgs e)
        {
            if (quantidade.Text == "" || Convert.ToDouble(quantidade.Text) == 0)
            {
                DisplayAlert("Papa Fila", "Informe a quantidade", "Ok");
            }
            else
            {
                try
                {                    
                    SqlCommand buscaItem = new SqlCommand("select Id, Codigo, Descricao, Depto, Marca, ValorVenda from Produto where CodFabrica = '" + codbarras.Text + "'", Conexao.conn);
                    SqlDataReader drItem = buscaItem.ExecuteReader();
                    drItem.Read();

                    if (!drItem.HasRows)
                    {
                        drItem.Close();
                        DisplayAlert("Papa Fila", "Item não localizado", "Ok");
                        codbarras.Focus();
                    }
                    else
                    {                        
                        Items.Add(new ItensNovaVenda
                        {
                            item = i,
                            Id = drItem.GetInt32(0),
                            codigo = drItem.GetString(1),
                            descricao = drItem.GetString(2),
                            departamento = drItem.GetString(3) ?? "1",
                            marca = drItem.GetString(4) ?? "",
                            preco = Convert.ToDecimal(drItem.GetDecimal(5).ToString("N2")),
                            quantidade = Convert.ToDecimal(quantidade.Text),
                            total = Convert.ToDecimal(drItem.GetDecimal(5).ToString("N2")) * Convert.ToDecimal(quantidade.Text)
                        });

                        i++;

                        codbarras.Focus();
                        codbarras.Text = "";                        
                        ValorTotal += drItem.GetDecimal(5) * Convert.ToDecimal(quantidade.Text);
                        quantidade.Text = "1";
                        total.Text = ValorTotal.ToString("N2");
                        drItem.Close();
                        produtos.ItemsSource = Items;
                    }
                }
                catch (Exception ex)
                {                    
                    DisplayAlert("Papa Fila", ex.ToString(), "Ok");
                }
            }            
        }
        

        async void Button_Clicked(object sender, EventArgs e)
        {
            var resposta = await DisplayAlert("Papa Fila", "Confirma a gravação do lançamento", "Sim", "Não");
            if (resposta == true)
            {
                if (Items.Count > 0)
                {
                    try
                    {
                        SqlCommand PegaNumeroComanda = new SqlCommand("SELECT max(Id) from comanda_c", Conexao.conn);
                        SqlDataReader drNumComanda = PegaNumeroComanda.ExecuteReader();
                        drNumComanda.Read();

                        var ultimoid = drNumComanda.GetInt32(0) + 1;
                        drNumComanda.Close();

                        //inclui registro na tabela comanda_c
                        SqlCommand GravaComandaC = new SqlCommand("" +
                            "insert into comanda_c " +
                                "(numero, data_emissao, status, total) " +
                            "VALUES " +
                                "(9999" + ultimoid + ", '" + DateTime.Now.ToString("yyyyMMdd") + "', 'P', " + ValorTotal.ToString().Replace(",",".") + ")", Conexao.conn);

                        GravaComandaC.ExecuteNonQuery();

                        //grava registro na tabela historicoc sqlite
                        var insereHistoricoC = new HistoricoC();
                        insereHistoricoC.id = ultimoid;
                        insereHistoricoC.numero = "9999" + ultimoid.ToString();
                        insereHistoricoC.datahora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        insereHistoricoC.total = Convert.ToDecimal(ValorTotal.ToString());
                        Conexao.sqliteconnection.Insert(insereHistoricoC);

                        //grava itens na tabela comanda_d                        
                        var insereHistoricoD = new HistoricoD();
                        int i = 1;
                        foreach (var item in Items)
                        {
                            SqlCommand GravaComandaD = new SqlCommand("" +
                                "insert into comanda_d" +
                                    "(id, item, produtoid, codfabrica, descricao, departamento, status, preco, qtde, total)" +
                                "VALUES " +
                                    "(" + ultimoid + ", " + i + ", '" + item.Id + "', '" + item.codigo + "', '" + item.descricao + "', " + item.departamento + ", 'P', " + Convert.ToDecimal(item.preco.ToString().Replace(",",".")) + ", " + Convert.ToDecimal(item.quantidade.ToString().Replace(",",".")) + ", " + Convert.ToDecimal(item.quantidade.ToString().Replace(",",".")) * Convert.ToDecimal(item.preco.ToString().Replace(",",".")) + ")", Conexao.conn);
                            GravaComandaD.ExecuteNonQuery();

                            i++;

                            //grava dados na tabela historicod sqlite
                            insereHistoricoD.idcomanda = ultimoid;
                            insereHistoricoD.item = i - 1;
                            insereHistoricoD.Id = item.Id;
                            insereHistoricoD.codigo = item.codigo;
                            insereHistoricoD.descricao = item.descricao;
                            insereHistoricoD.marca = item.marca;
                            insereHistoricoD.departamento = item.departamento;
                            insereHistoricoD.status = "P";
                            insereHistoricoD.preco = item.preco;
                            insereHistoricoD.quantidade = item.quantidade;
                            insereHistoricoD.total = Convert.ToDecimal(item.quantidade.ToString()) * item.preco;
                            Conexao.sqliteconnection.Insert(insereHistoricoD);                                                       
                        }                        

                        await DisplayAlert("Papa Fila", "Dados gravados. Número da comanda: 9999" + ultimoid.ToString(), "Ok");
                        Items.Clear();
                        total.Text = "";
                        codbarras.Focus();
                        quantidade.Text = "1";
                    }
                    catch(Exception ex)
                    {
                        await DisplayAlert("Papa Fila", ex.ToString(), "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Papa Fila", "Não existem itens para gravação", "Ok");
                    codbarras.Focus();
                }
            }            
        }

        async void Cancelar_Clicked(object sender, EventArgs e)
        {
            var resposta = await DisplayAlert("Papa Fila", "Confirma o cancelamento do lançamento", "Sim", "Não");
            if (resposta == true)
            {
                Items.Clear();                
                produtos.ItemsSource = Items;
                ValorTotal = 0;
                i = 1;
                total.Text = "";
                codbarras.Focus();
            }
        }
    }
}