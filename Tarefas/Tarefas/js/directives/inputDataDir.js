angular.module("tarefaApp").directive("inputData", function ($filter) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function ($scope, $element, $attrs, $ctrl) {

            var _formataData = function (data) {
                data = data.replace(/[^0-9]+/g, "");
                if (data.length >= 5) data = data.substring(0, 2) + "/" + data.substring(2, 4) + "/" + data.substring(4, 8);
                else if (data.length >= 3) data = data.substring(0, 2) + "/" + data.substring(2);
                return data;
            }

            $element.bind("keypress", function (e) {
                var key = e.which ? e.which : e.keyCode;
                var pres = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57];

                if (pres.indexOf(key) < 0)
                    e.preventDefault();
            });

            $element.bind("keyup change", function () {
                $ctrl.$setViewValue(_formataData($ctrl.$viewValue));
                $ctrl.$render();
            });

            $element.bind("blur", function () {
                var RegExPattern = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                var value = $ctrl.$viewValue;
                if (value && value != '' && !value.match(RegExPattern)) {
                    $ctrl.$setViewValue("");
                    $ctrl.$render();
                }
            });

            $ctrl.$formatters.unshift(function (modelValue) {
                if (!modelValue) {
                    $ctrl.$setViewValue("1901-01-01T00:00:00");
                    return "";
                }
                else if (modelValue == "1901-01-01T00:00:00" || modelValue == "0001-01-01T00:00:00") return "";
                var date = new Date(modelValue);
                var retVal = $filter('date')(date, "dd/MM/yyyy");
                return retVal;
            });

            $ctrl.$parsers.unshift(function (viewValue) {
                if (viewValue && viewValue.length == 10) {
                    viewValue = viewValue.replace(/[^0-9]+/g, "");
                    var ano = parseInt(viewValue.substring(4));
                    var mes = parseInt(viewValue.substring(2, 4)) - 1;
                    var dia = parseInt(viewValue.substring(0, 2));

                    var date = new Date(ano, mes, dia);
                    return $filter('date')(date, "yyyy-MM-ddTHH:mm:ss");
                }
                return "0001-01-01T00:00:00";
            });
        }
    }
});