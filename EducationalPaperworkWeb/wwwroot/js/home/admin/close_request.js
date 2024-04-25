function closeRequest() {
    $.ajax({
        url: '/Home/CloseRequest',
        type: 'POST',
        data: {
            chatId: selectedChatId
        },
        success: function (data, textStatus, xhr) {

        }
    });
}