webShopApp.controller('mainController', ['$scope', '$rootScope', '$window', '$cookieStore', 'cartService', 'wishlistService', '$http', '$location', function ($scope, $rootScope, $window, $cookieStore, cartService, wishlistService, $http, $location) {

    if ($cookieStore.get('productsView') == undefined) {
        $cookieStore.put('productsView', 'card');
    }

    $rootScope.cartSize = $rootScope.getCartSize();
    $rootScope.wlSize = $rootScope.getWishlistSize();

    $scope.logout = function(){
        $cookieStore.remove("loggedUser");
        $cookieStore.remove("isAdmin");
        $cookieStore.remove('cart');
        $cookieStore.remove('productsView');
        cartService.clearCart();

        $window.location.href = "#!/account";
        $rootScope.user = null;
    }

    $scope.editMyProfile = function() {
        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'users/' + $rootScope.getLoggedUser()
        }).then(function successCallback(response) {
            $rootScope.user = response.data;
            $location.path("saveUser");
        }, function errorCallback(response) {
            console.warn("User not loaded" + response);
        });
    }
}]);