var webShopApp = angular.module("webShopApp", [
    'ngRoute', 'ngCookies', 'ui-notification', 'naif.base64'
]);

webShopApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: 'templates/home.html',
        controller: 'homeController'
    }).when('/productsCard', {
        templateUrl: 'templates/productsCardview.html',
        controller: 'productController'
    }).when('/productsTable', {
        templateUrl: 'templates/productsTableview.html',
        controller: 'productController'
    }).when('/productDetails/:productId', {
        templateUrl: 'templates/productDetails.html',
        controller: 'productDetailsController'
    }).when('/saveProduct', {
        templateUrl: 'templates/add_edit_product.html',
        controller: 'productController'
    }).when('/cart', {
        templateUrl: 'templates/cart.html',
        controller: 'cartController'
    }).when('/users', {
        templateUrl: 'templates/users.html',
        controller: 'userController'
    }).when('/saveUser', {
        templateUrl: 'templates/add_edit_user.html',
        controller: 'userController'
    }).when('/wishlist', {
        templateUrl: 'templates/wishlist.html',
        controller: 'wishlistController'
    }).when('/myOrders', {
        templateUrl: 'templates/myOrders.html',
        controller: 'myOrderController'
    }).when('/orders', {
        templateUrl: 'templates/orders.html',
        controller: 'orderController'
    }).when('/account', {
        templateUrl: 'templates/account.html',
        controller: 'accountController'
    }).when('/contact', {
        templateUrl: 'templates/contact.html',
        controller: 'contactController'
    }).otherwise({
        redirectTo: '/'
    });
}]);

webShopApp.config(function (NotificationProvider) {
    NotificationProvider.setOptions({
        startTop: 20,
        startRight: 20,
        verticalSpacing: 10,
        horizontalSpacing: 10,
        positionX: 'right',
        positionY: 'bottom',
        maxCount: 3,
        delay: 3000
    });
});

webShopApp.run(function ($rootScope, $http, $window, $cookieStore, cartService, wishlistService) {

    $rootScope.serviceURL = "http://localhost:61838/api/";
    // $rootScope.serviceURL = "http://localhost:8081/api/";

    $rootScope.accessProductsPage = function () {
        if ($cookieStore.get('productsView') == 'card') {
            $window.location.href = "#!/productsCard";
        } else {
            $window.location.href = "#!/productsTable";
        }
    };

    $rootScope.goToLoginPage = function () {
        //$window.location.href = "#!/login";
    }

    $rootScope.isAdmin = function () {
        return $cookieStore.get('isAdmin');
    };

    $rootScope.isLoggedUser = function () {
        return (($cookieStore.get('loggedUser') != undefined) && ($cookieStore.get('loggedUser') != ''))
    };

    $rootScope.getLoggedUser = function () {
        return $cookieStore.get('loggedUser');
    };

    $rootScope.getCartSize = function () {
        return cartService.getCartSize();
    }

    $rootScope.getWishlistSize = function () {
        return wishlistService.getWlSize();
    }
});