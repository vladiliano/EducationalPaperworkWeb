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

function handleTakeAppeal(appealId, adminId) {
    $.ajax({
        url: '/Home/AcceptRequest',
        type: 'POST',
        data: {
            chatId: appealId,
            adminId: adminId
        },
        success: function (data, textStatus, xhr) {
            addChat(data);
        }
    });
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

function removeRowFromTable(chatId) {
    $('.take_appeal_btn').each(function () {
        if ($(this).val() === chatId.toString()) {
            var row = $(this).closest('tr');
            row.fadeOut('slow', function () {
                $(this).next().fadeIn('slow');
                $(this).remove();
            });
        }
    });
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

function addRowToTable(data) {
    var tbody = $('<tbody>');
    tbody.append(createTableRow(data));
    table.append(tbody);
}