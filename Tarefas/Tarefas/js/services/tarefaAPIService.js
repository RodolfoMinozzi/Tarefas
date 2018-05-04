angular.module("tarefaApp").factory('tarefaAPI', function ($http) {

    var _postTarefaSalvar = function (tarefa) {
        return $http(postTarefa('TarefaSalvar', { tarefa: angular.toJson(tarefa) }));
    };

    var _postTarefaExcluir = function (tarefa) {
        return $http(postTarefa('TarefaExcluir', { tarefa: angular.toJson(tarefa) }));
    };

    var _postTarefaListar = function (tarefa) {
        return $http(postTarefa('TarefaListar', { tarefa: angular.toJson(tarefa) }));
    };

    var _postTarefaTipoSalvar = function (tarefa) {
        return $http(postTarefa('TarefaTipoSalvar', { tarefaTipo: angular.toJson(tarefa) }));
    };

    var _postTarefaTipoExcluir = function (tarefa) {
        return $http(postTarefa('TarefaTipoExcluir', { tarefaTipo: angular.toJson(tarefa) }));
    };

    var _postTarefaTipoListar = function (tarefa) {
        return $http(postTarefa('TarefaTipoListar', { tarefaTipo: angular.toJson(tarefa) }));
    };

    return {
        postTarefaSalvar: _postTarefaSalvar,
        postTarefaExcluir: _postTarefaExcluir,
        postTarefaListar: _postTarefaListar,

        postTarefaTipoSalvar: _postTarefaTipoSalvar,
        postTarefaTipoExcluir: _postTarefaTipoExcluir,
        postTarefaTipoListar: _postTarefaTipoListar
    };

    function postTarefa(servico, dados) {
        return {
            method: 'POST',
            url: 'Services/Tarefas.asmx/' + servico,
            headers: { 'Content-Type': 'application/json' },
            responseType: 'json',
            transformResponse: function (data) {
                return angular.fromJson(data);
            },
            data: dados
        }
    }

});