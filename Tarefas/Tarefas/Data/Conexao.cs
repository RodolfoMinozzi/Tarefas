using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Tarefas.Data
{
    /// <summary>
    /// Rodolfo, 03/05/2018, Classe responsável pela conexão com banco de dados SQL.
    /// </summary>
    public class Conexao : IDisposable
    {
        bool disposed = false;
        SqlCommand cmd;
        public SqlDataReader Dr;

        /// <summary>
        /// Rodolfo Minozzi, 03/05/2018, Abrir conexão com banco de dados
        /// </summary>
        /// <param name="conString">String de conexão com banco de dados SQL</param>
        public Conexao(string conString)
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(conString);
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Sem conexão com Banco de Dados, entre em contato com Suporte!", ex);
            }
        }

        /// <summary>
        /// Verificar se o conteudo é uma data valida
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool IsDate(object data)
        {
            try
            {
                if (!DateTime.TryParse(data.ToString(), out DateTime date)) return false;
                DateTime dataPadrao = new DateTime(1901, 1, 1);
                if (date.Date != dataPadrao) return true;
                else return false;
            }
            catch { return false; }
        }

        public void Param(string parametro, object valor)
        {
            try
            {
                if (cmd.Parameters.IndexOf(parametro) >= 0) cmd.Parameters.RemoveAt(parametro);//caso já exista, será removido para evitar erros
                if (valor is DateTime && !IsDate(valor)) valor = DBNull.Value;
                else
                {
                    if (valor == null) valor = string.Empty;
                    if (valor is bool) valor = Convert.ToInt32(valor);
                }
                cmd.Parameters.AddWithValue(parametro, valor);
            }
            catch (Exception e) { throw new Exception("Erro Param", e); }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Executa a query
        /// </summary>
        /// <param name="query">Query a ser executada no banco</param>
        public void Execute(string query)
        {
            try
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) { throw new Exception("Erro Execute: " + Convert.ToString(query), e); }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Executa a query e retorna o novo ID quando identity
        /// </summary>
        /// <param name="query">Query a ser executada no banco</param>
        /// <returns></returns>
        public int ExecuteId(string query)
        {
            try
            {
                cmd.CommandText = Convert.ToString(query);
                var id = cmd.ExecuteScalar();

                int.TryParse(Convert.ToString(id), out int val);
                return val;
            }
            catch (Exception e) { throw new Exception("Erro ExecuteId: " + Convert.ToString(query), e); }
        }


        /// <summary>
        /// Rodolfo, 03/05/2018, Traz um resultado de uma query (select)
        /// </summary>
        /// <param name="query">Query a ser executada no banco</param>
        public DataTable Result(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                cmd.CommandText = query;
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    adp.FillLoadOption = LoadOption.PreserveChanges;
                    dt.BeginLoadData();
                    adp.Fill(dt);
                    dt.EndLoadData();
                }
                return dt;
            }
            catch (Exception e) { throw new Exception("Erro Result: " + Convert.ToString(query), e); }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Destroi o objeto cmd conectado ao banco
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (cmd?.Connection != null && cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();

            if (disposing)
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }

            disposed = true;
        }
    }
}