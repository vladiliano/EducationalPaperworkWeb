function addMessage(messageContent, isToday) {
    var currentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit' });
    var messageHtml = '';

    if (isToday) {
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
    currentDate.setHours(0, 0, 0, 0);
    messageTime.setHours(0, 0, 0, 0);
    var diffTime = Math.abs(currentDate - messageTime);
    var diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

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
    var previousDate = new Date(2024, 2, 1);
    var messages = data.messages;
    var companion = data.companion;
    var messageHtml = '';
    var messageHtmlTime = '';
    $('#companion-name').text(companion);
    $('.msg_card_body').html('');

    messages.forEach(function (message) {
        var messageTime = new Date(message.timeStamp);
        var messTime = messageTime.toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit' });
        var messageDate = getMessageDate(messageTime);

        if (messageDate !== previousDate) {
            messageHtmlTime = `<div class="msg_date">${messageDate}</div>`;
            previousDate = messageDate;
        }

        var messageHtml = `
        <div class="d-flex ${(message.senderId == senderId) ? "justify-content-end" : "justify-content-start"} mb-4">
            <div class="msg_container" id="mess">
                ${message.content}
                <span class="msg_time">${messTime}</span>
            </div>
        </div>
    `;

        var result = '';

        if (messageHtmlTime != null) {
            result = messageHtmlTime;
        }
        result += messageHtml;

        $('.msg_card_body').append(result);
        messageHtmlTime = null;
        messageHtml = null;
    });
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
            var isToday = false;
            if (xhr.status !== 204) {
                var todayDate = new Date().getDate();
                var mess = new Date(data.timeStamp);
                var messDate = mess.getDate();
                if (messDate !== todayDate) {
                    isToday = true;
                }
            }
            else if (xhr.status === 204) {
                isToday = true;
            }
            addMessage(messageContent, isToday);
            var element = document.querySelector('.msg_card_body');
            element.scrollTop = element.scrollHeight;
        }
    });
}

var previousClickedBtn = null;
var chatId;

function selectChat(window) {
    var value = window.val();
    if (value !== chatId) {
        chatId = value;

        if (previousClickedBtn !== null) {
            $(previousClickedBtn).removeClass('selected');
        }

        $(window).addClass('selected');
        previousClickedBtn = window;

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
                    $('#sendMessageForm').css('visibility', 'visible');
                    $('#card-header').css('visibility', 'visible');
                    updateChat(senderId, data);
                    var element = document.querySelector('.msg_card_body');
                    element.scrollTop = element.scrollHeight;
                }
            }
        });
    }
}

function removeChatFocus() {
    $(previousClickedBtn).removeClass('selected');
    previousClickedBtn = null;
}

function addChat(chatName) {
    $.ajax({
        url: '/Home/CreateChat',
        method: 'POST',
        data: {
            senderId: senderId,
            chatName: chatName
        },
        success: function (data) {
            var html = $('#chat-list').html();
            var messageHtml = `
                <button value="${data.id}" class="chat_btn">${data.name}</button>
            `;
            var newMessageHtml = messageHtml + html;
            $('#chat-list').html(newMessageHtml);
        }
    });
}

function setModal() {
    var html = `                    
    <div id="myModal" class="modal">
        <div class="modal-content">
            <span id="close-modal" class="close">&times;</span>
            <p class="modal-text">Вкажіть тему звернення</p>
            <input type="text" id="textInput" class="modal-input" minlength="5" maxlength="25">
            <span id="errorText" class="text-danger validation-message"></span>
            <button id="sendButton" class="modal-button">Надіслати</button>
        </div>
    </div>
    `;
    $('#sendMessageForm').css('visibility', 'hidden');
    $('#card-header').css('visibility', 'hidden');
    $('.msg_card_body').html(html);
}

function processTextInput() {
    var textInputValue = $('#textInput').val();
    if (textInputValue.length < 5 || textInputValue.length > 25) {
        $('#errorText').text('Тема звернення повина мати більше 5 символів.');
        return;
    }
    addChat(textInputValue);
    $('#myModal').css('display', 'none');
}

function AddFile() {
    var fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.style.display = 'none';
    $(fileInput).click();

    $(fileInput).change(function () {
        var formData = new FormData();
        formData.append('file', $(this)[0].files[0]);

        $(fileInput).click();
        $.ajax({
            url: '/Home/SendFile',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data, textStatus, xhr) {
                if (xhr.status === 204) {

                } else {

                }
            }
        });
    });
}