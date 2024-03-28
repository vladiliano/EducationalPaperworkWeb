function addMessage(messageContent, message) {
    var currentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit' });

    var messageTime = new Date(message.timeStamp);
    var currentDate = new Date();
    var messageHtml = '';

    if (Math.abs(currentDate.getDate() - messageTime.getDate()) > 1) {
        messageHtml = `<div class="msg_date">Cьогодні</div>`;
    }

    var newMessageHtml = `
        <div class="d-flex justify-content-end mb-4">
            <div class="msg_container" id="mess">
                ${messageContent}
                <span class="msg_time">${currentTime}</span>
            </div>
        </div>
    `;
    messageHtml += newMessageHtml;

    $('.msg_card_body').append(messageHtml);
    $('#inputMessage').val('');
}

function getMessageDate(messageTime) {
    var currentDate = new Date();
    var diffDays = Math.abs(currentDate.getDate() - messageTime.getDate());

    var messageDate = "";
    if (diffDays === 0) {
        messageDate = "Cьогодні";
    } else if (diffDays === 1) {
        messageDate = "Вчора";
    } else if (diffDays > 1 && diffDays <= 365) {
        var options = { day: 'numeric', month: 'long' };
        messageDate = messageTime.toLocaleDateString('uk-UA', options);
    } else if (diffDays > 365) {
        var options = { day: 'numeric', month: 'long', year: 'numeric' };
        messageDate = messageTime.toLocaleDateString('uk-UA', options);
    }

    return messageDate;
}

function updateChat(senderId, data) {
    var newMessageHtml = '';
    var previousDate = null;
    var messages = data.messages;
    var companion = data.companion;

    messages.forEach(function (message) {
        var messageTime = new Date(message.timeStamp);
        var messageDate = getMessageDate(messageTime);

        if (messageDate !== previousDate) {
            newMessageHtml += `<div class="msg_date">${messageDate}</div>`;
            previousDate = messageDate;
        }

        var messageTime = messageTime.toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit' });

        var messageHtml = `
        <div class="d-flex ${(message.senderId == senderId) ? "justify-content-end" : "justify-content-start"} mb-4">
            <div class="msg_container" id="mess">
                ${message.content}
                <span class="msg_time">${messageTime}</span>
            </div>
        </div>
    `;
        newMessageHtml += messageHtml;
    });

    $('.msg_card_body').html(newMessageHtml);
    $('#companion-name').text(companion);
}

function sendMessage() {
    var messageContent = $('#inputMessage').val();

    $.ajax({
        url: '/Home/SendMessage',
        method: 'POST',
        data: {
            senderId: senderId,
            chatId: chatId,
            mess: messageContent
        },
        success: function (data, textStatus, xhr) {
            if (xhr.status !== 204) {
                addMessage(messageContent, data);
            }
        }
    });
}

function selectChat(senderId, chatId) {
    if (previousClickedBtn !== null) {
        $(previousClickedBtn).removeClass('selected');
    }

    $(this).addClass('selected');
    previousClickedBtn = this;

    $.ajax({
        url: '/Home/LoadChat',
        method: 'POST',
        data: {
            senderId: senderId,
            chatId: chatId
        },
        success: function (data, textStatus, xhr) {
            if (xhr.status === 204) {
                $('#sendMessageForm').css('visibility', 'hidden');
                $('#card-header').css('visibility', 'hidden');
                var newMessageHtml = `
                    <div id="hello-world" class="hello-form" style="font-size: 15px; display: flex;">
                        Очікується підтвердження звернення від представника дирекції університету
                    </div>`;
                $('.msg_card_body').html(newMessageHtml);
            } else {
                updateChat(senderId, data);
                $('#sendMessageForm').css('visibility', 'visible');
                $('#card-header').css('visibility', 'visible');
            }
        }
    });
}