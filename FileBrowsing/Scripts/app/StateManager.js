app.factory('StateManager', ['$q', '$rootScope', function($q, $rootScope) {
            var waited = [];
            $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
                waited = []; // empty array when transition to new state
            });
            return {
                add: function() {
                    for(var i in arguments) {
                        waited.push(arguments[i]);
                    }
                    return this;
                },
                wait: function() {
                    var ret = $q.defer();
                    $q.all(waited)['finally'](function() {
                        waited = [];
                        ret.resolve(1);
                    });
                    return ret.promise;
                }
            };
        }])
.directive('ngLoaded', ['$compile', function($compile) {
    return {
        link: function(scope, element, attrs) {
            var indicator = angular.element('<div><img src="Content/loader.gif" /> Loading...</div>');
            indicator = $compile(indicator)(scope);
            element.after(indicator);
            scope.$watch(attrs.ngLoaded, function(newValue, oldValue) {
                if(newValue) {
                    element.css({display: 'block'});
                    indicator.css({display: 'none'});
                } else {
                    element.css({display: 'none'});
                    indicator.css({display: 'block'});
                }
            });
        }
    };
}]);