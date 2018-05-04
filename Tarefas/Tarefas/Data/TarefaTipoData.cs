using System;
using System.Collections.Generic;
using System.Data;
using Tarefas.Models;

namespace Tarefas.Data
{
    public class TarefaTipoData
    {
        /// <summary>
        /// CREATE TABLE TarefaTipo (Id int identity(1,1) primary key, Descricao varchar(30) not null) 
        /// </summary>

        string con;

        /// <summary>
        /// Rodolfo, 03/05/2018, Trabalhar com os dados da Tarefa
        /// </summary>
        /// <param name="strConn">Conexão com banco de dados.</param>
        public TarefaTipoData(string strConn)
        {
            con = strConn;
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Salvar Tarefa
        /// </summary>
        /// <param name="tarefa">Tarefa a ser salva</param>
        public void Salvar(TarefaTipo tarefaTipo)
        {
            using (Conexao c = new Conexao(con))
            {
                c.Param("@Id", tarefaTipo.Id);
                c.Param("@Descricao", tarefaTipo.Descricao);

                string sQuery;
                if (tarefaTipo.Id > 0)
                {
                    c.Param("@Id", tarefaTipo.Id);
                    sQuery = "UPDATE TarefaTipo SET Descricao=@Descricao WHERE Id=@Id";
                    c.Execute(sQuery);
                }
                else
                {
                    sQuery = "INSERT INTO TarefaTipo (Descricao) VALUES (@Descricao); SELECT @@IDENTITY;";
                    tarefaTipo.Id = c.ExecuteId(sQuery);
                }
            }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Excluir Tarefa
        /// </summary>
        /// <param name="tarefa">Tarefa a ser excluirda</param>
        public void Excluir(TarefaTipo tarefaTipo)
        {
            using (Conexao c = new Conexao(con))
            {
                c.Param("@Id", tarefaTipo.Id);
                string sQuery = "DELETE TarefaTipo WHERE Id=@Id";
                c.Execute(sQuery);
            }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Listar Tarefa
        /// </summary>
        /// <param name="tarefa">Filtro para listar a tarefa</param>
        public List<TarefaTipo> List(TarefaTipo tarefaTipo)
        {
            List<TarefaTipo> lstTarefasTipo = new List<TarefaTipo>();
            using (Conexao c = new Conexao(con))
            {
                string sQuery = "SELECT Id,Descricao FROM TarefaTipo";
                if (tarefaTipo != null && tarefaTipo.Id > 0)
                {
                    c.Param("@Id", tarefaTipo.Id);
                    sQuery += " WHERE Id=@Id";
                }

                DataTable dt = c.Result(sQuery);
                foreach (DataRow r in dt.Rows)
                {
                    TarefaTipo t = new TarefaTipo();
                    t.Id = int.Parse(Convert.ToString(r["Id"]));
                    t.Descricao = Convert.ToString(r["Descricao"]);
                    lstTarefasTipo.Add(t);
                }
            }
            return lstTarefasTipo;
        }
    }
}