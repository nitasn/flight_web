"use strict";



async function uploadFile(file) {
    alert("debug uploading");
    let reader = await new FileReader();
    await reader.readAsText(file);

    reader.onload = async function (event) {
        let plan_str = event.target.result;
        let plan_json;

        try {
            plan_json = JSON.parse(plan_str); // todo try catch
        }
        catch {
            alert('upload error: could not parse json!');
            return;
        }

        alert('about to send ' + plan_str);

        const url = '/api/FlightPlan';

        const http = new XMLHttpRequest()
        http.open('POST', url);
        http.setRequestHeader('Content-type', 'application/json');
        http.send(plan_str); // Make sure to stringify
        http.onload = function () {
            alert(http.responseText);
        };
    };
}




function on_drag(event) {
    var files = event.target.files;
    var file = files[0];
    upload_flight_plan(file);
    //uploadFile(file);
};

async function upload_flight_plan(file) {

    let reader = await new FileReader();
    await reader.readAsText(file);

    reader.onload = async function (event) {
        let url = '/api/FlightPlan';
        let content_str = event.target.result;
        let content_json = JSON.parse(content_str);

        fetch(url,
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: content_str
            })
            .then(alert("flight plan has been uploaded"))
            .catch(err => alert(err));
    };
};

function greet(name) {
    alert(`hey ${name}!`);
    const url = '/api/FlightPlan';
    get_that_works(url, data => {
        alert('got data: ' + data);
    });
};

function get_that_works(url, callback) {
    fetch(url)
        .then(response => {
            if (response.status !== 200)
                console.log('Fetch Error. Status Code: ' + response.status);
            else
                response.text().then(callback);
        })
        .catch(err => {
            console.log('Fetch Error :' + err);
        });
};

function five_times_a_second() {

}

/* when page is ready, start calling five_times_a_second */
$(function () { setInterval(five_times_a_second, 200); });