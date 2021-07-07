using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;
using SQLite;
using System.Data.SqlClient;
using PapaFilaRcr.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PapaFilaRcr
{  
    class Conexao
    {
        private static string enderecoservidor { get; set; }
        private static string porta { get; set; }
        private static string nomebanco { get; set; }
        private static string usuario { get; set; }
        private static string senha { get; set; }
        public static SqlConnection conn { get; set; }
        public static SQLiteConnection sqliteconnection { get; set; }
        public const string DbFileName = "PapaFila.db";

        public static string erro = "";
        public static string erroSqlLite = "";

        public static void Conecta()
        {
            try
            {
                conexaoSqlLite();
                var table = sqliteconnection.Table<ConfDb>();
                foreach (var valor in table)
                {
                    enderecoservidor = valor.enderecoservidor.ToString();
                    porta = valor.porta.ToString();
                    nomebanco = valor.nomebanco.ToString();
                    usuario = valor.usuario.ToString();
                    senha = valor.senha.ToString();
                }
                var connString = "Server=" + enderecoservidor + "," + porta + ";Database=" + nomebanco + ";Uid=" + usuario + ";Pwd=" + senha;
                conn = new SqlConnection(connString);
                conn.Open();
                
            }
            catch (SqlException e)
            {
                erro = e.ToString();
                
            }
        }

        public static void Desconecta()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public static bool Testa(string endereco, string portaservidor, string banco, string usuariobanco, string senhabanco)
        {
            try
            {
                var connString = "Server=" + endereco + "," + portaservidor + ";Database=" + banco + ";Uid=" + usuariobanco + ";Pwd=" + senhabanco;
                conn = new SqlConnection(connString);
                conn.Open();
                return true;
            }
            catch (SqlException e)
            {
                erro = e.ToString();
                return false;
            }
        }

        public static void conexaoSqlLite()
        {
            try
            {
                var pasta = new LocalRootFolder();
                //cria ou abre o arquivo
                var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
                //abre o BD
                sqliteconnection = new SQLiteConnection(arquivo.Path);
            }
            catch(SQLiteException e)
            {
                erroSqlLite = e.ToString();
            }
        }

        public static void criaTabelaConfDb()
        {
            try
            {
                //cria uma pasta base local para o dispositivo
                var pasta = new LocalRootFolder();
                //cria o arquivo
                var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
                //abre o BD
                sqliteconnection = new SQLiteConnection(arquivo.Path);                
                //cria a tabela no BD
                sqliteconnection.CreateTable<HistoricoC>();
            }
            catch (Exception e)
            {
                erroSqlLite = e.ToString();
            }
        }        

        public static void criaTabelaHistoricoc()
        {
            try
            {
                //cria uma pasta base local para o dispositivo
                var pasta = new LocalRootFolder();
                //cria o arquivo
                var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
                //abre o BD
                sqliteconnection = new SQLiteConnection(arquivo.Path);
                sqliteconnection.DeleteAll<ConfDb>();
                //cria a tabela no BD
                sqliteconnection.CreateTable<ConfDb>();
            }
            catch (Exception e)
            {
                erroSqlLite = e.ToString();
            }
        }

        public static void criaTabelaHistoricod()
        {
            try
            {
                //cria uma pasta base local para o dispositivo
                var pasta = new LocalRootFolder();
                //cria o arquivo
                var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
                //abre o BD
                sqliteconnection = new SQLiteConnection(arquivo.Path);
                //cria a tabela no BD
                sqliteconnection.CreateTable<HistoricoD>();
            }
            catch (Exception e)
            {
                erroSqlLite = e.ToString();
            }
        }

        public static void criaTabelaConfEtiqBal()
        {
            try
            {
                //cria uma pasta base local para o dispositivo
                var pasta = new LocalRootFolder();
                //cria o arquivo
                var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
                //abre o BD
                sqliteconnection = new SQLiteConnection(arquivo.Path);
                //cria a tabela no BD
                sqliteconnection.CreateTable<ConfEtiqBal>();
            }
            catch (Exception e)
            {
                erroSqlLite = e.ToString();
            }
        }

        //pegar dados configuração banco de dados no sqlLite
        public List<ConfDb> GetConfigData()
        {
            return (from data in sqliteconnection.Table<ConfDb>()
                    select data).ToList();
        }

        // Inserir dados na tabela confdb sql Lite
        public void InsertPedido(ConfDb confdb)
        {
            sqliteconnection.Insert(confdb);
        }
    }
}