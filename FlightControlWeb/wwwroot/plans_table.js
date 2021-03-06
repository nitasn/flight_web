﻿"use strict";


function GET(url, success, failure) {

    fetch(url)
        .then(response => response.text())
        .then(msg => success(msg))
        .catch(err => failure(err));
}

window.glb_flight_ids = new Array();

function formatDate(date) {
    date = new Date(date);
    const hours = date.getHours();
    let minutes = date.getMinutes();
    minutes = minutes < 10 ? '0' + minutes : minutes;
    const strTime = hours + ':' + minutes;
    return date.getDate() + "/" + (date.getMonth() + 1) + "/"
        + date.getFullYear() + "<br/>" + strTime;
}

function does_plans_place_holder_exist() {

    return (document.getElementById("plans-place-holder") != null);
}

function remove_plans_place_holder() {

    const row = document.getElementById("plans-place-holder");

    if (row != null) {

        row.parentNode.removeChild(row);
    }
}

function add_placeholder_to_table() {

    const tr = document.createElement('tr');

    tr.setAttribute("id", "plans-place-holder");

    tr.innerHTML = `
        <td colspan="3"">
            upload some flights!
        </td>
    `;

    document.getElementById('flights-table-body').append(tr);
}

function clear_details() {

    const tags = [
        "flight_id",
        "longitude",
        "latitude",
        "passengers",
        "date_time",
        "is_external",
    ];

    for (const tag of tags) {
        const element = document.getElementById('cell_' + tag);
        element.innerHTML = '';
    }
}

function show_details(flight) {

    const tags = [
        "flight_id",
        "passengers",
        "is_external",
    ];

    for (const tag of tags) {
        const element = document.getElementById('cell_' + tag);
        element.innerHTML = flight[tag];
    }

    const takeoff = document.getElementById('cell_date_time');
    takeoff.innerHTML = formatDate(flight['date_time']);

    const lat = document.getElementById('cell_latitude');
    lat.innerHTML = flight['latitude'].toFixed(5);

    const lng = document.getElementById('cell_longitude');
    lng.innerHTML = flight['longitude'].toFixed(5);
}

function delete_row_from_table(flight_id) {

    const index = glb_flight_ids.indexOf(flight_id);

    if (index >= 0) {

        glb_flight_ids.splice(index, 1);

        const row = document.getElementById(`id_${flight_id}`);

        if (row != null) {

            const prnt = row.parentNode;
            prnt.parentNode.removeChild(prnt);
        }

        if (glb_flight_ids.length == 0)
            add_placeholder_to_table();

        if (flight_id == document.getElementById('cell_flight_id').innerHTML)
            clear_details();
    }
}

function send_delete_request(flight_id) {

    const url = `/api/Flights/${flight_id}`;

    fetch(url, { method: 'DELETE' })
        .then(res => {
            if (!res.ok) {
                alert(`deletion error: response status ${res.status}`);
            }
        }).catch(err => alert(`deletion error: got error ${err}`));
}

function add_row_to_table(flight) {

    const table = document.getElementById('flights-table-body');

    const underlinable = `role="button"
        onmouseout="event.target.setAttribute('style',
            'text-decoration: normal')"
        onmouseover="event.target.setAttribute('style',
            'text-decoration: underline')"`;

    const tr = document.createElement('tr');
    tr.setAttribute("id", 'tr_' + flight.flight_id);

    if (!flight.is_external) {
        tr.innerHTML = `
            <th ${underlinable} scope='row' id='id_${flight.flight_id}'>
                ${flight.flight_id}
            </th>
            <td>${flight.company_name}</td>
            <td ${underlinable} id='rm_${flight.flight_id}'>
                remove
            </td>
        `;
    }
    else {
        tr.innerHTML = `
            <th ${underlinable} scope='row' id='id_${flight.flight_id}'>
                ${flight.flight_id}
            </th>
            <td>${flight.company_name}</td>
            <td>
                <i>(external)</i>
            </td>
        `;
    }

    table.append(tr);

    document.getElementById(`id_${flight.flight_id}`)
        .addEventListener("click", function () {

            if (ID_toShowDetailsOf in glb_airplane_markers)
                glb_airplane_markers[ID_toShowDetailsOf]
                    .setIcon(regular_plane_icon_url);

            ID_toShowDetailsOf = flight.flight_id;

            glb_airplane_markers[ID_toShowDetailsOf]
                .setIcon(selected_plane_icon_url);
        });

    if (!flight.is_external) {
        document.getElementById(`rm_${flight.flight_id}`)
            .addEventListener("click", function () {

                if (ID_toShowDetailsOf in glb_airplane_markers)
                    glb_airplane_markers[ID_toShowDetailsOf]
                        .setIcon(regular_plane_icon_url);

                send_delete_request(flight.flight_id);
            });
    }
}

///* when page is ready, do */
//$(function () {

//    add_plans_place_holder();
//});

//(function () { 

//    add_plans_place_holder();
//}());