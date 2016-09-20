app.service("myService", function ($http) {
        
    this.getEmployees = function () {
        return $http.get("/Home/getAll");
    };
});