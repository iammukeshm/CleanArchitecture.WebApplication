function ActivateUser(userId) {
    var taxRate = 0;
    $.ajax({
        type: "POST",
        url: "/api/admin/activate/" + userId,
        dataType: "json",
        contentType: "application/json",
        async: false,
        cache: false,
        success: function (jsonData) {},
        error: function () { }

    });
    return taxRate;
}