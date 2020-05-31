"use strict";


function add_to_my_flights(id, flight_plan, is_local) {

    const list = document.getElementById('file');

    // todo delete button
    const options = is_local ? 'delete' : '[extrnal]';

    list.innerHTML += `
        <tbody id=${id}>
        <tr>
            <th scope="row">${id}</th>
            <td>${flight_plan.company_name}</td>
            <td>${options}</td>
        </tr>
        </tbody>
    `;
}


(function () { /* sends local file to server, then calls add_to_my_flights */

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
                alert(`Uploaded. The new Flight's ID is ${id}`);
                const is_local = true;
                add_to_my_flights(id, flight_plan, is_local);
            }
            else {
                alert("server didn't accept the plan. make sure its correct");
            }
        }

        http.send(JSON.stringify(flight_plan));
    }

    document.getElementById('file').addEventListener('change', onChange);
}());