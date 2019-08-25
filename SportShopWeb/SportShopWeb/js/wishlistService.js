webShopApp.service('wishlistService', ['$cookieStore', '$http', '$rootScope', '$q', function ($cookieStore, $http, $rootScope, $q) {
    return {
        init: function () {
            var getWishList = function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: $rootScope.serviceURL + 'wishlists/' + $cookieStore.get('loggedUser')
                }).then(function successCallback(response) {
                    var wishlistProdustIDs = response.data;
                    deferred.resolve(wishlistProdustIDs);
                }, function errorCallback(response) {
                    wishlist = {};
                    $cookieStore.put('wishlist', wishlist);
                    alert("Eroor loading wishlist");
                });

                return deferred.promise;
            }

            getWishListProducts = function (wishlistProdustIDs) {
                var products = [];
                var promises = [];

                angular.forEach(wishlistProdustIDs, function (product) {
                    var deferred = $q.defer();

                    deferred.promise = $http({
                        method: 'GET',
                        url: $rootScope.serviceURL + 'products/' + product.ProductID
                    });

                    deferred.promise.then(function successCallback(response) {
                        products.push(response.data);
                        deferred.resolve();
                    }, function errorCallback(response) {
                        deferred.reject("Product not loaded" + response);
                    });

                    promises.push(deferred.promise);
                });

                return $q.all(promises).then(function () {
                    return products;
                });
            }

            getWishList().then(function (wishlist) {
                return getWishListProducts(wishlist.Products);
            }).then(function (wishListProducts) {
                $cookieStore.put('wishlist', wishListProducts);
                // get wishlist size
                var wishlist = $cookieStore.get('wishlist');
                if (wishlist != undefined)
                    $rootScope.wlSize = wishlist.length;
            })
        },

        getProducts: function () {
            return $cookieStore.get('wishlist');
        },

        addProduct: function (product) {
            var wishlist = $cookieStore.get('wishlist');
            if (wishlist == undefined) {
                wishlist = [];
            }
            var exists = wishlist.filter(function (p) { return p.ID == product.ID }).length > 0;
            if (!exists) {
                wishlist.push(product);
                $cookieStore.put('wishlist', wishlist);
            }
        },

        removeProduct: function (product) {
            if ($cookieStore.get('wishlist')) {
                var products = $cookieStore.get('wishlist').filter(function (p) { return p.ID != product.ID });
                $cookieStore.remove('wishlist');
                $cookieStore.put('wishlist', products);
            }
        },

        getWlSize: function () {
            var wishlist = $cookieStore.get('wishlist');
            if (wishlist != undefined) {
                return wishlist ? wishlist.length : 0;
            } else {
                return 0;
            }
        },

        clearWishList: function () {
            $cookieStore.remove('wishlist');
        }
    }
}]);