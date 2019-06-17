﻿import * as signalR from '@aspnet/signalr';
import * as bulmaToast from 'bulma-toast';

const connection = new signalR.HubConnectionBuilder().withUrl("/refreshHub").build();

connection.on("newRound", function () {
    location.reload();
});

connection.on("newTopScore", function (name, score, avatarUri) {
    bulmaToast.toast({
        message: `New Top Score! <figure class="image is-48x48" src="${avatarUri}"><img/></figure> Name: ${name}, Score: ${score}`,
        type: "is-info",
        dismissible: true,
        pauseOnHover: true,
        duration: 5000,
    });
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});