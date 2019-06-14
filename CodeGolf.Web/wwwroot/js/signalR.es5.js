﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/refreshHub").build();

connection.on("newRound", function () {
    location.reload();
});

connection.start()["catch"](function (err) {
    return console.error(err.toString());
});
