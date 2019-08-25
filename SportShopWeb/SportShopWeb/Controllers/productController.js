webShopApp.controller('productController', ['$scope', '$route', '$rootScope', '$http', '$location', '$cookieStore', 'cartService', 'wishlistService', '$q', 'Notification', function ($scope, $route, $rootScope, $http, $location, $cookieStore, cartService, wishlistService, $q, Notification) {
    if (!$rootScope.isAdmin() && $location.path() == "/saveProduct") {
        $location.path("/");
    }

    $scope.file = {};

    $scope.removeDisplayImage = function () {
        if (confirm("Remove image?")) {
            $scope.file = null;
            $scope.product.DisplayImageURL = "";
        }
    }

    $scope.changeView = function (viewType) {
        $cookieStore.put('productsView', viewType);
    };

    $scope.editingProduct = $scope.product;

    function getProducts() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'products'
        })
            .then(function successCallback(response) {
                deferred.resolve(response.data);
            }, function errorCallback(response) {
                deferred.reject("Products not loaded" + response);
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

    function setProductsImages(products) {
        promises = [];

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

    getProducts()
        .then(function (products) {
            return setProductsImages(products);
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
        });

    $scope.checkProduct = function (product) {
        product.checked = !product.checked;
    };

    function uploadImage(base64Image, productID) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: $rootScope.serviceURL + 'images',
            data: { "ProductID": productID, "Base64EncodedImage": base64Image }
        }).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject("Categories not loaded" + response);
        });

        return deferred.promise;
    }

    $scope.addProduct = function (product) {
        $http({
            method: 'POST',
            url: $rootScope.serviceURL + 'products',
            data: product
        }).then(function successCallback(response) {
            var newProduct = response.data;
            uploadImage($scope.file.base64, newProduct.ID).then(function (result) {
                newProduct.DisplayImageID = result.ID;
                $scope.editProduct(newProduct, true).then(function () {
                    Notification.success('Product saved');
                    $rootScope.accessProductsPage();
                });
            },
                function () {
                    Notification.error('Image is too big');
                    $scope.file = {};
                });
        }, function errorCallback(response) {
            Notification.error("Fail to save product: " + response.data.Message);
        });
    };

    $scope.editProduct = function (product, afterAdd) {
        var deferred = $q.defer();

        $http({
            method: 'PUT',
            url: $rootScope.serviceURL + 'products/' + product.ID,
            data: product
        }).then(function successCallback(response) {
            var newProduct = response.data;
            if ($scope.file && $scope.file.base64 && !afterAdd) {
                uploadImage($scope.file.base64, newProduct.ID).then(function (result) {
                    $scope.file = {};
                    newProduct.DisplayImageID = result.ID;
                    $scope.editProduct(newProduct, true).then(function () {
                        Notification.success('Product saved');
                        $location.path("productDetails/" + product.ID);
                        deferred.resolve(newProduct);
                    });
                },
                    function () {
                        Notification.error('Image is too big');
                        $scope.file = {};
                    });
            }
            else {
                if (!afterAdd)
                    Notification.success('Product saved');
                $location.path("productDetails/" + product.ID);
                deferred.resolve(response);
            }
        }, function errorCallback(response) {
            if (!afterAdd)
                Notification.error("Fail to save product: " + response.data.Message);
            deferred.reject("Fail to save product: " + response.data.Message);
        });

        return deferred.promise;
    };

    $scope.deleteProduct = function (product) {
        if (confirm("Are you sure?")) {
            $http({
                method: 'DELETE',
                url: $rootScope.serviceURL + 'products/' + product.ID
            }).then(function successCallback(response) {
                Notification.success('Product deleted');
                cartService.removeProduct(product);
                $route.reload();
            }, function errorCallback(response) {
                Notification.error("Fail to delete product: " + response.data.Message);
            });
        }
    };

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

    //$scope.deleteProducts = function (products) {
    //    if (confirm("Are you sure?")) {
    //        angular.forEach(products, function (product) {
    //            if (product.checked) {
    //                $http({
    //                    method: 'DELETE',
    //                    url: $rootScope.serviceURL + "products/" + product.ID
    //                }).then(
    //                    function success(data) {
    //                        cartService.removeProduct(product);
    //                        $scope.products = $scope.products.filter(function (prod) {
    //                            return (prod.name != product.name);
    //                        });
    //                    },
    //                    function error(data) {
    //                        alert("fail deleting: " + JSON.stringify({ data: data }));
    //                    });
    //            }
    //        });
    //    }
    //};

    //$scope.addProductsToCart = function (products) {
    //    var size = 0;
    //    angular.forEach(products, function (product) {
    //        if (product.checked) {
    //            size++;
    //            cartService.addProduct(product);
    //            product.checked = false;
    //        }
    //    });
    //    alert(size + " products added to cart");
    //    $rootScope.cartSize = cartService.getCartSize();
    //};

    //$scope.addProductsToWishlist = function (products) {
    //    var size = 0;
    //    angular.forEach(products, function (product) {
    //        if (product.checked) {
    //            size++;
    //            wishlistService.addProduct(product);
    //            product.checked = false;
    //        }
    //    });
    //    alert(size + " products added to wishlist");
    //    $rootScope.wlSize = wishlistService.getWlSize();
    //    wishlistService.saveWishlist();
    //};

    $scope.gotoEditProduct = function (product) {
        $rootScope.product = product;
        $location.path("saveProduct");
    };

    $scope.gotoAddProduct = function () {
        $rootScope.product = undefined;
        $location.path("saveProduct");
    };
}]);