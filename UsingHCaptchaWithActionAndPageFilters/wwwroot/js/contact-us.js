"use strict";

function submitContact() {
    if (!$("#contact-form").valid()) {
        return;
    }

    const url = $("#contact-form").attr("action");
    const requestVerificationToken = $('[name=__RequestVerificationToken]').val();
    const hCaptchaResponse = $('[name=h-captcha-response]').val();
    const fullNameValue = $('#FullName').val();
    const emailValue = $('#Email').val();
    const webAddressValue = $('#WebAddress').val();
    const bodyValue = $('#Body').val();

    const formData = new FormData();
    formData.append("__RequestVerificationToken", requestVerificationToken);
    formData.append("h-captcha-response", hCaptchaResponse);    
    formData.append("fullName", fullNameValue);
    formData.append("email", emailValue);
    formData.append("webAddress", webAddressValue);
    formData.append("body", bodyValue);

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            alert("Your message was sent successfully.");
        },
        error: function (req, status, error) {
            alert("Your message was not sent! Please try again later.");
        }
    });
}
