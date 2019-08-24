webShopApp.controller('productDetailsController', ['$scope', '$route', '$q', '$routeParams', '$rootScope', '$http', '$location', '$cookieStore', 'cartService', 'wishlistService', 'Notification', function ($scope, $route, $q, $routeParams, $rootScope, $http, $location, $cookieStore, cartService, wishlistService, Notification) {

    function getProduct() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'products/' + $routeParams.productId
        }).then(
            function successCallback(response) {
                $scope.product = response.data;
                deferred.resolve($scope.product);
            },
            function errorCallback(response) {
                deferred.reject("Product not loaded" + response);
            });

        return deferred.promise;
    }

    function getCategories() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'categories'
        })
            .then(function successCallback(response) {
                deferred.resolve(response.data);
            }, function errorCallback(response) {
                deferred.reject("Categories not loaded" + response);
            });

        return deferred.promise;
    }

    function setProductsImages(product) {
        promises = [];
        products = [product];

        angular.forEach(products, function (product) {
            var deferred = $q.defer();
            promises.push(deferred.promise);

            if (product.DisplayImageID) {
                deferred.promise = $http({
                    method: 'GET',
                    url: $rootScope.serviceURL + 'images/' + product.DisplayImageID
                }).then(function (response) {
                    product["DisplayImageURL"] = response.data.URL;
                    deferred.resolve();
                }, function errorCallback(response) {
                    deferred.reject("Image not found for product " + product.Name);
                });
            }
            else {
                deferred.resolve(product);
            }
        });

        return $q.all(promises).then(function () {
            return products;
        });
    }

    getProduct()
        .then(function (product) {
            return setProductsImages(product);
        })
        .then(function (productsWithImages) {
            $scope.products = productsWithImages;
            return getCategories();
        })
        .then(function (categories) {
            $scope.categories = categories;

            // set categories
            angular.forEach($scope.products, function (product, index) {
                $scope.products[index]["Category"] = categories.find(function (category) { return category.ID == $scope.products[index].CategoryID; }).Name;
            });

            // set sizes
            $scope.sizes = [];
            angular.forEach($scope.products, function (product) {
                angular.forEach(product.Size.split(","), function (size) {
                    $scope.sizes.push(size.trim());
                });
            });
            $scope.sizes = $scope.sizes.filter(function (value, index) { return $scope.sizes.indexOf(value) == index });
            $scope.product = $scope.products[0];
        });

    $scope.addToWishlist = function (product) {
        if ($rootScope.getLoggedUser()) {
            var request = {
                'UserID': $rootScope.getLoggedUser(),
                'Products': [{ 'ProductID': product.ID }]
            };
            $http({
                method: 'POST',
                url: $rootScope.serviceURL + 'wishlists',
                data: request
            }).then(function successCallback(response) {
                var initialSize = wishlistService.getWlSize();
                wishlistService.addProduct(product);
                $rootScope.wlSize = wishlistService.getWlSize();
                if (initialSize != $rootScope.wlSize)
                    Notification.success('Product wishlisted');
                else
                    Notification.error('Product already wishlisted');
            }, function errorCallback(response) {
                Notification.error("Fail to save wishlisted product: " + response.data.Message);
            });
        }
        else {
            Notification.error('Please log in');
        }
    }

    $scope.addToCart = function (product) {
        if ($rootScope.getLoggedUser()) {
            $http({
                method: 'POST',
                url: $rootScope.serviceURL + 'carts',
                data: { 'UserID': $rootScope.getLoggedUser(), 'Products': [{ 'ProductID': product.ID, 'Quantity': 1 }] }
            }).then(function successCallback(response) {
                cartService.addProduct(product);
                Notification.success('Added to cart');
            }, function errorCallback(response) {
                Notification.error('Error adding to cart');
            });
        }
        else {
            Notification.error('Please log in');
        }
    }

    $scope.gotoEditProduct = function (product) {
        $rootScope.product = product;
        $location.path("saveProduct");
    };
}]);