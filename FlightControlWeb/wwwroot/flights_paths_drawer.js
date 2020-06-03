"use strict";

window.glb_path_drawings = new Object(); // dictionary: id -> list of lines
window.glb_airplane_markers = new Object(); // dictionary: id -> single marker

function delete_paths_from_map(id) {

    for (const path of glb_path_drawings[id]) {

        remove_from_map(path);
    }
    delete glb_path_drawings[id];

    remove_from_map(glb_airplane_markers[id]);
    delete glb_airplane_markers[id];
};

function move_marker(flight) {

    const location = new google.maps.LatLng(flight.latitude, flight.longitude);
    const marker = glb_airplane_markers[flight.flight_id];

    moveMarkerTo(marker, location);
}

function add_path_to_map(flight) {

    const id = flight.flight_id;

    const success = function (msg) {

        const plan = JSON.parse(msg);

        const segments = plan.segments;

        segments.unshift(plan.initial_location);

        let paths = [];

        for (let i = 0; i < segments.length - i; i++) {

            const from = segments[i];
            const to = segments[i + 1];

            paths.push(draw_line_on_map(
                { lat: from.latitude, lng: from.longitude },
                { lat: to.latitude, lng: to.longitude }));

        }

        glb_path_drawings[id] = paths;

        glb_airplane_markers[id] = draw_airplane_icon_on_map(
            { lat: flight.latitude, lng: flight.longitude },
            id,
            function () { /* onclick */
                if (ID_toShowDetailsOf != null)

                    if (ID_toShowDetailsOf in glb_airplane_markers)
                        glb_airplane_markers[ID_toShowDetailsOf]
                            .setIcon(regular_plane_icon_url);

                ID_toShowDetailsOf = id;
                glb_airplane_markers[id].setIcon(selected_plane_icon_url);
            }
        );
    }

    const failure = function (err) {
        say_nicely(`couldn't get flight ${id} plan; ${err}`);
    }

    const url = `api/FlightPlan/${id}`;

    GET(url, success, failure);
}