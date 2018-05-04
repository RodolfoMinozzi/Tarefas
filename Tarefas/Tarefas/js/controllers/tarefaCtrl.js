var app = angular.module('tarefaApp', []);
app.controller('tarefaCtrl', function ($scope, tarefaAPI) {
    $scope.tarefas = [];

    function httpError(data, parent) {
        var msgErro = data.data;
        if (msgErro && msgErro.d) msgErro = msgErro.d;
        console.log(msgErro);
    }

    tarefaAPI.postTarefaTipoListar({}).then(function (data) {
        $scope.tarefaTipo = angular.fromJson(data.data.d);
    }, httpError);

    tarefaAPI.postTarefaListar({}).then(function (data) {
        $scope.tarefas = angular.fromJson(data.data.d);
    }, httpError);

    $scope.tarefaSalvar = function (tarefa) {
        tarefaAPI.postTarefaSalvar(tarefa).then(function (data) {
            tarefaRemover(tarefa.Id);
            $scope.tarefas.push(angular.fromJson(data.data.d));
            $scope.tarefa = {};
        }, httpError);
    }

    $scope.tarefaLimpar = function () {
        $scope.tarefa = {};
    }

    $scope.tarefaExcluir = function (tarefa) {
        tarefaAPI.postTarefaExcluir(tarefa).then(function (data) {
            tarefaRemover(tarefa.Id);
        }, httpError);
    }

    $scope.tarefaEditar = function (tarefa) {
        $scope.tarefa = angular.copy(tarefa);
    }

    function tarefaRemover(id) {
        $scope.tarefas = $scope.tarefas.filter(function (el) {
            return el.Id !== id;
        });
    }
});