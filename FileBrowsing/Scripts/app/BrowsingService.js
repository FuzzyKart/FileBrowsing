app.service("BrowsingService", function ($http) {

    this.getFolders = function () {
        var url = 'api/FileFolder';
        return $http.get(url).success(function (response) {
            return response.data;
        });
    }
    /*this.getFolder = function (dir) {
        var url = 'api/FileFolder/' + dir;
        return $http.get(url, { params: { path: dir } }).success(function (data) {
            $scope.Folders = data.GetFolders;
            $scope.Files = data.GetFiles;
            $scope.Root = data.GetRoot;
        })
       .error(function () {
           $scope.error = "Some Error.";
       });
    }*/
});