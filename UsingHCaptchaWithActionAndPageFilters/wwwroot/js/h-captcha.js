"use strict";

(function () {
    setInterval(hCaptchaChangeDetectorCallback, 1000);
})();
function initializeHCaptcha(sitekey) {   
    $('#hcaptcha-wrapper')
        .append(`<div class="h-captcha" data-sitekey="${sitekey}"></div>`);        
}
function hCaptchaChangeDetectorCallback() {
    const hCaptchaValue = $('[name=h-captcha-response]').val();
    if (hCaptchaValue === "" || hCaptchaValue === undefined) {        
        $('#submit-button').prop('disabled', true);
    }
    else {
        $('#submit-button').prop('disabled', false);
    }
}
