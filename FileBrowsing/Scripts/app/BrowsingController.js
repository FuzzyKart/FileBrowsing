app.controller('BrowsingController', function ($http, $scope, BrowsingService) {
    getAllFolders();
    $scope.folderPath = null;
    function getAllFolders() {
        loadingData();
        $http.get('/api/FileFolder/').success(function (data) {
            initializeData(data);
        })
       .error(function () {
           $scope.error = "Some Error.";
       });
    }
    $scope.getFolderByDir = function (dir) {
        loadingData();
        $http({ method:'GET', url:'/api/FileFolder/', params: {'path':dir}}).success(function (data) {
            initializeData(data);
        })
       .error(function () {
           $scope.error = "Some Error.";
       });
    }
    function initializeData(data) {
        $scope.loaderMore = false;
        $scope.Folders = data.GetFolders;
        $scope.Files = data.GetFiles;
        $scope.Current = data.GetCurrentPath;
        $scope.Count = data.GetCount;
        $scope.Parent = data.GetParentPath;
    }
    function loadingData() {
        $scope.loaderMore = true;
        $scope.lblMessage = 'loading please wait....!';
        $scope.result = "color-green";
    }
})