using System;
using System.Collections.Generic;
using System.Data;
using Tarefas.Models;

namespace Tarefas.Data
{
    /// <summary>
    /// Rodolfo, 03/05/2018, Trabalhar com os dados da Tarefa
    /// </summary>
    public class TarefaData
    {
        /// <summary>
        /// CREATE TABLE Tarefa (Id int identity(1,1) primary key, TipoId int not null, Descricao varchar(30) not null, Data datetime not null) 
        /// </summary>

        string con;

        /// <summary>
        /// Rodolfo, 03/05/2018, Trabalhar com os dados da Tarefa
        /// </summary>
        /// <param name="strConn">Conexão com banco de dados.</param>
        public TarefaData(string strConn)
        {
            con = strConn;
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Salvar Tarefa
        /// </summary>
        /// <param name="tarefa">Tarefa a ser salva</param>
        public void Salvar(Tarefa tarefa)
        {
            using (Conexao c = new Conexao(con))
            {
                c.Param("@TipoId", tarefa.Tipo.Id);
                c.Param("@Descricao", tarefa.Descricao);
                c.Param("@Data", tarefa.Data);

                string sQuery;
                if (tarefa.Id > 0)
                {
                    c.Param("@Id", tarefa.Id);
                    sQuery = "UPDATE Tarefa SET TipoId=@TipoId,Descricao=@Descricao,Data=@Data WHERE Id=@Id";
                    c.Execute(sQuery);
                }
                else
                {
                    sQuery = "INSERT INTO Tarefa (TipoId,Descricao,Data) VALUES (@TipoId,@Descricao,@Data); SELECT @@IDENTITY;";
                    tarefa.Id = c.ExecuteId(sQuery);
                }

            }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Excluir Tarefa
        /// </summary>
        /// <param name="tarefa">Tarefa a ser excluirda</param>
        public void Excluir(Tarefa tarefa)
        {
            using (Conexao c = new Conexao(con))
            {
                c.Param("@Id", tarefa.Id);
                string sQuery = "DELETE Tarefa WHERE Id=@Id";
                c.Execute(sQuery);
            }
        }

        /// <summary>
        /// Rodolfo, 03/05/2018, Listar Tarefa
        /// </summary>
        /// <param name="tarefa">Filtro para listar a tarefa</param>
        public List<Tarefa> List(Tarefa tarefa)
        {
            List<Tarefa> lstTarefas = new List<Tarefa>();
            using (Conexao c = new Conexao(con))
            {
                string sQuery = "SELECT T.Id,T.TipoId,TT.Descricao TipoDescricao,T.Descricao,T.Data FROM Tarefa T LEFT JOIN TarefaTipo TT on T.TipoId=TT.Id";
                if (tarefa.Id > 0)
                {
                    c.Param("@Id", tarefa.Id);
                    sQuery += " WHERE Id=@Id";
                }

                DataTable dt = c.Result(sQuery);
                foreach (DataRow r in dt.Rows)
                {
                    Tarefa t = new Tarefa();
                    t.Id = int.Parse(Convert.ToString(r["Id"]));
                    t.Descricao = Convert.ToString(r["Descricao"]);
                    if (c.IsDate(r["Data"])) t.Data = Convert.ToDateTime(r["Data"]);
                    t.Tipo = new TarefaTipo();
                    t.Tipo.Id = int.Parse(Convert.ToString(r["TipoId"]));
                    t.Tipo.Descricao = Convert.ToString(r["TipoDescricao"]);
                    lstTarefas.Add(t);
                }
            }
            return lstTarefas;
        }
    }
}