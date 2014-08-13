var app = angular.module("app", ['ngRoute']);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            controller: 'ListController',
            templateUrl: 'list.html'
        })
        .when("/list/:filter?/:page?", {
            controller: 'ListController',
            templateUrl: 'list.html'
        });
        //.otherwise({
        //    redirectTo: '/'
        //});
});

app.service("logs", function ($http) {
    this.getLogs = function (filter, start, count) {
        return $http.get("api/logs", { params: { filter: filter, start: start, count: count } })
        .then(function (response) {
            return response.data;
        },
        function (response) {
            throw (response.data && response.data.message || "Error Getting Logs");
        });
    };
});

app.controller("ListController", function ($scope, logs, $sce, $routeParams, $location) {
    $scope.model = {};
   
    function PagerButton(text, page, enabled, current) {
        this.text = $sce.trustAsHtml(text + "");
        this.page = page;
        this.enabled = enabled;
        this.current = current;
    }

    function Pager(result, pageSize, filter) {
        this.start = result.start;
        this.count = result.count;
        this.total = result.total;
        this.pageSize = pageSize;
        this.filter = filter;

        this.totalPages = Math.ceil(this.total / pageSize);
        this.currentPage = (this.start / pageSize) + 1;
        this.canPrev = this.currentPage > 1;
        this.canNext = this.currentPage < this.totalPages;

        this.buttons = [];

        var totalButtons = 7; // ensure this is odd
        var pageSkip = 10;
        var startButton = 1;
        if (this.currentPage > Math.floor(totalButtons / 2)) startButton = this.currentPage - Math.floor(totalButtons / 2);

        var endButton = startButton + totalButtons - 1;
        if (endButton >= this.totalPages) endButton = this.totalPages;
        if (this.totalPages > totalButtons &&
            (endButton - startButton + 1) < totalButtons) {
            startButton = endButton - totalButtons + 1;
        }

        var prevPage = this.currentPage - pageSkip;
        if (prevPage < 1) prevPage = 1;

        var nextPage = this.currentPage + pageSkip;
        if (nextPage > this.totalPages) nextPage = this.totalPages;

        this.buttons.push(new PagerButton("<strong>&lt;&lt;</strong>", 1, endButton > totalButtons));
        this.buttons.push(new PagerButton("<strong>&lt;</strong>", prevPage, endButton > totalButtons));

        for (var i = startButton; i <= endButton; i++) {
            this.buttons.push(new PagerButton(i, i, true, i === this.currentPage));
        }

        this.buttons.push(new PagerButton("<strong>&gt;</strong>", nextPage, endButton < this.totalPages));
        this.buttons.push(new PagerButton("<strong>&gt;&gt;</strong>", this.totalPages, endButton < this.totalPages));
    }

    $scope.search = function (filter) {
        var url = "/list";
        if (filter) {
            url += "/" + filter;
        }
        $location.url(url);
    };

    var filter = $routeParams.filter;
    $scope.model.message = null;
    $scope.model.filter = filter;
    $scope.model.logs = null;
    $scope.model.pager = null;
    $scope.model.waiting = true;
    

    var itemsPerPage = 10;
    var page = $routeParams.page || 1;
    var startItem = (page - 1) * itemsPerPage;

    logs.getLogs(filter, startItem, itemsPerPage).then(function (result) {
        $scope.model.waiting = false;
        $scope.model.logs = result.logs;
        if (result.logs && result.logs.length) {
            $scope.model.pager = new Pager(result, itemsPerPage, filter);
        }
    }, function (error) {
        $scope.model.message = error;
        $scope.model.waiting = false;
    });

    $scope.model.minDate = new Date();
    $scope.model.fromDate = new Date();
    $scope.model.toDate = new Date();
    $scope.model.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'shortDate'];
    $scope.model.format = $scope.model.formats[0];

    $scope.model.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.model.opened = true;
    };

    $scope.model.dateOptions = {
        'year-format': "'yy'",
        'starting-day': 1
    };
});
