using System;
using Plugin.DeviceInfo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PapaFilaRcr.Models;

namespace PapaFilaRcr
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfiguracoesDb : ContentPage
	{
		public ConfiguracoesDb ()
		{
			InitializeComponent ();
            dispositivo.Text = CrossDeviceInfo.Current.Id;
        }

        private void Ip_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ip.Text))
            {
                DisplayAlert("Papa Fila", "Preencha o campo IP", "Ok");
                ip.Focus();
            }
            else
            {
                porta.Focus();
            }
        }

        private void Ip_Completed_1(object sender, EventArgs e)
        {

        }

        private void Porta_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(porta.Text))
            {
                DisplayAlert("Papa Fila", "Preencha o campo porta", "Ok");
                porta.Focus();
            }
            else
            {
                nomebanco.Focus();
            }
        }

        private void Nomebanco_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nomebanco.Text))
            {
                DisplayAlert("Papa Fila", "Preencha o campo Nome do banco", "Ok");
                nomebanco.Focus();
            }
            else
            {
                usuario.Focus();
            }
        }

        private void Usuario_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usuario.Text))
            {
                DisplayAlert("Papa Fila", "Preencha o campo Usuário", "Ok");
                usuario.Focus();
            }
            else
            {
                senha.Focus();
            }
        }

        private void Senha_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(senha.Text))
            {
                DisplayAlert("Papa Fila", "Preencha o campo Senha", "Ok");
                senha.Focus();
            }
            else
            {
                if(!string.IsNullOrEmpty(ip.Text) && !string.IsNullOrEmpty(porta.Text) && !string.IsNullOrEmpty(nomebanco.Text) && !string.IsNullOrEmpty(usuario.Text))
                {
                    salvar.IsEnabled = true;
                }
            }
        }

        private void Salvar_Clicked(object sender, EventArgs e)
        {            
            Conexao.Testa(ip.Text, porta.Text, nomebanco.Text, usuario.Text, senha.Text);
            if(Conexao.erro.Length > 0)
            {
                DisplayAlert("Papa Fila", "Erro ao conectar com banco de dados " + Conexao.erro.ToString(), "Ok");
                Conexao.erro = "";
            }
            else
            {
                Conexao.criaTabelaConfDb();
                Conexao.criaTabelaHistoricoc();
                Conexao.criaTabelaHistoricod();
                if(Conexao.erroSqlLite.Length > 0)
                {
                    DisplayAlert("Papa Fila", Conexao.erroSqlLite.ToString(), "OK");
                    Conexao.erroSqlLite = "";
                }                
                var insere = new ConfDb();               
                insere.enderecoservidor = ip.Text;                
                insere.porta = porta.Text;
                insere.nomebanco = nomebanco.Text;
                insere.usuario = usuario.Text;
                insere.senha = senha.Text;
                Conexao.sqliteconnection.Insert(insere);
                DisplayAlert("Papa Fila", "Configuração gravada", "Ok");
            }
            Conexao.Desconecta();
        }
    }
}