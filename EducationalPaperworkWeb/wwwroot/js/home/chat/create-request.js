﻿function createChat(chatName) {
    $.ajax({
        url: '/Home/CreateChat',
        method: 'POST',
        data: {
            userId: senderId,
            chatName: chatName
        },
        success: function (data) {
            addChat(data);
        }
    });
}

function addChat(data) {
    var html = $('#chat-list').html();
    var messageHtml = `
                <button value="${data.id}" class="chat_btn">${data.name}</button>
            `;
    var newMessageHtml = messageHtml + html;
    $('#chat-list').html(newMessageHtml);
    var element = $('#chat-list').find('[value="' + data.id + '"]');
    element.click();
}