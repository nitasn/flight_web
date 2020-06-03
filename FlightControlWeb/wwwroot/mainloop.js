"use strict";




function loop_forever() {

    if (document.readyState !== "complete") return;

    const success = function (msg) {

        const flights_from_server = JSON.parse(msg);

        let checked_flights = [];
        let showed_details = false;

        for (const flight of flights_from_server) {

            const id = flight.flight_id;

            if (!glb_flight_ids.includes(id)) { /* new flight */

                remove_plans_place_holder(); // if removed alerady, no effect

                add_row_to_table(flight);
                add_path_to_map(flight);

                glb_flight_ids.push(id);
            }
            else { /* updating an existing flight */
                move_marker(flight);
            }

            checked_flights.push(id);

            if (id == ID_toShowDetailsOf) {
                show_details(flight);
                showed_details = true;
            }
        }

        glb_flight_ids
            .filter(id => !checked_flights.includes(id))
            .forEach(id => {
                delete_row_from_table(id);
                delete_paths_from_map(id);
            });

        if (!showed_details || ID_toShowDetailsOf == null) {
            clear_details();
        }
    }

    let time = get_user_selected_time();

    if (time) {

        const url = `api/Flights?relative_to=${time}&sync_all`;

        GET(url, success, on_update_failure);
    }
    else {
        say_nicely('Please Pick a Valid DateTime!');
    }
}

function on_update_failure(err) {

    err = err.toString();
    const fetch_err = err.includes('fetch'); // yikes but works

    if (fetch_err)
        say_nicely("Couldn't update flights (failed to fetch)." +
            "<br /> Server might be down.");
    else
        say_nicely("Couldn't update flights; err msg: <br />" + err);
}


function fatut() {
    // todo delete me
    alert(document.getElementById('rb-real-time').checked);
}

function get_user_selected_time() {

    if (document.getElementById('rb-real-time').checked) {

        return new Date().toISOString(); // real time
    }

    const time_picker = document.getElementById('inpt-specific-time');

    return time_picker.value;
}

/* when page is ready, do */

(function () {
    setInterval(loop_forever, 200);
}());
