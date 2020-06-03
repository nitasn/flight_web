"use strict";


(function () { // sends local flight-plan json-file to server

    function onChange(event) {
        const reader = new FileReader();
        reader.onload = file_loaded;
        reader.readAsText(event.target.files[0]);
    }

    function file_loaded(event) {
        try {
            const obj = JSON.parse(event.target.result);
            upload_it(obj);
        }
        catch {
            alert("umm... json file couldn't be parsed. make sure it's valid!");
        }
    }

    function upload_it(flight_plan) {

        const url = '/api/FlightPlan';

        const http = new XMLHttpRequest()
        http.open('POST', url);
        http.setRequestHeader('Content-type', 'application/json');

        http.onload = function () {
            if (http.readyState == 4 && http.status == 200) {
                const id = http.responseText;
                // todo green nice label 'flight added'
                // the five_times_a_second_loop will add it to the list!
            }
            else {
                alert("server didn't accept the plan. make sure its correct");
            }
        }

        http.send(JSON.stringify(flight_plan));
    }

    document.getElementById('file').addEventListener('change', onChange);
}());
