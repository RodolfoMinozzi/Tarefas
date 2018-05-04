//Autor: Rodolfo Minozzi
//Data: 03/05/2018
// *** Controller reponsável pelos aventos na tela de tarefa

var app = angular.module('tarefaApp', []);
app.controller('tarefaCtrl', function ($scope, tarefaAPI) {
    $scope.tarefas = [];

    //Tratamento de erro do http
    function httpError(data, parent) {
        var msgErro = data.data;
        if (msgErro && msgErro.d) msgErro = msgErro.d;
        console.log(msgErro);
    }

    //Bucar os tipos da tarefa para o select
    tarefaAPI.postTarefaTipoListar({}).then(function (data) {
        $scope.tarefaTipo = angular.fromJson(data.data.d);
    }, httpError);

    //Bucar a lista de tarefas para a table
    tarefaAPI.postTarefaListar({}).then(function (data) {
        $scope.tarefas = angular.fromJson(data.data.d);
    }, httpError);

    //Salvar o objeto tarefa, e adicionar a nova tarefa a table
    $scope.tarefaSalvar = function (tarefa) {
        tarefaAPI.postTarefaSalvar(tarefa).then(function (data) {
            tarefaRemover(tarefa.Id);
            $scope.tarefas.push(angular.fromJson(data.data.d));
            $scope.tarefa = {};
        }, httpError);
    }

    //Limpa o objeto tarefa para limpar os componentes da tela
    $scope.tarefaLimpar = function () {
        $scope.tarefa = {};
    }

    //Exclui a tarefa e remove da table
    $scope.tarefaExcluir = function (tarefa) {
        tarefaAPI.postTarefaExcluir(tarefa).then(function (data) {
            tarefaRemover(tarefa.Id);
        }, httpError);
    }

    //Abre o conteudo da tarefa na tela para edição
    $scope.tarefaEditar = function (tarefa) {
        $scope.tarefa = angular.copy(tarefa);
    }

    //Remove uma tarefa do array utilizado na table
    function tarefaRemover(id) {
        $scope.tarefas = $scope.tarefas.filter(function (el) {
            return el.Id !== id;
        });
    }
});