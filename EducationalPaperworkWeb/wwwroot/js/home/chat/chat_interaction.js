function updateChat(data) {
    var previousDate = new Date(2024, 2, 1);
    var messages = data.messages;
    var companion = data.companion;
    var messageHtmlTime = '';
    var messageHtml = '';
    var sender = '';
    $('#companion-name').text(companion);
    $('.msg_card_body').html('');

    messages.forEach(function (message) {
        var messageDate = getMessageDate(message.timeStamp);
        var time = new Date(message.timeStamp);
        var messageTime = time.toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit' });

        if (messageDate !== previousDate) {
            messageHtmlTime = getDateFormHtml(messageDate);
            $('.msg_card_body').append(messageHtmlTime);
            previousDate = messageDate;
        }

        sender = message.senderId == senderId ? "justify-content-end" : "justify-content-start";

        if (message.type === 1) {
            messageHtml = getTextMessageHtml(message.content, messageTime, sender)
        }
        else if (message.type === 2) {
            messageHtml = getFileMessageHtml(message.content, messageTime, sender);
        }

        $('.msg_card_body').append(messageHtml);
    });
}

function selectChat(window) {
    var value = window.val();

    if (value !== selectedChatId) {
        selectedChatId = value;

        if (previousClickedBtn !== null) {
            $(previousClickedBtn).removeClass('selected');
        }

        $(window).addClass('selected');
        previousClickedBtn = window;

        getChatData();
    }
}

function getChatData() {
    $.ajax({
        url: '/Home/GetChat',
        method: 'POST',
        data: {
            userId: senderId,
            chatId: selectedChatId
        },
        success: function (data, textStatus, xhr) {
            handleChatDataResponse(data, textStatus, xhr);
        }
    });
}

function handleChatDataResponse(data, textStatus, xhr) {
    if (xhr.status === 204) {
        hideMessageForm();
        var newMessageHtml = getWaitFormHtml();
        $('.msg_card_body').html(newMessageHtml);
    } else {
        showMessageForm();
        updateChat(data);
        var element = document.querySelector('.msg_card_body');
        element.scrollTop = element.scrollHeight;
        var messContainer = document.querySelector('#inputMessage');
        messContainer.focus();
    }
}

function getMessageDate(Time) {
    var messageDate = new Date(Time);
    var currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);
    messageDate.setHours(0, 0, 0, 0);
    var diffTime = Math.abs(currentDate - messageDate);
    var diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    if (diffDays === 0) {
        return "Cьогодні";
    } else if (diffDays === 1) {
        return "Вчора";
    } else if (diffDays > 1 && diffDays <= 365) {
        var options = { day: 'numeric', month: 'long' };
        return messageDate.toLocaleDateString('uk-UA', options);
    } else if (diffDays > 365) {
        var options = { day: 'numeric', month: 'long', year: 'numeric' };
        return messageDate.toLocaleDateString('uk-UA', options);
    }
}

function removeChatFocus() {
    $(previousClickedBtn).removeClass('selected');
    previousClickedBtn = null;
}

function setGreetings() {
    $('#myModal').css('display', 'none');
    var html = getClientGreetingsFormHtml();
    $('.msg_card_body').html(html);
}