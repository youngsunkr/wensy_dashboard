//<![CDATA[

var ResetTime = 15000;
function setCookie_main(name, value, exdays) {
    var today = new Date();
    today.setDate(today.getDate() + 1);
    document.cookie = name + "=" + escape(value) + "; path=/;";
}
function emailCheck(email_address) {
    email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,12}$/i;
    if (!email_regex.test(email_address)) {
        alert("이메일 형식에 맞지않습니다.")
        return false;
    } else {
        return true;
    }
}
function passCheck(str) {
    val_regex = /^[a-zA-Z0-9._-]{6,12}$/i;
    if (!val_regex.test(str)) {
        alert("문자열 6~12자리로 입력해주세요");
        return false;
    } else {
        return true;
    }
}
function getCookie(name) {
    var nameOfCookie = name + "=";
    var x = 0;
    while (x <= document.cookie.length) {
        var y = (x + nameOfCookie.length);
        if (document.cookie.substring(x, y) == nameOfCookie) {
            if ((endOfCookie = document.cookie.indexOf(";", y)) == -1)
                endOfCookie = document.cookie.length;
            return unescape(document.cookie.substring(y, endOfCookie));
        }
        x = document.cookie.indexOf(" ", x) + 1;
        if (x == 0)
            break;
    }
    return '';
}

function returnChkFirst(mCode) {
    var oldCookieVal = '';
    var newCookieVal = '';
    //기존 쿠키 값이 없을때(첫로딩)
    if (!getCookie('serverStatus_OLD') || 0 == getCookie('serverStatus_OLD').length) {
        return '0';
    }
    else {
        var tempStr_old = getCookie('serverStatus_OLD').split('&');
        for (i = 0; i < tempStr_old.length; i++) {
            var temp_old = tempStr_old[i].split('=');
            var key_old = temp_old[0];
            var val_old = temp_old[1];

            if (key_old == mCode) {
                oldCookieVal = val_old;
            }
        }

        var tempStr = getCookie('serverStatus').split('&');
        for (i = 0; i < tempStr.length; i++) {
            var temp = tempStr[i].split('=');
            var key = temp[0];
            var val = temp[1];

            if (key == mCode) {
                newCookieVal = val;
            }
        }

        if (oldCookieVal == newCookieVal) {
            return '1';
        }
        else {
            return '2';
        }
    }
}
function win_open_slide(url) {
    if (url.length != 0) {
        window.open(url, '_blank', 'scrollbars=yes resizable=yes width=1200 height=900')
    }
}
function win_open_center(url, name, width, height, scrollbars) {
    var posLt = (screen.availWidth - width) / 2;
    var posTp = (screen.availHeight - height) / 2;
    var pop = window.open(url, name, "top=" + posTp + ",left=" + posLt + ",width=" + width + ",height=" + height + ",scrollbars=" + scrollbars);
    try { pop.focus(); } catch (e) { }
}

function returnServerStatus(mCode) {
    if (0 == getCookie('serverStatus').length) {
        return null;
    }
    else {
        var tempStr = getCookie('serverStatus').split('&');
        for (i = 0; i < tempStr.length; i++) {
            var tempStr01 = tempStr[i].split('=');
            var key = tempStr01[0];
            var val = tempStr01[1];

            if (key == mCode) {
                return val;
            }
        }
    }
}

function chkMenuAuth(mCode) {
    if (0 == getCookie('Authorize').length) {
        return null;
    }
    else {
        var tempStr = getCookie('Authorize').split('&');
        for (i = 0; i < tempStr.length; i++) {
            var tempStr01 = tempStr[i].split('=');
            var key = tempStr01[0];
            var val = tempStr01[1];

            if (key == mCode) {
                return val;
            }
        }
    }
}
function chkGrade() {
    if (0 == getCookie('Authorize').length) {
        return false;
    }
    else {
        var tempStr = getCookie('Authorize');
        if (tempStr == '1')
        { return false; }
        else
        { return true; }
    }
}
function chkAuth() {
    if (!chkGrade()) {
        alert('권한이 없습니다.');
        location.href = '/';
    }
}
function chkDemo() {
    if (!chkGrade()) {
        alert('권한이 없습니다.');
        location.href = '/';
    }
}
function chkAuthLevel(mCode, level) {
    if (!chkMenuAuth(mCode) || level > chkMenuAuth(mCode)) {
        alert('권한이 없습니다.');
        return false;
    }
    return true;
}

function activeMenu(container, key) {
    $('#' + container + ' #' + key).addClass('alert-info');
}

$(document).ready(function () {
    $('.number').each(function () {
        $(this).live("keyup", function (event) {
            var $this = $(this);
            $this.css('ime-mode', 'disabled');
            var value = $this.val().match(/^(-?)[0-9]*$/g);
            if (value == null) {
                $this.val($this.val().replace(/[^0-9]/g, ''));
            }
        });
    });

    $('.date').each(function () {
        $(this).datepicker({ dateFormat: 'yy-mm-dd' });
    });
});
function Func_Reload() {
    location.reload();
    setTimeout(function () { Func_Reload() }, 15000)
}
//]]>