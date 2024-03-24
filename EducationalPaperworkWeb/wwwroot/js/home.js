document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("inputMessage").addEventListener("keyup", function (event) {
        if (event.key === "Enter") {
            document.querySelector(".send_btn").click();
        }
    });
});