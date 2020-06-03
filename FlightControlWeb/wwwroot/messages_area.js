"use strict";


window.total_messages = 0;


function say_nicely(msg) {

    const paragraph_element = document.getElementById('msg_paragraph');

    paragraph_element.innerHTML = msg;

    const duration_millis = 3000;

    total_messages++;
    const this_msg_id = total_messages;

    setTimeout(function () {

        if (total_messages == this_msg_id) {

            paragraph_element.innerHTML = '';
        }

    }, duration_millis);
}

function clear_msg() {

    const paragraph_element = document.getElementById('msg_paragraph');

    paragraph_element.innerHTML = '';
}