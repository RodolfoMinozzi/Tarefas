using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using Tarefas.Data;
using Tarefas.Models;

namespace Tarefas.Services
{
    /// <summary>
    /// Rodolfo Minozzi, 03/05/2018
    /// Serviço para chamada dos métodos responsáveis pela comunicação com bando para as Tarefas
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    [System.Web.Script.Services.ScriptService]
    public class Tarefas : System.Web.Services.WebService
    {

        static string sConn = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TarefaSalvar(string tarefa)
        {
            try
            {
                Tarefa tar = JsonConvert.DeserializeObject<Tarefa>(tarefa, settings);
                TarefaData td = new TarefaData(sConn);
                td.Salvar(tar);

                return JsonConvert.SerializeObject(tar);
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TarefaExcluir(string tarefa)
        {
            try
            {
                Tarefa tar = JsonConvert.DeserializeObject<Tarefa>(tarefa, settings);
                TarefaData td = new TarefaData(sConn);
                td.Excluir(tar);

                return JsonConvert.SerializeObject(tar);
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TarefaListar(string tarefa)
        {
            try
            {
                Tarefa tar = JsonConvert.DeserializeObject<Tarefa>(tarefa, settings);
                TarefaData td = new TarefaData(sConn);
                List<Tarefa> lstTarefa = td.List(tar);

                return JsonConvert.SerializeObject(lstTarefa);
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TarefaTipoSalvar(string tarefaTipo)
        {
            try
            {
                TarefaTipo tipo = JsonConvert.DeserializeObject<TarefaTipo>(tarefaTipo, settings);

                TarefaTipoData td = new TarefaTipoData(sConn);
                td.Salvar(tipo);

                return JsonConvert.SerializeObject(tipo);
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TarefaTipoExcluir(string tarefaTipo)
        {
            try
            {
                TarefaTipo tarTp = JsonConvert.DeserializeObject<TarefaTipo>(tarefaTipo, settings);
                TarefaTipoData td = new TarefaTipoData(sConn);
                td.Excluir(tarTp);

                return JsonConvert.SerializeObject(tarTp);
            }
            catch (Exception e) { return e.Message; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TarefaTipoListar(string tarefaTipo)
        {
            try
            {
                TarefaTipo tarTp = JsonConvert.DeserializeObject<TarefaTipo>(tarefaTipo, settings);
                TarefaTipoData td = new TarefaTipoData(sConn);
                List<TarefaTipo> lstTarefaTipo = td.List(tarTp);
                return JsonConvert.SerializeObject(lstTarefaTipo);
            }
            catch (Exception e) { return e.Message; }
        }
    }
}
