var GridObject = {};

function GridLoad(e) {

    var _this = this;
    var gridName = _this.wrapper.attr("id");
    var gridWrapper = _this.wrapper;
    var gridSource = _this.dataSource.data();
    _this.wrapper.removeAttr("tabIndex");
    _this.wrapper.find('[data-task="Delete"],[data-task="Update"],[data-task="Detail"]').addClass("hide");
    _this.wrapper.parents('.k-grid').find(".k-header.k-grid-toolbar .k-button").attr("data-id", "");

    GridObject[gridName] = {
        gridWrapper: gridWrapper,
        gridSelectedUids: [],
        gridSelectedObjects: [],
        gridSource: gridSource
    };

    gridWrapper.on("dblclick", 'tbody [role="row"]', function () {

        if (gridWrapper.find('[data-task="Detail"]').length > 0) {
            gridWrapper.find('[data-task="Detail"]').trigger("click");
        } else {
            gridWrapper.find('[data-task="Update"]').trigger("click");
        }

    })

    gridWrapper.on("change", '[data-event="GridSelector"]', function (event) {
        var gridSelectedRows = _this.select();
        var gridSelectedRow = $(this).parents("tr");
        var gridSelectedNotElem = gridSelectedRows.filter(function (i, elem) {
            return $(elem).attr("data-uid") != gridSelectedRow.attr("data-uid");
        });

        if (this.checked) {
            _this.select([$(this).parents("tr")]);
        } else {
            _this.clearSelection();
            //if(multiple) Multiple olayına göre burda işlem yapılacak
            _this.select(gridSelectedNotElem);
        }
    });


    $("#" + gridName).trigger("load:grid", this.dataSource);
}

function GridChange(e) {

    var _this = this;
    var gridName = _this.wrapper.attr("id");
    var gridWrapper = _this.wrapper;
    var selectionid = $(gridWrapper).attr("data-selectionid")
    var gridSelectedUids = _this.select().map(function (i, elem) { return _this.dataItem(elem).uid; }).toArray();
    var gridSelectedIds = _this.select().map(function (i, elem) { return _this.dataItem(elem)[selectionid ? selectionid : "id"] }).toArray();
    var gridSelectedObjects = _this.select().map(function (i, elem) { return _this.dataItem(elem) }).toArray();
    var gridSource = _this.dataSource.data();

    GridObject[gridName] = {
        gridWrapper: gridWrapper,
        gridSelectedUids: gridSelectedUids,
        gridSelectedIds: gridSelectedIds,
        gridSelectedObjects: gridSelectedObjects,
        gridSource: gridSource
    };

    gridWrapper.find('[data-event="GridSelector"]').prop("checked", false);
    //if(multiple) Multiple olayına göre burda işlem yapılacak
    $.each(_this.select(), function () {
        $(this).find('[data-event="GridSelector"]').prop("checked", true);
    })



    var adet = gridSelectedIds.length;

    if (adet == 0) {
        gridWrapper.find(".k-header.k-grid-toolbar .k-button").attr("data-id", "");
        gridWrapper.find('[data-task="Update"],[data-task="Detail"],[data-task="Delete"]').addClass("hide");
    } else if (adet == 1) {
        gridWrapper.find(".k-header.k-grid-toolbar .k-button").attr("data-id", gridSelectedIds.join(","));
        gridWrapper.find('[data-task="Update"],[data-task="Detail"],[data-task="Delete"]').removeClass("hide");
    } else {
        gridWrapper.find(".k-header.k-grid-toolbar .k-button").attr("data-id", gridSelectedIds.join(","));
        gridWrapper.find('[data-task="Update"],[data-task="Detail"]').addClass("hide");
        gridWrapper.find('[data-task="Delete"]').removeClass("hide");
    }

    $("#" + gridName).trigger("selected:grid", gridSelectedObjects);
}
