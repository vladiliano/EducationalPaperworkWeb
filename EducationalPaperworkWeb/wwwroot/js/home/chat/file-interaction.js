function AddFile() {
    var fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.style.display = 'none';
    $(fileInput).click();

    $(fileInput).change(function () {
        var formData = new FormData();
        formData.append('file', $(this)[0].files[0]);
        formData.append('userId', senderId);
        formData.append('chatId', selectedChatId);

        $.ajax({
            url: '/Home/UploadFile',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData
        });
    });
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