(function () {
    'use strict';

    angular.module('abioka', [
        'ngMessages',
        'ngCookies',
        'ngResource',
        'ui.router',
        'ngMaterial',
        'md.data.table',
        'angularMoment']
     ).run(run);

    /* @ngInject */
    function run($rootScope, $state, $stateParams, userService, translationService) {
        translationService.setGlobalResources();

        $rootScope.$on('$stateChangeStart', function (e, toState, toParams, fromState, fromParams) {
            var user = userService.getUser();
            if (toState.isPublic !== true && !user.IsSignedIn) {
                e.preventDefault();
                $state.transitionTo("login", null, {
                    notify: false
                });
                $state.go("login");
            }
        });
    }
})();
