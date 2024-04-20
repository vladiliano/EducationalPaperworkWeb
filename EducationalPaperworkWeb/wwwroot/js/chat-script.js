var previousClickedBtn = null;
var chatId;
var table;

function getDateMessageHtml(text) {
    return `
        <div class="msg_date">
            ${text}
        </div>
    `;
}

function getWaitMessageHtml() {
    return `
        <div id="hello-world" class="hello-form" style="font-size: 15px; display: flex;">
            Очікується підтвердження звернення від представника дирекції університету
        </div>
    `;
}

function getStudentHelloWorld() {
    return `
    <div class="hello-form">
            Оберіть чат або надішліть звернення для його створення
        </div>
    `;
}

function getCreateChatWindow() {
    return `
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
}

function hideMessageForm() {
    $('#sendMessageForm').css('visibility', 'hidden');
    $('#card-header').css('visibility', 'hidden');
}

function showMessageForm() {
    $('#sendMessageForm').css('visibility', 'visible');
    $('#card-header').css('visibility', 'visible');
}

function getFileName(content) {
    return content.split('*')[1];
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
            messageHtmlTime = getDateMessageHtml(messageDate);
            $('.msg_card_body').append(messageHtmlTime);
            previousDate = messageDate;
        }

        sender = message.senderId == senderId ? "justify-content-end" : "justify-content-start";

        if (message.type === 1) {
            messageHtml = getTextMessage(message.content, messageTime, sender)
        }
        else if (message.type === 2) {
            messageHtml = getFileMessage(message.content, messageTime, sender);
        }

        $('.msg_card_body').append(messageHtml);
    });
}

function sendMessage() {
    var messageContent = $('#inputMessage').val();

    $.ajax({
        url: '/Home/SendMessage',
        method: 'POST',
        data: {
            userId: senderId,
            chatId: chatId,
            mess: messageContent
        }
    });
}

function selectChat(window) {
    var value = window.val();
    if (value !== chatId) {
        var previousChatId = chatId;
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
                userId: senderId,
                chatId: chatId,
                previousChatId: previousChatId
            },
            success: function (data, textStatus, xhr) {
                if (xhr.status === 204) {
                    hideMessageForm();
                    var newMessageHtml = getWaitMessageHtml();
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
        });
    }
}

function removeChatFocus() {
    $(previousClickedBtn).removeClass('selected');
    previousClickedBtn = null;
}

function setModal() {
    var html = getCreateChatWindow();
    hideMessageForm();
    $('.msg_card_body').html(html);
}

function myModalOpen() {
    $('#myModal').css('display', 'block');
    $('#textInput').focus();
}

function createChat(chatName) {
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

function processTextInput() {
    var textInputValue = $('#textInput').val();
    if (textInputValue.length < 5 || textInputValue.length > 25) {
        $('#errorText').text('Тема звернення повина мати більше 5 символів.');
        return;
    }
    createChat(textInputValue);
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
        formData.append('userId', senderId);
        formData.append('chatId', chatId);

        $.ajax({
            url: '/Home/UploadFile',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData
        });
    });
}

function setHelloWorld() {
    $('#myModal').css('display', 'none');
    var html = getStudentHelloWorld();
    $('.msg_card_body').html(html);
}

document.addEventListener('DOMContentLoaded', function () {
    document.querySelector('.msg_card_body').addEventListener('click', function (event) {
        if (event.target && event.target.matches('#file-link')) {
            event.preventDefault();
            const contentValue = event.target.getAttribute('data-content');
            var formData = new FormData();
            formData.append('fileName', contentValue);

            $.ajax({
                url: '/Home/DownloadFile',
                type: 'POST',
                processData: false,
                contentType: false,
                data: formData,
                success: function (data, textStatus, xhr) {
                    var disposition = xhr.getResponseHeader('Content-Disposition');
                    var startIndex = disposition.indexOf("filename*=UTF-8''") + "filename*=UTF-8''".length;
                    var encodedSubstring = disposition.substring(startIndex);
                    var filename = decodeURIComponent(encodedSubstring);
                    var contentType = xhr.getResponseHeader('Content-Type');

                    var blob = new Blob([data], { type: contentType });

                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = filename;

                    document.body.appendChild(link);
                    link.click();

                    document.body.removeChild(link);
                }
            });
        }
    });
});

function setListRequests() {
    $.ajax({
        url: '/Home/GetUnacceptedRequests',
        type: 'GET',
        success: function (data, textStatus, xhr) {
            if (data) {
                removeChatFocus();
                displayData(data);
            }
        }
    });
}

function createTableHeaders(headers) {
    var headerRow = $('<tr>');
    headers.forEach(function (header) {
        headerRow.append($('<th>').text(header).addClass('table_column_header'));
    });
    headerRow.append($('<th>').text('Дії').addClass('table_column_header table_column_header_actions'));
    return headerRow;
}

function formatDateTime(dateTime) {
    var date = new Date(dateTime);
    var formattedTime = date.getHours() + ':' + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes();
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear() % 100;
    var formattedDate = (day < 10 ? '0' : '') + day + '.' + (month < 10 ? '0' : '') + month + '.' + (year < 10 ? '0' : '') + year;
    return formattedTime + ' (' + formattedDate + ')';
}

function createTableRow(item) {
    var row = $('<tr>').addClass('request_row');
    row.append($('<td>').text(formatDateTime(item.item2.timeStamp)));
    row.append($('<td>').text(item.item2.name));
    row.append($('<td>').text(item.item1.surname + ' ' + item.item1.name + ' ' + item.item1.patronymic));
    row.append($('<td>').text(getFacultyString(item.item1.faculty) + ' ' + item.item1.group));

    var button = $('<button>')
        .text('Взяти звернення')
        .addClass('take_appeal_btn')
        .val(item.item2.id);

    row.append($('<td>').append(button));
    return row;
}

function displayData(data) {
    if (data) {
        table = $('<table>').addClass('table requests_table');
        var thead = $('<thead>');
        var tbody = $('<tbody>');

        var headers = ['Час звернення', 'Тема звернення', 'Ім\'я студента', 'Група студента'];
        thead.append(createTableHeaders(headers));
        table.append(thead);

        hideMessageForm();
        $('.msg_card_body').html(table);

        data.forEach(function (item) {
            tbody.append(createTableRow(item));
        });

        table.append(tbody);
    }
}

function getFacultyString(facultyNumber) {
    switch (facultyNumber) {
        case 0: return 'обрати';
        case 1: return 'МІТ';
        case 2: return 'Е';
        case 3: return 'ІКМ';
        case 4: return 'ХТ';
        case 5: return 'БЕМ';
        case 6: return 'КН';
        case 7: return 'СГТ';
        default: return '';
    }
}

function handleTakeAppeal(appealId, adminId) {
    $.ajax({
        url: '/Home/AcceptRequest',
        type: 'POST',
        data: {
            chatId: appealId,
            adminId: adminId
        },
        success: function (data, textStatus, xhr) {
            removeRow(data);
            addChat(data);
        }
    });
}

function removeRow(data) {
    $('.take_appeal_btn').each(function () {
        if ($(this).val() === data.id.toString()) {
            var row = $(this).closest('tr');
            row.fadeOut('slow', function () {
                $(this).next().fadeIn('slow');
                $(this).remove();
            });
        }
    });
}