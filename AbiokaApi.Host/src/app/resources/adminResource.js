﻿(function () {
    'use strict';

    angular.module('abioka').factory('AdminResource', adminResource);

    /* @ngInject */
    function adminResource($resource) {
        return {
            users: $resource('./user/:id', null, {
                'update': { method:'PUT' }
            }),
            roles: $resource('./role/:id', null, {
                'update': { method: 'PUT' }
            })
        };
    }

})();