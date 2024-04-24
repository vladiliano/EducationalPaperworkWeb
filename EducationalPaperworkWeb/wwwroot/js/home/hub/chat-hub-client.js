let hubConnection;

document.addEventListener("DOMContentLoaded", function () {

    hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/Hubs/ChatHub")
        .build();

    hubConnection.start().then(function () {
        console.log("Connected to ChatHub");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    hubConnection.on("SendMessageToChat", data => {
        appendMessageToChat(data);
    });

    hubConnection.on("AddRequestToTable", data => {
        addRowToTable(data);
    });

    hubConnection.on("RemoveRequestFromTable", chatId => {
        removeRowFromTable(chatId);
    });
});